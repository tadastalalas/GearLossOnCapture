using GearLossOnCapture.Core;
using GearLossOnCapture.Diagnostics;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;
using TaleWorlds.CampaignSystem.Roster;

namespace GearLossOnCapture.Behaviors;

// GLOC-FEATURE: Captured heroes always lose weapons and horse, then armor in a fixed order, to their captor; heroes released in rags get new gear; executed heroes' gear goes to the executioner | gate: always (each part has its own setting)
internal sealed class GearLossBehavior : CampaignBehaviorBase
{
    private const string RagsId = "tattered_rags";

    private static readonly EquipmentIndex[] AlwaysTaken =
    {
        EquipmentIndex.Weapon0, EquipmentIndex.Weapon1, EquipmentIndex.Weapon2, EquipmentIndex.Weapon3,
        EquipmentIndex.ExtraWeaponSlot, EquipmentIndex.Horse, EquipmentIndex.HorseHarness,
    };

    // Stripping order. Body armor and boots are one step.
    private static readonly EquipmentIndex[][] ArmorSteps =
    {
        new[] { EquipmentIndex.Head },
        new[] { EquipmentIndex.Gloves },
        new[] { EquipmentIndex.Cape },
        new[] { EquipmentIndex.Body, EquipmentIndex.Leg },
    };

    public override void RegisterEvents()
    {
        CampaignEvents.HeroPrisonerTaken.AddNonSerializedListener(this, OnHeroPrisonerTaken);
        CampaignEvents.HeroPrisonerReleased.AddNonSerializedListener(this, OnHeroPrisonerReleased);
        CampaignEvents.BeforeHeroKilledEvent.AddNonSerializedListener(this, OnBeforeHeroKilled);
    }

    public override void SyncData(IDataStore dataStore) { }

    private static void OnHeroPrisonerTaken(PartyBase captor, Hero hero)
    {
        if (hero == null || captor == null || !Applies(hero)) return;

        if (IsPlayerCaptor(captor) && !ModSettings.PlayerCaptorTakes)
        {
            GlocLog.Debug($"{hero.Name} captured by {captor.Name}: gear spared (player captor takes nothing).");
            return;
        }

        bool takesArmor = CaptorTakesArmor(captor, hero);
        GlocLog.Debug($"{hero.Name} captured by {captor.Name}: weapons taken, armor {(takesArmor ? "taken" : "spared")}.");

        int taken = 0;
        if (ModSettings.LoseBattle) taken += Strip(hero.BattleEquipment, captor, takesArmor);
        if (ModSettings.LoseCivilian) taken += Strip(hero.CivilianEquipment, captor, takesArmor);
        if (ModSettings.LoseStealth) taken += Strip(hero.StealthEquipment, captor, takesArmor);
        GlocLog.Debug($"{taken} item(s) taken from {hero.Name}.");
    }

    private static void OnHeroPrisonerReleased(Hero hero, PartyBase party, IFaction captorFaction, EndCaptivityDetail detail, bool showNotification)
    {
        if (hero == null || !ModSettings.ReequipOnRelease) return;

        if (InRags(hero.BattleEquipment)) Reequip(hero, hero.BattleEquipment, Equipment.EquipmentType.Battle);
        if (InRags(hero.CivilianEquipment)) Reequip(hero, hero.CivilianEquipment, Equipment.EquipmentType.Civilian);
        if (InRags(hero.StealthEquipment)) Reequip(hero, hero.StealthEquipment, Equipment.EquipmentType.Stealth);
    }

    private static void OnBeforeHeroKilled(Hero victim, Hero killer, KillCharacterAction.KillCharacterActionDetail detail, bool showNotification)
    {
        if (victim == null || !ModSettings.ExecutionTakesAll) return;
        if (detail != KillCharacterAction.KillCharacterActionDetail.Executed && detail != KillCharacterAction.KillCharacterActionDetail.ExecutionAfterMapEvent) return;

        // Executioner's party; falls back to the party holding the prisoner if the killer has none.
        ItemRoster? roster = (killer?.PartyBelongedTo?.Party ?? victim.PartyBelongedToAsPrisoner)?.ItemRoster;
        if (roster == null)
        {
            GlocLog.Debug($"{victim.Name} executed: no party inventory to receive the gear.");
            return;
        }

        int taken = TakeAll(victim.BattleEquipment, roster);
        taken += TakeAll(victim.CivilianEquipment, roster);
        taken += TakeAll(victim.StealthEquipment, roster);
        GlocLog.Debug($"{victim.Name} executed: {taken} item(s) taken.");
    }

