# Technical architecture

Phase 02 status: Task 02.01 inspection performed; user confirmation pending. See [the audit](PHASE02_AUDIT.md). The runtime architecture below remains planned until Task 02.02 is proposed and approved. Android work remains deferred by explicit user instruction.

## Implemented foundation

Unity 6000.3.27f1 runs the URP project locally. Original assets live under `_Project`, external imports under `_External`, and shared setup documents at the repository root. Runtime, Editor, and test code have separate directories. No runtime architecture or gameplay managers are needed in Phase 01.

### Rendering

Five quality levels reference five explicit URP assets in `Assets/_Project/Settings/Rendering`. PC tiers share the PC forward renderer; Android tiers share the mobile forward renderer. SRP Batcher is enabled. Depth/opaque copies, renderer features, and additional-light shadows are disabled initially. Post-processing remains supported by URP but disabled on foundation cameras. Standard URP Lit materials and a small directional-lit test scene establish the rendering check.

PC High is the desktop default. Android Medium is the mobile default. These are starting budgets, not measured device performance. See `PHASE01_SETUP.md` for exact settings and tradeoffs.

### Input

`Assets/_Project/Settings/Input/ThreadlineInput.inputactions` contains Gameplay button actions and preserves the template UI map. Keyboard and gamepad bindings share semantic actions; future touch controls should invoke those same actions using on-screen controls or a gesture adapter. No polling wrapper, movement consumer, touch UI, or gesture recognizer is introduced yet.

### Scenes

| Scene | Purpose | Production build |
| --- | --- | --- |
| `00_Bootstrap` | Future startup initialization; camera only now | First |
| `01_MainMenu` | Future menus; camera only now | Second |
| `02_Gameplay` | Future gameplay; camera only now | Third |
| `90_TestEnvironment` | URP material, light, shadow, and camera checks | Excluded |

Scenes remain deliberately minimal. Bootstrap does not load another scene until Phase 02 defines the lifecycle. Windows and Android development Build Profiles use the production scene list; smoke verification may supply the test scene explicitly without changing that list.

### Code and tests

No empty runtime assembly or placeholder game class is required. Add runtime `.asmdef` boundaries with the first real runtime module. The `Threadline.Editor` assembly contains the narrowly scoped Windows Android-build socket-directory utility. The foundation test assembly is Editor-only and references this utility and the packages needed to validate assets and settings. Neither assembly enters player builds.

On this Windows host, JDK Unix-domain socket connections fail with the default TEMP path. The host already configures `-Djdk.net.unixdomain.tmpdir` through `JAVA_TOOL_OPTIONS`, but Unity removes that variable when launching Gradle. `AndroidGradleSocketDirectory` honors that existing, valid directory by temporarily assigning the Editor process's TEMP/TMP during Android builds. It restores the original values after completion, failure, or assembly reload. No global environment, protected JDK file, SDK installation, or runtime game code is modified. Machines without that explicit JDK option receive no override.

### Version control

`main` starts with the project baseline. Source assets, dependency locks, settings, and metadata are committed. Binary art/audio/model/font files use LFS; Unity text assets and `.meta` files remain ordinary Git text. Generated state, build outputs, signing files, and `_Temporary` are ignored. LFS is configured locally; remote synchronization requires an explicitly chosen repository with LFS support.

The Pipeline package provides local Editor control. Its HTTP/code-evaluation tooling must remain excluded from player builds; verify development and release inclusion settings before shipping.

## Planned architecture, not implemented

- Phase 02: lifecycle, scene loading, narrow events/interfaces, configuration, versioned JSON saves, and useful logging.
- Phase 03–05: movement, traversal, animation/camera behavior, Reality/Glitch/Void switching, and Flow.
- Phase 06–10: modular art, track streaming/pooling, deterministic procedural generation, hazards, scoring, progression, and production character assets.
- Phase 11: menus, HUD, accessibility, audio mixer routing, and music/SFX production.
- Phase 12: accounts, asynchronous cloud saves, leaderboards, ghost races, secure APIs, and run validation. No real-time multiplayer in version 1.0.
- Phase 13–15: content completion, profiling/device QA/compliance, signing, distribution, monitoring, and updates.

Core gameplay must remain offline-first. Future online requests may not block movement or traversal. Save migrations and server trust boundaries require explicit versioning and validation when implemented.
