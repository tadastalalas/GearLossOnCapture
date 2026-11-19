GearLossOnCapture: workspace instructions. They override the global file.

- <Prefix>: GLOC (Gloc in type names). <Root> namespace and ModuleId: GearLossOnCapture.
- Logger: `Diagnostics\GlocLog.cs` (Debug / Warn / Error).
- No Harmony patches: no Patches\ folder, no PatchAll, no Harmony package.
- `Core\ModInfo.cs` holds the ModuleId, file and folder names and the module root path.
- The mod saves nothing (SyncData is empty).
- MCM Id and FolderName = "GearLossOnCapture" (released; never change).
- Kept exception: existing text ids are descriptive (`GLOC_<Name>`, hints `GLOC_<Name>_H`), not 7 random characters. Never rename them; new ids follow the global rule.
- CNs/CNt translations are by Cartoonist57: new rows use his "[ 预设：X ]" / "[ 預設：X ]" default style.
