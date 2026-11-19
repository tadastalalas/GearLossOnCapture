using System;

namespace GearLossOnCapture.Data;

// Shape of ModConfiguration.xml and the only place where defaults and ranges live (MCMSettings uses them too).
public class ModConfiguration
{
    public const float ChanceDefault = 0.8f;
    public const float ChanceMin = 0f;
    public const float ChanceMax = 1f;
    public const float SlotChanceDefault = 0.8f;
    public const float SlotChanceMin = 0f;
    public const float SlotChanceMax = 1f;
    public const int MinItemValueDefault = 200;
    public const int MinItemValueMin = 0;
    public const int MinItemValueMax = 100000;
    public const bool ReequipOnReleaseDefault = true;

    public const bool LoseBattleDefault = true;
    public const bool LoseCivilianDefault = false;
    public const bool LoseStealthDefault = false;

    public const bool AIDefault = true;
    public const bool PlayerDefault = true;
    public const bool PlayerClanDefault = true;
    public const bool PlayerCaravanMastersDefault = true;

    public const bool PlayerCaptorTakesDefault = true;
    public const bool BanditsAlwaysDefault = true;
    public const bool HonorableSparesDefault = true;
    public const bool MercifulSparesDefault = true;
    public const bool DishonorableAlwaysDefault = true;
    public const bool RelationSparesDefault = true;
    public const int SpareRelationDefault = 10;
    public const int SpareRelationMin = -100;
    public const int SpareRelationMax = 100;
    public const bool ExecutionTakesAllDefault = true;

    public const bool LoggingEnabledDefault = false;

    public GeneralSection General { get; set; } = new();

    public EquipmentSetsSection EquipmentSets { get; set; } = new();

    public WhoLosesGearSection WhoLosesGear { get; set; } = new();

    public CaptorRulesSection CaptorRules { get; set; } = new();

    // Always the last section.
    public TechnicalSettingsSection TechnicalSettings { get; set; } = new();

    internal void Clamp()
    {
        General.Chance = ClampChance(General.Chance, ChanceDefault, ChanceMin, ChanceMax);
        General.SlotChance = ClampChance(General.SlotChance, SlotChanceDefault, SlotChanceMin, SlotChanceMax);
        General.MinItemValue = Math.Min(Math.Max(General.MinItemValue, MinItemValueMin), MinItemValueMax);
        CaptorRules.SpareRelation = Math.Min(Math.Max(CaptorRules.SpareRelation, SpareRelationMin), SpareRelationMax);
    }

    private static float ClampChance(float value, float fallback, float min, float max) => float.IsNaN(value) ? fallback : Math.Min(Math.Max(value, min), max);

    public class GeneralSection
    {
        public float Chance { get; set; } = ChanceDefault;
        public float SlotChance { get; set; } = SlotChanceDefault;
        public int MinItemValue { get; set; } = MinItemValueDefault;
        public bool ReequipOnRelease { get; set; } = ReequipOnReleaseDefault;
    }

    public class EquipmentSetsSection
    {
        public bool LoseBattle { get; set; } = LoseBattleDefault;
        public bool LoseCivilian { get; set; } = LoseCivilianDefault;
        public bool LoseStealth { get; set; } = LoseStealthDefault;
    }

    public class WhoLosesGearSection
    {
        public bool AI { get; set; } = AIDefault;
        public bool Player { get; set; } = PlayerDefault;
        public bool PlayerClan { get; set; } = PlayerClanDefault;
        public bool PlayerCaravanMasters { get; set; } = PlayerCaravanMastersDefault;
    }

    public class CaptorRulesSection
    {
        public bool PlayerCaptorTakes { get; set; } = PlayerCaptorTakesDefault;
        public bool BanditsAlways { get; set; } = BanditsAlwaysDefault;
        public bool HonorableSpares { get; set; } = HonorableSparesDefault;
        public bool MercifulSpares { get; set; } = MercifulSparesDefault;
        public bool DishonorableAlways { get; set; } = DishonorableAlwaysDefault;
        public bool RelationSpares { get; set; } = RelationSparesDefault;
        public int SpareRelation { get; set; } = SpareRelationDefault;
        public bool ExecutionTakesAll { get; set; } = ExecutionTakesAllDefault;
    }

    public class TechnicalSettingsSection
    {
        // Always the last entry.
        public bool LoggingEnabled { get; set; } = LoggingEnabledDefault;
    }
}
