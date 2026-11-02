# THREADLINE project structure

## Ownership and locations

- `Assets/_Project/`: original THREADLINE assets and project-specific configuration.
- `Assets/_External/<Vendor>/<AssetPack>/`: imported third-party assets. Preserve vendor structure when scripts or import tools depend on it. Record source, version, license, and modifications alongside each import.
- `Assets/_Temporary/`: disposable local experiments only. Nothing in a build scene or committed asset may depend on this folder.
- `Packages/`: Unity Package Manager dependencies; do not copy package contents into `_External` or edit `Library/PackageCache`.
- `Documentation/`: repository-level standards and progress records.
- `Assets/_Project/Documentation/`: Unity-imported technical, art, and design references. Do not duplicate repository documents here.

Existing template assets remain in their original locations. Move them later using Unity's Project window or `AssetDatabase.MoveAsset`, preserving their `.meta` files and GUIDs. No assets were moved or deleted in Task 01.02.

## Original asset folders

All paths below are relative to `Assets/_Project/`. Empty folders reserve the explicitly requested organization; they do not represent implemented systems.

```text
Art/
  Characters/
  Environments/{Shared,Reality,Glitch,Void}/
  Materials/
  Textures/
  Models/
  Props/
  Lighting/
  Decals/
Audio/{Music,SFX,Ambience,Voice,Mixers}/
Animation/{Characters,Environment,Gameplay,Controllers,Masks,Rigging}/
Cinematics/{Cameras,Timeline,Sequences}/
Code/{Runtime,Editor,Tests}/
Data/{ScriptableObjects,Configurations,Localization}/
Prefabs/{Characters,Environment,Gameplay,UI,VFX,Cameras}/
Scenes/{Bootstrap,MainMenu,Gameplay,Test}/
Settings/{Rendering,Input,Quality,Physics,Build}/
Shaders/ShaderGraph/
UI/{Fonts,Icons,Sprites,Themes,Components,Screens}/
VFX/{Particles,Graphs,Textures}/
Documentation/{Technical,Art,Design}/
```

Braces above are compact notation for separate folders, not literal folder names.

Use `Art/Models` for shared raw meshes, `Art/Props` for prop source assets, and `Prefabs/Environment` for assembled environment objects. Keep character and environment-specific source assets in their respective art folders. Shared material assets belong only in `Art/Materials`; no duplicate `Shaders/Materials` hierarchy is needed. VFX-specific textures belong in `VFX/Textures`, while general textures belong in `Art/Textures`.

Keep PascalCase directory names, with `_Project`, `_External`, and `_Temporary` as the three deliberate root exceptions. Use the existing spelling consistently. Future naming standards will define asset filenames.

## Unity special folders

- `Editor`: scripts compile for the Editor and are excluded from players. Future assembly definitions must preserve this boundary explicitly.
- `Resources`: assets can be loaded by resource path and are included in player builds even without scene references. None was created; add one only for a documented need.
- `StreamingAssets`: files are copied into builds without normal asset processing. Platform-specific access rules apply, particularly on Android. None is needed yet.
- `Plugins`: Unity has special native-plugin import and compilation conventions. Use only for a real plugin requirement and check platform import settings. None is needed yet.
- `Tests` is organizational, not an automatic exclusion from player builds. Add test assembly definitions when tests are introduced.

## Version control policy

Commit original assets, required external assets with redistribution rights, every corresponding `.meta` file, `Packages/manifest.json`, `Packages/packages-lock.json`, `ProjectSettings/`, and repository documentation. Meta GUIDs preserve references; losing or regenerating a meta file can break scenes, materials, and prefabs. Unity creates folder metadata too; preserve it.

Do not commit `Library/`, `Temp/`, `Obj/`, `Logs/`, `UserSettings/`, build outputs, IDE-generated project files, or local experimental content. Put downloads, generated reports, and scratch files outside `Assets` where possible.

Git configuration is pending the dedicated version-control task. That task must ignore `/Assets/_Temporary/` and `/Assets/_Temporary.meta`, configure generated-file exclusions, and verify them before staging files. The folder currently has no content and is not automatically excluded from builds; do not use it for persistent assets. No ignore rules or Git LFS setup are claimed as implemented yet.

Use Unity's Project window to move or rename imported assets so their metadata follows. For a new folder, select its parent and use **Assets > Create > Folder**. Never drag assets between folders using Windows Explorer while Unity is open.
