using GearLossOnCapture.Core;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace GearLossOnCapture.Data;

// Used only when MCM is loaded. The only file that may use MCM types.
internal sealed class MCMSettings : AttributeGlobalSettings<MCMSettings>
{
    private const string SaveFormat = "json2";
    private const string PercentFormat = "#0%";
    private const string DenarsFormat = "0 denars";
    private const string WholeNumberFormat = "0";

    // Released Id (equals ModuleId): changing it resets every player's saved settings.
    public override string Id => ModInfo.ModuleId;

    public override string DisplayName => Texts.ModName.ToString();

    public override string FolderName => ModInfo.ModuleId;

    public override string FormatType => SaveFormat;

    [SettingPropertyFloatingInteger(Texts.Ids.Chance, ModConfiguration.ChanceMin, ModConfiguration.ChanceMax, PercentFormat, Order = 0, RequireRestart = false, HintText = Texts.Ids.ChanceHint)]
    [SettingPropertyGroup(Texts.Ids.GroupGeneral, GroupOrder = 0)]
    public float Chance { get; set; } = ModConfiguration.ChanceDefault;

    [SettingPropertyFloatingInteger(Texts.Ids.SlotChance, ModConfiguration.SlotChanceMin, ModConfiguration.SlotChanceMax, PercentFormat, Order = 1, RequireRestart = false, HintText = Texts.Ids.SlotChanceHint)]
    [SettingPropertyGroup(Texts.Ids.GroupGeneral, GroupOrder = 0)]
    public float SlotChance { get; set; } = ModConfiguration.SlotChanceDefault;

    [SettingPropertyInteger(Texts.Ids.MinItemValue, ModConfiguration.MinItemValueMin, ModConfiguration.MinItemValueMax, DenarsFormat, Order = 2, RequireRestart = false, HintText = Texts.Ids.MinItemValueHint)]
    [SettingPropertyGroup(Texts.Ids.GroupGeneral, GroupOrder = 0)]
    public int MinItemValue { get; set; } = ModConfiguration.MinItemValueDefault;

    [SettingPropertyBool(Texts.Ids.ReequipOnRelease, Order = 3, RequireRestart = false, HintText = Texts.Ids.ReequipOnReleaseHint)]
    [SettingPropertyGroup(Texts.Ids.GroupGeneral, GroupOrder = 0)]
    public bool ReequipOnRelease { get; set; } = ModConfiguration.ReequipOnReleaseDefault;

    [SettingPropertyBool(Texts.Ids.LoseBattle, Order = 0, RequireRestart = false, HintText = Texts.Ids.LoseBattleHint)]
    [SettingPropertyGroup(Texts.Ids.GroupSets, GroupOrder = 1)]
    public bool LoseBattle { get; set; } = ModConfiguration.LoseBattleDefault;

    [SettingPropertyBool(Texts.Ids.LoseCivilian, Order = 1, RequireRestart = false, HintText = Texts.Ids.LoseCivilianHint)]
    [SettingPropertyGroup(Texts.Ids.GroupSets, GroupOrder = 1)]
    public bool LoseCivilian { get; set; } = ModConfiguration.LoseCivilianDefault;

    [SettingPropertyBool(Texts.Ids.LoseStealth, Order = 2, RequireRestart = false, HintText = Texts.Ids.LoseStealthHint)]
    [SettingPropertyGroup(Texts.Ids.GroupSets, GroupOrder = 1)]
    public bool LoseStealth { get; set; } = ModConfiguration.LoseStealthDefault;

    [SettingPropertyBool(Texts.Ids.AI, Order = 0, RequireRestart = false, HintText = Texts.Ids.AIHint)]
    [SettingPropertyGroup(Texts.Ids.GroupVictims, GroupOrder = 2)]
    public bool AI { get; set; } = ModConfiguration.AIDefault;

    [SettingPropertyBool(Texts.Ids.Player, Order = 1, RequireRestart = false, HintText = Texts.Ids.PlayerHint)]
    [SettingPropertyGroup(Texts.Ids.GroupVictims, GroupOrder = 2)]
    public bool Player { get; set; } = ModConfiguration.PlayerDefault;

