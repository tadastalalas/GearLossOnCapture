using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using GearLossOnCapture.Data;
using GearLossOnCapture.Diagnostics;

namespace GearLossOnCapture.Core;

// The only place mod code reads settings from. MCM loaded -> MCM. MCM not loaded -> ModConfiguration.xml.
internal static class ModSettings
{
    private const string McmAssemblyName = "MCMv5";

    private static ModConfiguration _xml = new();
    private static XmlSerializer? _serializer;
    private static bool _useMcm;

    internal static float Chance => _useMcm ? McmChance() : _xml.General.Chance;

    internal static float SlotChance => _useMcm ? McmSlotChance() : _xml.General.SlotChance;

    internal static int MinItemValue => _useMcm ? McmMinItemValue() : _xml.General.MinItemValue;

    internal static bool ReequipOnRelease => _useMcm ? McmReequipOnRelease() : _xml.General.ReequipOnRelease;

    internal static bool LoseBattle => _useMcm ? McmLoseBattle() : _xml.EquipmentSets.LoseBattle;

    internal static bool LoseCivilian => _useMcm ? McmLoseCivilian() : _xml.EquipmentSets.LoseCivilian;

    internal static bool LoseStealth => _useMcm ? McmLoseStealth() : _xml.EquipmentSets.LoseStealth;

    internal static bool AI => _useMcm ? McmAI() : _xml.WhoLosesGear.AI;

    internal static bool Player => _useMcm ? McmPlayer() : _xml.WhoLosesGear.Player;

    internal static bool PlayerClan => _useMcm ? McmPlayerClan() : _xml.WhoLosesGear.PlayerClan;

    internal static bool PlayerCaravanMasters => _useMcm ? McmPlayerCaravanMasters() : _xml.WhoLosesGear.PlayerCaravanMasters;

    internal static bool PlayerCaptorTakes => _useMcm ? McmPlayerCaptorTakes() : _xml.CaptorRules.PlayerCaptorTakes;

    internal static bool BanditsAlways => _useMcm ? McmBanditsAlways() : _xml.CaptorRules.BanditsAlways;

    internal static bool HonorableSpares => _useMcm ? McmHonorableSpares() : _xml.CaptorRules.HonorableSpares;

    internal static bool MercifulSpares => _useMcm ? McmMercifulSpares() : _xml.CaptorRules.MercifulSpares;

    internal static bool DishonorableAlways => _useMcm ? McmDishonorableAlways() : _xml.CaptorRules.DishonorableAlways;

    internal static bool RelationSpares => _useMcm ? McmRelationSpares() : _xml.CaptorRules.RelationSpares;

    internal static int SpareRelation => _useMcm ? McmSpareRelation() : _xml.CaptorRules.SpareRelation;

    internal static bool ExecutionTakesAll => _useMcm ? McmExecutionTakesAll() : _xml.CaptorRules.ExecutionTakesAll;

    internal static bool LoggingEnabled => _useMcm ? McmLoggingEnabled() : _xml.TechnicalSettings.LoggingEnabled;

    private static XmlSerializer Serializer => _serializer ??= new XmlSerializer(typeof(ModConfiguration));

    // Called on every campaign start.
    internal static void Reload()
    {
        _useMcm = false;
        _xml = new ModConfiguration();

        if (IsMcmLoaded())
        {
            if (McmHasSettings())
            {
                _useMcm = true;
                return;
            }

            GlocLog.Warn("MCM is loaded but returned no settings for this mod. Using " + ModInfo.ConfigurationFileName + " instead.");
        }

        _xml = LoadFromFile();
    }

    private static bool IsMcmLoaded()
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        for (int i = 0; i < assemblies.Length; i++)
        {
            if (assemblies[i].GetName().Name == McmAssemblyName) return true;
        }

