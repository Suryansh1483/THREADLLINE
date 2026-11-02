# Phase 01 verification report

Date: 2026-11-03 (Asia/Calcutta). Status: non-Android foundation complete and verified; Android setup and packaging explicitly deferred by the user.

## Verified foundation

| Check | Evidence |
| --- | --- |
| Correct project and Editor | Existing THREADLINE project, Unity 6000.3.27f1; CLI 1.0.0-beta.15 |
| Folder ownership | `_Project`, `_External`, ignored `_Temporary`; 76 initial folders created through Unity |
| Metadata | Filesystem inspection found 0 missing asset/folder `.meta` files |
| Naming and standards | `CODING_STANDARDS.md`, `ASSET_GUIDELINES.md`, `PROJECT_STRUCTURE.md` |
| Rendering | Five valid URP assets, valid renderer references, SRP Batcher enabled |
| Quality defaults | PC High for Standalone, Android Medium for Android |
| Color and input backend | Linear color; Input System Package (New) |
| Packages | Cinemachine 3.1.7 and Animation Rigging 1.4.1 installed; registered package versions match the lock |
| Shader Graph compatibility | URP, Core, and Shader Graph resolve to 17.3.0; no HDRP dependency |
| Production scenes | Bootstrap, MainMenu, Gameplay enabled in order; render test excluded |
| Input asset | Six Gameplay actions have keyboard/gamepad bindings; UI map and project-wide registration preserved |
| Scene integrity | Four foundation scenes load through isolated previews with no missing scripts or materials |
| Render output | Visually inspected 1280x720 capture: cyan cube, orange sphere, purple cube, ground, directional lighting/shadows; no pink materials |
| Live Play mode | Frame counter advanced from 271 to 638; correct URP asset and render-test scene active |
| Code boundary | Editor-only build utility and test assemblies; no runtime gameplay classes |
| Automated checks | 12 passed, 0 failed, 0 skipped in 0.85 seconds after adding the scoped build-host utility |
| Windows build | StandaloneWindows64, Mono development build, Succeeded, 0 errors, 0 warnings |
| Final Windows rebuild | 16.6 seconds; 168,119,842 bytes; 0 errors and 0 warnings |
| Final Editor reopen | WindowsDevelopment, PC High, Bootstrap; compilation finished; Console 0 errors and 0 warnings |
| Source boundaries | 0 temporary dependencies and 0 Editor/test assemblies included in the player |
| Windows launch | Headless player loaded assemblies and initialized PhysX; no exception/load-error matches in its smoke log |
| Android tooling | Installed Android build module; bundled SDK/NDK/JDK/Gradle paths recognized |
| Android tool versions | JDK 17.0.18; NDK 27.2.12479018 (r27c); target API 36 installed |
| Android configuration | IL2CPP, ARM64, minimum API 26, target API 36, OpenGLES3, ASTC development profile |
| Local Git | Initialized `main`, author supplied by user; baseline, configuration, and verification committed separately |
| LFS | Git LFS 3.8.1; official archive hash matched published SHA-256; local filters/hooks initialized |
| Git exclusions | Library/Temp/Logs/UserSettings/Builds/Artifacts and `_Temporary` probes ignored |
| LFS/text separation | FBX/PNG/WAV probes use LFS; `.unity` and `.meta` probes do not |
| Remote access | User-provided origin `https://github.com/Suryansh1483/THREADLLINE.git`; authenticated push succeeded and `main` tracks `origin/main` |
| Documentation | README, architecture, standards, assets/provenance, setup guide, inventory, development log, and complete 15-phase roadmap |

## Build scope

The development Build Profiles retain the production scene order beginning at Bootstrap. Smoke builds explicitly place `90_TestEnvironment` first, followed by all three production scenes. This validates the renderer in a packaged build without changing production inclusion. Bootstrap contains no automatic transition or initialization system yet.

Windows output: `Builds/Windows/THREADLINE.exe` plus its adjacent data/runtime files. Distribute the entire output folder when testing, not the executable alone. The headless launch uses the Null graphics device and does not prove standalone GPU rendering or performance; the live Editor capture supplies the visual renderer check.

Android setup and builds are paused at the user's request. No successful APK, signature inspection, or device validation is claimed. Existing Android settings, profile, native build cache, diagnostics, and the unit-checked socket-path utility are preserved for the later Android task. The last active Gradle client was stopped, its blocking notice dismissed, and the Editor's original TEMP/TMP environment values were confirmed restored.

## Local evidence

Generated evidence is intentionally ignored by Git:

- `Artifacts/Phase01/editmode-tests.json`
- `Artifacts/Phase01/editmode-tests-final.json`
- `Artifacts/Phase01/windows-build-report.json`
- `Artifacts/Phase01/windows-build-report-final.json`
- `Artifacts/Phase01/final-core-inspection.json`
- `Artifacts/Phase01/console-after-final-reopen.json`
- `Artifacts/Phase01/windows-player.log`
- `Artifacts/Phase01/render-test.png`
- Gradle controlled-reproduction logs and thread diagnostics; these are local investigation evidence, not a successful Android build report

Source-level checks live in `Assets/_Project/Code/Tests/FoundationTests.cs`, with their Editor-only test assembly definition alongside them. Run through **Window > General > Test Runner > EditMode**.

## Recovered setup issues

- A registry query timed out during package import. Exact package versions were verified using official Unity 6.3 documentation and then confirmed in UPM's registered package list.
- Domain reloads interrupted a queued scene command. Its effects were checked before rerunning; no foundation scenes or materials had been created by the interrupted request.
- Authoring snippets needed Unity 6 API namespace/extension adjustments; those transient snippets were compiled before execution, so failed compilation did not partially create scene assets.
- Windows Build Profile creation initially matched both Windows Player and Server. Explicit Windows/Player selection resolved the ambiguity.
- The first scene-integrity test tried to restore an empty Test Runner scene setup. It now loads isolated preview scenes without changing the user's scene setup. The final expanded suite passes all twelve cases.
- Unity emitted transient package-import diagnostics while removing Visual Scripting and importing bundled shader assets. These are distinguished from project compile errors and build results; no package source or warning suppression was added to conceal them.
- An installer command for Git LFS was rejected by automatic approval review without a detailed reason. The official portable archive was used successfully instead.
- Android Gradle reproduced a JDK Unix-domain socket failure. Short-path experiments isolated the cause. An Editor-only workaround and regression tests were added without changing global environment settings or protected JDK files. APK verification remains deferred, so the workaround is not described as fully validated for packaging.

## Remaining limits

- No Android device is connected. APK installation, touch hardware, device GPU compatibility, battery/thermal behavior, and frame rate are untested.
- iOS, release signing, store submission, and Windows IL2CPP release tooling are not verified by these development builds.
- Company name and `com.example.threadline` identifiers remain explicit placeholders. Replace them before distribution under final branding.
- Android ASTC targets selected modern devices; broader compatibility may require tested ETC2 builds or texture targeting.
- PC VSync follows display refresh. Android's explicit 60/30 FPS runtime policy belongs to Phase 02 configuration and is not implemented by Phase 01 settings alone.
- TMP support is present through uGUI; font resources, actual UI, audio mixing, controllers, traversal, procedural generation, saves, and online features remain assigned to later phases.
- A successful foundation build does not establish production readiness or performance targets.

The project reopened successfully and retained its settings. The non-Android foundation is ready for Phase 02 work when requested. Android remains deferred; this report does not mark the entire cross-platform Phase 01 complete. Repository commit and synchronization receipts are recorded in the development log.