    [SettingPropertyBool(Texts.Ids.PlayerClan, Order = 2, RequireRestart = false, HintText = Texts.Ids.PlayerClanHint)]
    [SettingPropertyGroup(Texts.Ids.GroupVictims, GroupOrder = 2)]
    public bool PlayerClan { get; set; } = ModConfiguration.PlayerClanDefault;

    [SettingPropertyBool(Texts.Ids.PlayerCaravanMasters, Order = 3, RequireRestart = false, HintText = Texts.Ids.PlayerCaravanMastersHint)]
    [SettingPropertyGroup(Texts.Ids.GroupVictims, GroupOrder = 2)]
    public bool PlayerCaravanMasters { get; set; } = ModConfiguration.PlayerCaravanMastersDefault;

    [SettingPropertyBool(Texts.Ids.PlayerCaptorTakes, Order = 0, RequireRestart = false, HintText = Texts.Ids.PlayerCaptorTakesHint)]
    [SettingPropertyGroup(Texts.Ids.GroupCaptors, GroupOrder = 3)]
    public bool PlayerCaptorTakes { get; set; } = ModConfiguration.PlayerCaptorTakesDefault;

    [SettingPropertyBool(Texts.Ids.BanditsAlways, Order = 1, RequireRestart = false, HintText = Texts.Ids.BanditsAlwaysHint)]
    [SettingPropertyGroup(Texts.Ids.GroupCaptors, GroupOrder = 3)]
    public bool BanditsAlways { get; set; } = ModConfiguration.BanditsAlwaysDefault;

    [SettingPropertyBool(Texts.Ids.HonorableSpares, Order = 2, RequireRestart = false, HintText = Texts.Ids.HonorableSparesHint)]
    [SettingPropertyGroup(Texts.Ids.GroupCaptors, GroupOrder = 3)]
    public bool HonorableSpares { get; set; } = ModConfiguration.HonorableSparesDefault;

    [SettingPropertyBool(Texts.Ids.MercifulSpares, Order = 3, RequireRestart = false, HintText = Texts.Ids.MercifulSparesHint)]
    [SettingPropertyGroup(Texts.Ids.GroupCaptors, GroupOrder = 3)]
    public bool MercifulSpares { get; set; } = ModConfiguration.MercifulSparesDefault;

    [SettingPropertyBool(Texts.Ids.DishonorableAlways, Order = 4, RequireRestart = false, HintText = Texts.Ids.DishonorableAlwaysHint)]
    [SettingPropertyGroup(Texts.Ids.GroupCaptors, GroupOrder = 3)]
    public bool DishonorableAlways { get; set; } = ModConfiguration.DishonorableAlwaysDefault;

    [SettingPropertyBool(Texts.Ids.RelationSpares, Order = 5, RequireRestart = false, HintText = Texts.Ids.RelationSparesHint)]
    [SettingPropertyGroup(Texts.Ids.GroupCaptors, GroupOrder = 3)]
    public bool RelationSpares { get; set; } = ModConfiguration.RelationSparesDefault;

    [SettingPropertyInteger(Texts.Ids.SpareRelation, ModConfiguration.SpareRelationMin, ModConfiguration.SpareRelationMax, WholeNumberFormat, Order = 6, RequireRestart = false, HintText = Texts.Ids.SpareRelationHint)]
    [SettingPropertyGroup(Texts.Ids.GroupCaptors, GroupOrder = 3)]
    public int SpareRelation { get; set; } = ModConfiguration.SpareRelationDefault;

    [SettingPropertyBool(Texts.Ids.ExecutionTakesAll, Order = 7, RequireRestart = false, HintText = Texts.Ids.ExecutionTakesAllHint)]
    [SettingPropertyGroup(Texts.Ids.GroupCaptors, GroupOrder = 3)]
    public bool ExecutionTakesAll { get; set; } = ModConfiguration.ExecutionTakesAllDefault;

    // "Technical settings" is always the last group and "Logging for debugging" its last entry.
    [SettingPropertyBool(Texts.Ids.LoggingEnabled, Order = 0, RequireRestart = false, HintText = Texts.Ids.LoggingEnabledHint)]
    [SettingPropertyGroup(Texts.Ids.GroupTechnical, GroupOrder = 4)]
    public bool LoggingEnabled { get; set; } = ModConfiguration.LoggingEnabledDefault;
}
