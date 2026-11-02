# PROJECT THREADLINE

**WEAVE THE WORLDS. MASTER THE MOMENTUM.**

THREADLINE is an internal production codename for a premium 3D sci-fi action/parkour endless runner. The planned identity combines Reality Weaving across REALITY, GLITCH, and VOID with momentum-based Kinetic Flow. Commercial branding remains undecided.

## Current scope

Phase 01 establishes the Unity project foundation. The Windows foundation is implemented and build-verified. Android packaging and further Android setup are paused at the user's request; existing settings and diagnostic progress are preserved for later. Player movement, traversal, reality switching, procedural generation, menus, save systems, and online services are planned for later phases; none are implemented here.

## Environment

- Unity **6000.3.27f1**, Universal Render Pipeline, C#.
- Windows PC and Android development targets. iOS is a future additional target requiring an appropriate Apple build environment and testing.
- Input System; Cinemachine; uGUI including TextMeshPro; Animator, Timeline, and Animation Rigging; Shader Graph and Particle System.
- Git and Git LFS. ScriptableObject data and versioned JSON saves are architecture plans for Phase 02.

## Open the project

1. Install Git and Git LFS if missing. On a fresh checkout, run `git lfs install` and `git lfs pull`.
2. In Unity Hub, select **Projects > Add > Add project from disk** and choose the folder containing `Assets`, `Packages`, and `ProjectSettings`.
3. Open with **6000.3.27f1**. Allow package resolution, script compilation, and asset imports to finish.
4. Open `Assets/_Project/Scenes/Bootstrap/00_Bootstrap.unity`. Its empty background is intentional; initialization and scene transitions belong to Phase 02.
5. For rendering inspection, open `Assets/_Project/Scenes/Test/90_TestEnvironment.unity` and inspect **Window > General > Game** in edit mode. The configured Play start scene is Bootstrap; automated Play-mode rendering checks temporarily clear `EditorSceneManager.playModeStartScene` through the CLI and restore it afterward. Stop Play mode before editing assets.
6. Inspect **Window > General > Console** for errors. Review **File > Build Profiles** before building.

The Unity CLI connection uses the existing editor tooling package. Example from PowerShell:

```powershell
unity status --project-path 'C:\Users\Suryansh Patel\Unity Projects\THREADLINE' --format json
```

## Project organization

- `Assets/_Project/`: original game assets, settings, scenes, and code/tests.
- `Assets/_External/`: vendor assets with documented licenses.
- `Assets/_Temporary/`: ignored, disposable local content; keep it out of dependencies and builds.
- `Packages/`: committed package manifest and dependency lock.
- `ProjectSettings/`: shared Unity settings.
- `Documentation/`: standards, architecture, setup record, roadmap, and verification evidence.
- `Builds/` and `Artifacts/`: generated outputs, excluded from Git.

Start with [technical architecture](Documentation/TECHNICAL_ARCHITECTURE.md), [setup and settings](Documentation/PHASE01_SETUP.md), [coding standards](Documentation/CODING_STANDARDS.md), [asset guidelines](Documentation/ASSET_GUIDELINES.md), and [development log](Documentation/DEVELOPMENT_LOG.md). The final [verification report](Documentation/PHASE01_VERIFICATION.md) distinguishes completed checks from hardware and release work still pending.

The company name and `com.example.threadline` identifiers are explicit development placeholders. Do not publish with them. Source control uses the [user-created GitHub repository](https://github.com/Suryansh1483/THREADLLINE); generated builds and local diagnostics remain excluded.