        return false;
    }

    // Every method that touches MCMSettings must be NoInlining and may only be called when MCM is loaded.
    // Otherwise the game tries to load MCM's classes and crashes when MCM is not installed.
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmHasSettings() => MCMSettings.Instance != null;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static float McmChance() => MCMSettings.Instance?.Chance ?? ModConfiguration.ChanceDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static float McmSlotChance() => MCMSettings.Instance?.SlotChance ?? ModConfiguration.SlotChanceDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmMinItemValue() => MCMSettings.Instance?.MinItemValue ?? ModConfiguration.MinItemValueDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmReequipOnRelease() => MCMSettings.Instance?.ReequipOnRelease ?? ModConfiguration.ReequipOnReleaseDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmLoseBattle() => MCMSettings.Instance?.LoseBattle ?? ModConfiguration.LoseBattleDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmLoseCivilian() => MCMSettings.Instance?.LoseCivilian ?? ModConfiguration.LoseCivilianDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmLoseStealth() => MCMSettings.Instance?.LoseStealth ?? ModConfiguration.LoseStealthDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmAI() => MCMSettings.Instance?.AI ?? ModConfiguration.AIDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmPlayer() => MCMSettings.Instance?.Player ?? ModConfiguration.PlayerDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmPlayerClan() => MCMSettings.Instance?.PlayerClan ?? ModConfiguration.PlayerClanDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmPlayerCaravanMasters() => MCMSettings.Instance?.PlayerCaravanMasters ?? ModConfiguration.PlayerCaravanMastersDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmPlayerCaptorTakes() => MCMSettings.Instance?.PlayerCaptorTakes ?? ModConfiguration.PlayerCaptorTakesDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmBanditsAlways() => MCMSettings.Instance?.BanditsAlways ?? ModConfiguration.BanditsAlwaysDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmHonorableSpares() => MCMSettings.Instance?.HonorableSpares ?? ModConfiguration.HonorableSparesDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmMercifulSpares() => MCMSettings.Instance?.MercifulSpares ?? ModConfiguration.MercifulSparesDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmDishonorableAlways() => MCMSettings.Instance?.DishonorableAlways ?? ModConfiguration.DishonorableAlwaysDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmRelationSpares() => MCMSettings.Instance?.RelationSpares ?? ModConfiguration.RelationSparesDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int McmSpareRelation() => MCMSettings.Instance?.SpareRelation ?? ModConfiguration.SpareRelationDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmExecutionTakesAll() => MCMSettings.Instance?.ExecutionTakesAll ?? ModConfiguration.ExecutionTakesAllDefault;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static bool McmLoggingEnabled() => MCMSettings.Instance?.LoggingEnabled ?? ModConfiguration.LoggingEnabledDefault;

    private static ModConfiguration LoadFromFile()
    {
        string path = Path.Combine(ModInfo.ModuleRoot, ModInfo.ConfigurationFileName);
        ModConfiguration configuration = new();

        if (!File.Exists(path))
        {
            GlocLog.Warn(ModInfo.ConfigurationFileName + " was missing. Created it with the default settings.");
            TryWrite(path, configuration);
            return configuration;
        }

        try
        {
            configuration = Read(path);
            if (!HasEverySetting(path, configuration))
            {
                GlocLog.Warn(ModInfo.ConfigurationFileName + " had missing settings. Added them with their default values.");
                TryWrite(path, configuration);
            }
        }
        catch (Exception ex) when (ex is InvalidOperationException or XmlException)
        {
            GlocLog.Warn(ModInfo.ConfigurationFileName + " was broken and was replaced with the default settings. Reason: " + (ex.InnerException?.Message ?? ex.Message));
            configuration = new ModConfiguration();
            TryWrite(path, configuration);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            GlocLog.Warn(ModInfo.ConfigurationFileName + " could not be opened. Using the default settings. Reason: " + ex.Message);
            configuration = new ModConfiguration();
        }

        // Out-of-range values are corrected in memory only; the file keeps what the player typed.
        configuration.Clamp();
        return configuration;
    }

    private static ModConfiguration Read(string path)
    {
        using FileStream stream = File.OpenRead(path);
        return Serializer.Deserialize(stream) as ModConfiguration ?? new ModConfiguration();
    }

    private static void TryWrite(string path, ModConfiguration configuration)
    {
        try
        {
            XmlWriterSettings writerSettings = new() { Indent = true, Encoding = Encoding.UTF8 };
            using XmlWriter writer = XmlWriter.Create(path, writerSettings);
            Serializer.Serialize(writer, configuration, NoNamespaces());
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            GlocLog.Warn(ModInfo.ConfigurationFileName + " could not be written. The settings are kept in memory for this session. Reason: " + ex.Message);
        }
    }

    private static bool HasEverySetting(string path, ModConfiguration configuration)
    {
        XDocument complete = new();
        using (XmlWriter writer = complete.CreateWriter())
        {
            Serializer.Serialize(writer, configuration, NoNamespaces());
        }

        XElement? fileRoot = XDocument.Load(path).Root;
        return fileRoot != null && complete.Root != null && ContainsEveryElement(fileRoot, complete.Root);
    }

    private static bool ContainsEveryElement(XElement actual, XElement expected)
    {
        foreach (XElement expectedChild in expected.Elements())
        {
            XElement? actualChild = actual.Element(expectedChild.Name);
            if (actualChild == null || !ContainsEveryElement(actualChild, expectedChild)) return false;
        }

        return true;
    }

    private static XmlSerializerNamespaces NoNamespaces()
    {
        XmlSerializerNamespaces namespaces = new();
        namespaces.Add(string.Empty, string.Empty);
        return namespaces;
    }
}
