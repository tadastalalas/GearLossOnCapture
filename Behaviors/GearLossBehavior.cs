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

// GLOC-FEATURE: Captured heroes lose gear to their captor, heroes released in rags get new gear, executed heroes' gear goes to the executioner | gate: always (each part has its own setting)
internal sealed class GearLossBehavior : CampaignBehaviorBase
{
    private const string RagsId = "tattered_rags";

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

        bool takes = CaptorTakes(captor, hero);
        GlocLog.Debug($"{hero.Name} captured by {captor.Name}: gear {(takes ? "taken" : "spared")}.");
        if (!takes) return;

        int taken = 0;
        if (ModSettings.LoseBattle) taken += Strip(hero.BattleEquipment, captor);
        if (ModSettings.LoseCivilian) taken += Strip(hero.CivilianEquipment, captor);
        if (ModSettings.LoseStealth) taken += Strip(hero.StealthEquipment, captor);
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

    private static bool CaptorTakes(PartyBase captor, Hero hero)
    {
        if (captor == PartyBase.MainParty || captor.LeaderHero == Hero.MainHero) return ModSettings.PlayerCaptorTakes && MBRandom.RandomFloat < ModSettings.Chance;
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

    private static int Strip(Equipment eq, PartyBase captor)
    {
        if (eq == null || InRags(eq)) return 0;

        int minItemValue = ModSettings.MinItemValue;
        float slotChance = ModSettings.SlotChance;
        int taken = 0;
        for (int i = 0; i < (int)EquipmentIndex.NumEquipmentSetSlots; i++)
        {
            ItemObject item = eq[i].Item;
            if (item == null || item.Value < minItemValue || MBRandom.RandomFloat >= slotChance) continue;

            captor.ItemRoster?.AddToCounts(eq[i], 1);
            eq[i] = EquipmentElement.Invalid;
            taken++;
        }

        if (taken == 0 || eq[EquipmentIndex.Body].Item != null) return taken;
        ItemObject rags = MBObjectManager.Instance.GetObject<ItemObject>(RagsId);
        if (rags != null) eq[EquipmentIndex.Body] = new EquipmentElement(rags);
        else GlocLog.Warn($"Item '{RagsId}' was not found; the stripped hero was left without body armor.");
        return taken;
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
