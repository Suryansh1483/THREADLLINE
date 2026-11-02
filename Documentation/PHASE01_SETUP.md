# Phase 01 setup and decisions

The user confirmed Tasks 01.01 and 01.02, then authorized completing the remaining Phase 01 tasks without intermediate confirmation. Each task below records the beginner-facing inspection path. The execution log and final verification report record evidence; this guide does not replace actual build results.

## Task 01.01 — Existing project and CLI

**Objective / why:** reuse the correct local project and establish live Editor control. **Prerequisites:** the installed Unity environment; do not reinstall it.

**Implementation:** open `C:\Users\Suryansh Patel\Unity Projects\THREADLINE` with Unity 6000.3.27f1. The CLI connects through the project's existing Pipeline package. Check version under **Help > About Unity**, and errors under **Window > General > Console**. The CLI version used for setup is `1.0.0-beta.15`.

**Expected result / verification:** Editor reports the matching version, project is ready, and compilation finishes. **Common problems:** imports and domain reloads temporarily interrupt the CLI; wait for readiness before changing assets. Never replay an interrupted asset-creation command until checking whether it already wrote assets. **Status:** user confirmed.

## Task 01.02 — Folder organization

**Objective / why:** establish ownership and consistent asset locations. **Prerequisites:** the connected project.

**Implementation:** in **Window > General > Project**, expand **Assets > _Project**. Unity's AssetDatabase created 76 folders with metadata. `_Project` holds original work; `_External` holds licensed vendor imports; ignored `_Temporary` holds disposable experiments. `PROJECT_STRUCTURE.md` lists the complete tree and special folder behavior. To create a future folder, select its parent and choose **Assets > Create > Folder**.

**Expected result / verification:** each asset/folder has preserved `.meta` metadata. **Common problems:** Windows Explorer moves can separate assets from metadata; use Unity's Project window instead. **Status:** user confirmed; later setup moves template configuration into `_Project` while preserving GUIDs.

## Task 01.03 — Naming conventions

**Objective / why:** avoid conflicting names and file placement before gameplay code exists. **Prerequisites:** folder organization.

**Implementation:** follow `CODING_STANDARDS.md`: PascalCase types/methods/properties, camelCase locals/parameters/private serialized fields, `I` for interfaces, descriptive asset names, ordered scenes. Example: `RealityConfiguration.cs`, `RealityRoadSurface.mat`, `Gameplay/RealitySwitch`.

**Expected result / verification:** names and examples are documented; existing vendor names are preserved. **Common problems:** a C# MonoBehaviour filename must match its class; do not rename only one. **Status:** documented.

## Task 01.04 — Project settings

**Objective / why:** create conservative PC/mobile defaults without premature physics or gameplay tuning. **Prerequisites:** URP and Input System already installed.

**Implementation / inspection paths:**

