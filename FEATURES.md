# GearLossOnCapture — Features

| Feature | Gate | File |
|---|---|---|
| Captured heroes always lose weapons, banner, horse and harness to the captor's inventory | always (needs Who Loses Gear + equipment set; player captor needs PlayerCaptorTakes) | `Behaviors\GearLossBehavior.cs` |
| Armor is taken in order: helmet, gloves, cape, body + boots (armor roll, then a roll per step, first failure stops; minimum item value) | setting:Chance (+ SlotChance, MinItemValue) | `Behaviors\GearLossBehavior.cs` |
| Heroes stripped down to the body step are dressed in tattered rags | always | `Behaviors\GearLossBehavior.cs` |
| Heroes released in rags get a fresh vanilla equipment set | setting:ReequipOnRelease | `Behaviors\GearLossBehavior.cs` |
| Which equipment sets can be taken (battle, civilian, stealth) | setting:LoseBattle / LoseCivilian / LoseStealth | `Behaviors\GearLossBehavior.cs` |
| Who can lose gear (AI, player, player clan, player caravan masters) | setting:AI / Player / PlayerClan / PlayerCaravanMasters | `Behaviors\GearLossBehavior.cs` |
| Captor rules (player captor, bandits, honor, mercy, relation threshold; spare rules protect armor only) | setting:PlayerCaptorTakes / BanditsAlways / HonorableSpares / MercifulSpares / DishonorableAlways / RelationSpares (+ SpareRelation) | `Behaviors\GearLossBehavior.cs` |
| Executed heroes' gear goes to the executioner's party | setting:ExecutionTakesAll | `Behaviors\GearLossBehavior.cs` |
| Settings: MCM when loaded, otherwise ModConfiguration.xml | always | `Core\ModSettings.cs`, `Data\MCMSettings.cs`, `Data\ModConfiguration.cs`, `_Module\ModConfiguration.xml` |
| Log file (problems always, debug lines only when enabled) | setting:LoggingEnabled | `Diagnostics\GlocLog.cs`, `Core\ModInfo.cs` |
| Localization (English + 12 languages) | — | `Core\Texts.cs`, `_Module\ModuleData\Languages\` |
