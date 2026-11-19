using TaleWorlds.Localization;

namespace GearLossOnCapture.Core;

internal static class Texts
{
    #region Settings
    public static readonly TextObject ModName = new(Ids.ModName);
    #endregion

    internal static class Ids
    {
        #region Settings
        public const string ModName = "{=GLOC_DisplayName}Gear Loss On Capture";

        public const string GroupGeneral = "{=GLOC_General}General";
        public const string Chance = "{=GLOC_Chance}Gear Loss Chance";
        public const string ChanceHint = "{=GLOC_Chance_H}Chance that a captured hero loses any gear at all. Rolled once per capture. Default: 80%";
        public const string SlotChance = "{=GLOC_SlotChance}Per-Item Loss Chance";
        public const string SlotChanceHint = "{=GLOC_SlotChance_H}After the main roll passes, each equipped item is rolled separately. 100% strips everything. Default: 80%";
        public const string MinItemValue = "{=GLOC_MinItemValue}Minimum Item Value";
        public const string MinItemValueHint = "{=GLOC_MinItemValue_H}Items worth less than this are never taken. 0 disables the filter. Default: 200";
        public const string ReequipOnRelease = "{=GLOC_ReequipOnRelease}Re-equip On Release";
        public const string ReequipOnReleaseHint = "{=GLOC_ReequipOnRelease_H}Heroes freed while wearing rags receive a fresh vanilla-generated equipment set. Default: true";

        public const string GroupSets = "{=GLOC_Sets}Equipment Sets";
        public const string LoseBattle = "{=GLOC_LoseBattle}Battle Gear";
        public const string LoseBattleHint = "{=GLOC_LoseBattle_H}Battle equipment set can be taken. Default: true";
        public const string LoseCivilian = "{=GLOC_LoseCivilian}Civilian Gear";
        public const string LoseCivilianHint = "{=GLOC_LoseCivilian_H}Civilian equipment set can be taken. Default: false";
        public const string LoseStealth = "{=GLOC_LoseStealth}Stealth Gear";
        public const string LoseStealthHint = "{=GLOC_LoseStealth_H}Stealth equipment set can be taken. Default: false";

        public const string GroupVictims = "{=GLOC_Victims}Who Loses Gear";
        public const string AI = "{=GLOC_AI}AI Heroes";
        public const string AIHint = "{=GLOC_AI_H}Heroes outside the player clan can lose gear. Default: true";
        public const string Player = "{=GLOC_Player}Player";
        public const string PlayerHint = "{=GLOC_Player_H}The player character can lose gear. Default: true";
        public const string PlayerClan = "{=GLOC_PlayerClan}Player Clan Members";
        public const string PlayerClanHint = "{=GLOC_PlayerClan_H}Family and companions in the player clan can lose gear. Does not include caravan masters. Default: true";
        public const string PlayerCaravanMasters = "{=GLOC_PlayerCaravanMasters}Player Caravan Masters";
        public const string PlayerCaravanMastersHint = "{=GLOC_PlayerCaravanMasters_H}Companions leading player caravans can lose gear. Default: true";

        public const string GroupCaptors = "{=GLOC_Captors}Captor Rules";
        public const string PlayerCaptorTakes = "{=GLOC_PlayerCaptorTakes}Player Takes Gear As Captor";
        public const string PlayerCaptorTakesHint = "{=GLOC_PlayerCaptorTakes_H}When the player captures a hero, their gear is taken and added to the player inventory. Default: true";
        public const string BanditsAlways = "{=GLOC_BanditsAlways}Bandits Always Take Gear";
        public const string BanditsAlwaysHint = "{=GLOC_BanditsAlways_H}Bandit captors ignore the chance roll and always take gear. Default: true";
        public const string HonorableSpares = "{=GLOC_HonorableSpares}Honorable Captors Spare Gear";
        public const string HonorableSparesHint = "{=GLOC_HonorableSpares_H}Captor party leader with positive Honor trait never takes gear. Default: true";
        public const string MercifulSpares = "{=GLOC_MercifulSpares}Merciful Captors Spare Gear";
        public const string MercifulSparesHint = "{=GLOC_MercifulSpares_H}Captor party leader with positive Mercy trait never takes gear. Default: true";
        public const string DishonorableAlways = "{=GLOC_DishonorableAlways}Dishonorable Captors Always Take Gear";
        public const string DishonorableAlwaysHint = "{=GLOC_DishonorableAlways_H}Captor party leader with negative Honor trait ignores the chance roll and always takes gear. Default: true";
        public const string RelationSpares = "{=GLOC_RelationSpares}Friendly Captors Spare Gear";
        public const string RelationSparesHint = "{=GLOC_RelationSpares_H}Captor party leader with relation at or above the threshold never takes gear. Default: true";
        public const string SpareRelation = "{=GLOC_SpareRelation}Friendly Relation Threshold";
        public const string SpareRelationHint = "{=GLOC_SpareRelation_H}Relation value at which a captor is considered friendly. Default: 10";
        public const string ExecutionTakesAll = "{=GLOC_ExecutionTakesAll}Take All Equipment After Execution";
        public const string ExecutionTakesAllHint = "{=GLOC_ExecutionTakesAll_H}When a hero is executed, every item from the battle, civilian and stealth sets goes to the executioner's party inventory. Applies to player and AI executions. Default: true";

        public const string GroupTechnical = "{=GLOC_Tq4mZ8r}Technical settings";
        public const string LoggingEnabled = "{=GLOC_b7XkN2w}Logging for debugging";
        public const string LoggingEnabledHint = "{=GLOC_R5hVc9e}Logging for debugging (English only). Default: false";
        #endregion
    }
}