1. **Edit > Project Settings > Player**: Company Name `Threadline Placeholder Studio`, Product Name `THREADLINE`, Version `0.1.0`. These are internal development values.
2. Select the Windows/Android platform tab under Player. Application identifier: `com.example.threadline`; Android Bundle Version Code `1`. iOS uses the same placeholder identifier but is not build-verified on Windows.
3. **Player > Other Settings > Configuration**: Windows development uses Mono; Android uses IL2CPP, ARM64. API compatibility: .NET Standard 2.1 (`NET_Standard` in Unity's API). Stripping: Disabled for Windows Mono, Minimal for Android. Windows IL2CPP release builds require the native Windows toolchain and separate validation later.
4. Android **Player > Resolution and Presentation**: Landscape Left. **Other Settings**: minimum API 26; target API 36, which is installed. Pin this explicit development baseline instead of inheriting whichever higher SDK is installed. The machine also has SDK 37.0; its metadata reports PreviewSdkInt 0. This is not a store submission policy. Review platform and store requirements at release time.
5. **Player > Other Settings > Rendering**: Linear color space. Android graphics API: OpenGLES3 only, with Auto Graphics API disabled. Start with this broad URP-compatible baseline; Vulkan requires a separately tested device matrix before enabling. Windows retains its supported template graphics API setup.
6. **Edit > Project Settings > Graphics**: default pipeline `Assets/_Project/Settings/Rendering/PCHigh_RPAsset.asset`. Quality overrides select platform-specific pipelines.
7. **Edit > Project Settings > Quality**: five tiers in the table below. Desktop default PC High; Android default Android Medium. Desktop excludes Android tiers; Android excludes PC tiers.
8. **Edit > Project Settings > Time**: retain Fixed Timestep `0.02` seconds (50 Hz) and Maximum Allowed Timestep `0.33333334` seconds. These standard defaults avoid premature movement tuning. The maximum bounds physics catch-up after stalls; it does not guarantee frame rate. Revisit after movement profiling.
9. **Edit > Project Settings > Physics**: solver iterations `6`, velocity iterations `1`; retain normal gravity and the default collision matrix. Do not invent a layer matrix before colliders and traversal contracts exist. Phase 02/03 must define Player/World/Hazard/Trigger interactions before altering collisions.
10. **Edit > Project Settings > Editor**: Force Text asset serialization; version-control mode Visible Meta Files. This keeps scenes/settings reviewable and GUID references stable.
11. **Edit > Project Settings > Player > Other Settings > Active Input Handling**: Input System Package (New). The original template already enabled it; no legacy/Both mode is required.
12. **Edit > Project Settings > Audio**: retain system sample rate, stereo, Best latency/default DSP selection, 32 real and 512 virtual voices. No audio content exists to justify tuning these budgets. Actual mixer routing follows when audio production begins.

| Tier | URP asset | Render scale | MSAA | Shadow distance | Cascades | Main shadow map | HDR |
| --- | --- | --- | --- | --- | --- | --- | --- |
| PC High | `PCHigh_RPAsset` | 1.0 | 4x | 60 m | 4 | 2048 | On |
| PC Medium | `PCMedium_RPAsset` | 1.0 | 2x | 40 m | 2 | 1024 | On |
| Android High | `AndroidHigh_RPAsset` | 1.0 | 2x | 35 m | 2 | 1024 | Off |
| Android Medium | `AndroidMedium_RPAsset` | 0.85 | 2x | 25 m | 1 | 1024 | Off |
| Android Low | `AndroidLow_RPAsset` | 0.7 | Off | 15 m | 1 | 512 | Off |

All five assets live in `Assets/_Project/Settings/Rendering`. URP asset MSAA governs rendering; the legacy Quality anti-aliasing field is 0. All tiers enable SRP Batcher and disable extra camera depth/opaque copies and additional-light shadows. Soft shadows are enabled for PC and Android High. Additional lights per object: 4 on PC, 2 on Android. Renderer features such as SSAO start disabled. Android Low limits texture mipmaps by one level. These values need device profiling once real content exists.

**Frame-rate strategy:** PC VSync Count 1 follows display refresh; it is not a universal 60 FPS cap. Android VSync Count 0 and optimized frame pacing are enabled. Android's explicit 60/30 target selection will be added with runtime platform configuration in Phase 02; without a runtime request, Unity's mobile default can be 30 FPS. `Update` follows rendered frames; `FixedUpdate` follows fixed simulation steps; neither is a frame limiter. Targets remain PC 60+, selected Android 60, lower-tier Android fallback 30, subject to profiling.

**Expected result / verification:** all quality tiers reference valid URP assets and platform defaults differ appropriately. **Common problems:** a quality override can mask Graphics' default pipeline; inspect both pages. Pink materials indicate a shader/pipeline problem and must not be hidden. **Status:** configured; final report records verification.

## Task 01.05 — Packages

**Objective / why:** retain a small, compatible foundation. **Prerequisites:** stable package resolution and internet access for new packages.

**Implementation:** inspect **Window > Package Management > Package Manager > In Project**. Install through Unity's Package Manager API, not manual manifest editing. The direct additions are `com.unity.cinemachine@3.1.7` and `com.unity.animation.rigging@1.4.1`. Unity's 6000.3 package documentation lists both as released versions: [Cinemachine](https://docs.unity.com/en-us/engine/6000.3/manual/packages-list/packages-all/pack-safe/com-unity-cinemachine), [Animation Rigging](https://docs.unity.com/en-us/engine/6000.3/manual/packages-list/packages-all/pack-safe/com-unity-animation-rigging).

| Package | Role / need | Build implications |
| --- | --- | --- |
| URP 17.3.0 | Essential renderer now; retained | Platform shader variants and render budgets need profiling |
| Input System 1.20.1 | Essential semantic input foundation now; retained | Runtime input dependency |
| Cinemachine 3.1.7 | Available for the planned runner camera | No camera rig added yet |
| uGUI 2.0.0 / TextMeshPro | Required UI stack; retained | TMP is integrated in uGUI; do not add an obsolete standalone TMP package |
| Test Framework 1.6.0 | Editor verification; retained | Test assemblies must remain Editor-only |
| Animation Rigging 1.4.1 | Explicitly justified by upcoming IK/traversal work | Additional Burst/Collections dependencies; no rig components added yet |
| Shader Graph 17.3.0 | URP dependency, same version family | No separate duplicate installation |
| Timeline 1.8.13 | Retained animation/cinematic tooling | No timelines or sequences authored yet |
| Pipeline 0.9.1-exp.1 | Existing local Editor automation | Experimental tooling package; keep out of player builds |
| Rider / Visual Studio integrations | Retained developer IDE support | Editor tooling only |
| VFX Graph | Deferred; Particle System suffices now | Avoid mobile/platform cost without a concrete effect |
| Recorder | Deferred until captures need it | Avoid unused editor dependency |

Removed unused template AI Navigation, Multiplayer Center, Visual Scripting, and Unity Version Control integration. No Phase 01 content depends on them; Git is the requested VCS. Built-in engine modules remain intact. The final package inventory records direct and transitive resolved versions.

**Expected result / verification:** no missing package references, matching URP/Shader Graph versions, successful compilation. **Common problems:** registry calls may time out while imports block Editor updates. Read package results and lock state before retrying; do not update to experimental releases to bypass a problem. **Status:** installed; final report records checks.

## Task 01.06 — Scenes and Build Profiles

**Objective / why:** establish stable scene paths and safe build inclusion. **Prerequisites:** rendering settings and input dependencies.

**Implementation:** open scenes under `Assets/_Project/Scenes`: `Bootstrap/00_Bootstrap`, `MainMenu/01_MainMenu`, `Gameplay/02_Gameplay`, and `Test/90_TestEnvironment`. Production scenes contain only Main Camera and AudioListener. The test scene adds Directional Light, ground, and three colored primitives. Build scene order is Bootstrap, MainMenu, Gameplay. TestEnvironment stays out of production profiles. Inspect **File > Build Profiles** for WindowsDevelopment and AndroidDevelopment under `Assets/_Project/Settings/Build`.

**Expected result / verification:** all four scenes load; three production scenes are enabled in order. **Common problems:** Bootstrap renders an empty background intentionally; there is no automatic menu transition yet. **Status:** created; final report records load checks.

## Task 01.07 — Input Actions

**Objective / why:** separate input intent from future controllers. **Prerequisites:** Input System enabled.

**Implementation:** double-click `Assets/_Project/Settings/Input/ThreadlineInput.inputactions` in the Project window. An action map groups actions; bindings connect devices to those actions. Gameplay contains:

| Action | Keyboard | Gamepad |
| --- | --- | --- |
| MoveLeft | A / Left Arrow | D-pad Left / Left Stick Left |
| MoveRight | D / Right Arrow | D-pad Right / Left Stick Right |
| Jump | Space | South button |
| Slide | S / Down Arrow | East button |
| RealitySwitch | E | Right shoulder |
| Pause | Escape | Start |

All six actions are Buttons. A future controller consumes action events; a future mobile gesture adapter or Input System OnScreenButton targets the same intent. No raw-touch binding is assigned directly to Jump or Slide because every screen tap must not trigger gameplay indiscriminately. The template UI map and keyboard/gamepad/touch UI bindings are preserved. No C# wrapper is generated.

**Expected result / verification:** asset imports and maps/actions resolve; project-wide input registration retains the original asset GUID. **Common problems:** input assets alone do not move a character; there is deliberately no controller yet. **Status:** created; final report records checks.

## Task 01.08 — Rendering test

**Objective / why:** prove URP camera/light/material compatibility. **Prerequisites:** valid pipeline and renderer references.

**Implementation:** open `Assets/_Project/Scenes/Test/90_TestEnvironment.unity`, then **Window > General > Game**. Expect a dark background, ground, cyan cube, orange sphere, purple cube, and directional shadows. Select each quality level in **Edit > Project Settings > Quality** to compare budgets. Restore PC High afterward. Camera post-processing is disabled to keep the check inexpensive; no final art or effects are introduced.

**Expected result / verification:** visible geometry, no pink materials, valid light/camera, and no shader errors. **Common problems:** the Play start override launches Bootstrap; edit-mode Game view can inspect TestEnvironment directly. **Status:** authored; final report records capture and checks.

## Task 01.09 — Git and LFS

**Objective / why:** establish reproducible source history and efficient binary storage. **Prerequisites:** Git; LFS was missing and installed from the official portable distribution after its checksum matched the published checksum.

**Implementation:** repository uses `main`, local author `Suryansh Patel <suryanshpatel1483@gmail.com>`, `.gitignore`, `.gitattributes`, and local LFS filters/hooks. [Git LFS](https://git-lfs.com/) supplies binary pointer handling. Commit text Unity assets normally; use LFS for common art/model/audio/font binaries. The remote explicitly supplied by the user is `https://github.com/Suryansh1483/THREADLLINE.git` (spelling preserved). Git author identity is separate from GitHub authentication. Never store access tokens in source or command arguments.

**Expected result / verification:** generated files are ignored, LFS works, source and `.meta` files are tracked, commits have the supplied author. **Common problems:** a new shell may be needed after adding portable LFS to the user PATH; a remote push may require Git Credential Manager sign-in. **Status:** local baseline committed; final report records remote status.

## Task 01.10 — Code and test boundaries

**Objective / why:** keep Editor/test dependencies out of player builds. **Prerequisites:** Test Framework.

**Implementation:** use `Assets/_Project/Code/Runtime`, `Editor`, and `Tests`. Runtime assembly definitions are deferred until real runtime modules exist. The actual build-host utility uses an Editor-only `Threadline.Editor.asmdef`; foundation tests use an Editor-only test assembly with explicit references. Inspect **Window > General > Test Runner > EditMode** for available checks.

**Expected result / verification:** tests compile and pass, while player builds contain no NUnit test assembly. **Common problems:** `Tests` is not a special exclusion folder; the test `.asmdef` must enforce Editor-only/test references. **Status:** strategy documented; final report records test results.

## Task 01.11 — Documentation

**Objective / why:** preserve decisions for later phases. **Prerequisites:** implemented foundation.

**Implementation:** read root `README.md` and `Documentation/`: architecture, coding standards, asset guidelines/provenance, project structure, this setup guide, development log, full roadmap, and final verification report. Installed package versions are captured from manifest/lock.

**Expected result / verification:** future plans are explicitly distinguished from implemented behavior. **Common problems:** do not treat an empty folder or installed package as an implemented gameplay system. **Status:** written; results updated after verification.

## Task 01.12 — Build verification

**Objective / why:** prove compilation and packaging using the real Windows/Android toolchains. **Prerequisites:** all foundation assets saved and imports complete.

**Implementation:** under **File > Build Profiles**, inspect WindowsDevelopment (Windows x64) and AndroidDevelopment (ARM64, IL2CPP, ASTC). Enable Development Build, keep profiler autoconnect off, and use ignored `Builds/Windows` / `Builds/Android` outputs. Android uses Unity's bundled SDK/NDK/OpenJDK; inspect **Edit > Preferences > External Tools**. Smoke builds can explicitly put TestEnvironment first while production profiles keep Bootstrap first. No keystore, store upload, or release signing is needed for a debug APK.

**Expected result / verification:** successful BuildReport and output executable/APK. A packaged build alone does not prove device performance, store compliance, or production readiness. **Common problems:** IL2CPP and first-time shader/Gradle compilation take time; inspect actual build logs rather than interpreting a pending import as failure.

This host also reproduced `Unable to establish loopback connection`, caused by JDK Unix-domain socket connection failure. A controlled Gradle comparison failed without a short socket path and passed with it. Unity filters out the existing `JAVA_TOOL_OPTIONS` setting. The Editor-only `AndroidGradleSocketDirectory` build callback therefore temporarily uses the explicitly configured `jdk.net.unixdomain.tmpdir` directory as TEMP/TMP for Android builds, restoring both afterward. It uses no hardcoded host path and does nothing if that option is absent or its directory does not exist. The observed host option points to `C:/Android/java-sockets`.

The [JDK 17 networking documentation](https://docs.oracle.com/en/java/javase/17/docs/api/java.base/java/net/doc-files/net-properties.html) documents the socket-directory property and Windows TEMP fallback. A direct edit to the protected bundled JDK configuration was denied, so the original JDK configuration remains unchanged. No administrator access, JDK replacement, or global TEMP change is required by the project utility. **Status:** final report records actual builds and remaining limits.

## Task 01.13 — Final inspection

**Objective / why:** hand a clean, verified foundation to Phase 02. **Prerequisites:** saved assets and build results.

**Implementation:** inspect Console, load every scene, check missing scripts/materials/pipeline references and package resolution, run tests, verify metadata and ignore rules, then reopen the project. Read `PHASE01_VERIFICATION.md` for evidence and limitations.

**Expected result / verification:** no critical compilation errors or broken dependencies; documented risks and unresolved checks remain visible. **Common problems:** never clear or suppress Console errors merely to claim a clean project. **Status:** determined by final verification report, not this guide.