    private static int TakeAll(Equipment eq, ItemRoster roster)
    {
        if (eq == null) return 0;
        int taken = 0;
        for (int i = 0; i < (int)EquipmentIndex.NumEquipmentSetSlots; i++)
        {
            if (eq[i].Item == null) continue;
            roster.AddToCounts(eq[i], 1);
            eq[i] = EquipmentElement.Invalid;
            taken++;
        }

        return taken;
    }

    private static bool Applies(Hero hero)
    {
        if (hero == Hero.MainHero) return ModSettings.Player;
        if (hero.Clan != Clan.PlayerClan) return ModSettings.AI;
        return hero.PartyBelongedTo?.IsCaravan == true ? ModSettings.PlayerCaravanMasters : ModSettings.PlayerClan;
    }

    private static bool IsPlayerCaptor(PartyBase captor) => captor == PartyBase.MainParty || captor.LeaderHero == Hero.MainHero;

    private static bool CaptorTakesArmor(PartyBase captor, Hero hero)
    {
        if (IsPlayerCaptor(captor)) return MBRandom.RandomFloat < ModSettings.Chance;
        if (ModSettings.BanditsAlways && captor.MapFaction?.IsBanditFaction == true) return true;

        Hero leader = captor.LeaderHero;
        if (leader != null)
        {
            int honor = leader.GetTraitLevel(DefaultTraits.Honor);
            if (ModSettings.HonorableSpares && honor > 0) return false;
            if (ModSettings.MercifulSpares && leader.GetTraitLevel(DefaultTraits.Mercy) > 0) return false;
            if (ModSettings.RelationSpares && leader.GetRelation(hero) >= ModSettings.SpareRelation) return false;
            if (ModSettings.DishonorableAlways && honor < 0) return true;
        }

        return MBRandom.RandomFloat < ModSettings.Chance;
    }

    // Weapons, banner, horse and harness always go. Armor goes step by step in ArmorSteps order;
    // a step with nothing worth taking is skipped without a roll, the first failed roll stops the stripping.
    private static int Strip(Equipment eq, PartyBase captor, bool takesArmor)
    {
        if (eq == null) return 0;

        int taken = 0;
        foreach (EquipmentIndex slot in AlwaysTaken) taken += Take(eq, slot, captor, 0);
        if (!takesArmor) return taken;

        int minItemValue = ModSettings.MinItemValue;
        float stepChance = ModSettings.SlotChance;
        int step = 0;
        for (; step < ArmorSteps.Length; step++)
        {
            if (!HasTakeable(eq, ArmorSteps[step], minItemValue)) continue;
            if (MBRandom.RandomFloat >= stepChance) break;
            foreach (EquipmentIndex slot in ArmorSteps[step]) taken += Take(eq, slot, captor, minItemValue);
        }

        // Rags only once the body step was reached and left the hero without body armor.
        if (step < ArmorSteps.Length || eq[EquipmentIndex.Body].Item != null) return taken;
        ItemObject rags = MBObjectManager.Instance.GetObject<ItemObject>(RagsId);
        if (rags != null) eq[EquipmentIndex.Body] = new EquipmentElement(rags);
        else GlocLog.Warn($"Item '{RagsId}' was not found; the stripped hero was left without body armor.");
        return taken;
    }

    private static bool HasTakeable(Equipment eq, EquipmentIndex[] slots, int minItemValue)
    {
        foreach (EquipmentIndex slot in slots)
        {
            if (IsTakeable(eq[slot].Item, minItemValue)) return true;
        }

        return false;
    }

    private static bool IsTakeable(ItemObject? item, int minItemValue) => item != null && item.StringId != RagsId && item.Value >= minItemValue;

    private static int Take(Equipment eq, EquipmentIndex slot, PartyBase captor, int minItemValue)
    {
        if (!IsTakeable(eq[slot].Item, minItemValue)) return 0;

        captor.ItemRoster?.AddToCounts(eq[slot], 1);
        eq[slot] = EquipmentElement.Invalid;
        return 1;
    }

    private static bool InRags(Equipment eq) => eq?[EquipmentIndex.Body].Item?.StringId == RagsId;

    private static void Reequip(Hero hero, Equipment eq, Equipment.EquipmentType type)
    {
        Equipment? fresh = Campaign.Current.Models.EquipmentSelectionModel?.GetEquipmentForHeroComeOfAge(hero, type);
        if (fresh == null || fresh[EquipmentIndex.Body].Item == null)
        {
            GlocLog.Debug($"{hero.Name} released in rags: no new {type} equipment was generated.");
            return;
        }

        eq.FillFrom(fresh, true);
        GlocLog.Debug($"{hero.Name} released in rags: received new {type} equipment.");
    }
}
