## GLOC v1.5.7.0

- Migrated from the legacy build (net472;net6 + game HintPaths, version 1.1.0) to `Bannerlord.BuildResourcesLite.Sdk` + `Bannerlord.ModuleLoader.Injector`: one DLL per game version (`GearLossOnCapture.v<ver>.dll`), netstandard2.0. Versions from `..\supported-game-versions.txt`: 1.5.7, 1.4.8, 1.4.7.
  - `ModuleName` now `Gear Loss On Capture`; `Version` 1.5.7.0; `Bannerlord.MCM` stays 5.12.3.
  - Dropped `Lib.Harmony`, `Harmony.Extensions`, `BUTR.Harmony.Analyzer`, `Nullable`, `IsExternalInit`, `Bannerlord.BuildResources`: the mod has no Harmony patches.
  - `_Module\SubModule.xml`: Module Loader entry; MCM is no longer a `DependedModule`, only an optional `LoadBeforeThis` metadata (`v$mcmversion$`).
- No game-version differences: no `#if`. Checked against 1.5.7 source: `CampaignEvents.HeroPrisonerTaken`, `HeroPrisonerReleased`, `BeforeHeroKilledEvent`, `Hero.StealthEquipment`, `KillCharacterActionDetail.ExecutionAfterMapEvent`, `EquipmentSelectionModel.GetEquipmentForHeroComeOfAge`. All 3 versions build.
- Settings split: `Settings.cs` (MCM-only, root namespace) replaced by `Data\ModConfiguration.cs` (defaults + ranges), `Data\MCMSettings.cs`, `Core\ModSettings.cs` and `_Module\ModConfiguration.xml`.
  - MCM loaded → MCM values read live; MCM not loaded → `ModConfiguration.xml` (sections `General`, `EquipmentSets`, `WhoLosesGear`, `CaptorRules`, `TechnicalSettings`), reloaded in `SubModule.OnGameStart`.
  - MCM Id and FolderName stay `GearLossOnCapture`; property names unchanged, so players keep their MCM values.
  - Before: with MCM missing or `Settings.Instance == null` the mod did nothing. Now it runs on the xml values.
- New setting `LoggingEnabled` ("Logging for debugging", group "Technical settings", default false).
- New logger `Diagnostics\GlocLog.cs` → `Modules\GearLossOnCapture\logs\GearLossOnCapture_<date>.log`. Debug lines in `GearLossBehavior` (capture result, items taken, execution, re-equip); Warn when `tattered_rags` is missing.
- Texts: every MCM string moved to `Core\Texts.cs` (`Texts.Ids` consts). Existing ids `GLOC_<Name>` kept. New ids in all 13 files: `GLOC_Tq4mZ8r` (Technical settings), `GLOC_b7XkN2w` (Logging for debugging), `GLOC_R5hVc9e` (hint).
- CNs/CNt translations merged/added by Cartoonist57 (his "[ 预设：X ]" / "[ 預設：X ]" style used for the new rows).
- `Behaviors\GearLossBehavior.cs`: reads `ModSettings` instead of a passed `Settings`; `Strip`/`TakeAll` return item counts for logging. Same checks, same random-roll order. Feature header added.
- Save compatibility: none (SyncData empty, behavior class name unchanged).
- Test: with MCM and without MCM (xml created/rewritten/clamped), capture by player/bandit/honorable lord, release in rags, execution; log file with logging on/off.
