# Coding standards

## Names

Use descriptive names without redundant prefixes. C# filenames match their primary type, such as `RealityConfiguration.cs`. Types, methods, public properties, enums, and enum values use PascalCase. Interfaces use `I`, such as `IInteractable`. Locals and parameters use camelCase. Private fields use camelCase, including `[SerializeField] private float forwardSpeed;`. Constants use PascalCase. Namespaces start with `Threadline` and reflect ownership, such as `Threadline.Input` or `Threadline.Editor`.

| Asset | Example | Rule |
| --- | --- | --- |
| ScriptableObject type | `RealityConfiguration` | Describe the data and purpose |
| ScriptableObject asset | `RealityDefaults.asset` | Describe the instance |
| Prefab | `RealityRoadStraight.prefab` | Object and variant |
| Scene | `00_Bootstrap.unity` | Ordered production scenes; test scenes start at 90 |
| Material | `RealityRoadSurface.mat` | Subject and surface |
| Texture | `RealityRoad_BaseColor.png` | Subject and channel |
| Texture | `RealityRoad_Normal.png` | Import as Normal map |
| Model | `RealityRoadStraight.fbx` | Subject and variant |
| Animation | `RunnerJumpStart.anim` | Subject, action, stage |
| Animator controller | `RunnerLocomotion.controller` | Subject and purpose |
| Audio | `RunnerFootstepMetal01.wav` | Subject, action, variant |
| Particle prefab | `RealitySwitchBurst.prefab` | Effect and behavior |
| Shader Graph | `RealitySurface.shadergraph` | Visual purpose |
| UI sprite | `PauseIcon.png` | Purpose |
| UI prefab | `PauseButton.prefab` | Purpose and component |
| Input asset | `ThreadlineInput.inputactions` | Project and purpose |
| Action map/action | `Gameplay/RealitySwitch` | PascalCase; exact names form a contract |
| Configuration | `AndroidMedium_RPAsset.asset` | Platform, tier, asset role |
| JSON data | `player-save.json` | Lowercase kebab-case; schema version required when saves are implemented |

These examples describe conventions, not implemented gameplay classes or save systems.

## C# and responsibilities

Use four spaces, Allman braces, one statement per line, explicit access modifiers, and UTF-8. Keep imports at the top and Editor APIs outside runtime assemblies. Use `var` when the right-hand expression makes the type clear. Serialize private fields instead of exposing mutable public fields for Inspector access. Prefer composition and one clear responsibility per script. Introduce interfaces only for a concrete boundary or consumer.

Avoid scene-wide searches in per-frame callbacks, unnecessary allocations, and premature managers. Validate external data and required references. Handle recoverable failures explicitly; report useful context for unexpected failures. Never hide errors with empty catches, disabled tests, or blanket warning suppression. Comments explain decisions and constraints rather than restating code. Preserve existing comments unless the task changes their meaning.

## Code locations and assemblies

- Runtime code: `Assets/_Project/Code/Runtime/`.
- Editor-only utilities: `Assets/_Project/Code/Editor/`.
- Tests: `Assets/_Project/Code/Tests/`.

Do not create empty runtime assemblies or placeholder classes. Runtime code remains in Unity's default assembly until the first real runtime module justifies a boundary. The implemented Android build-host utility uses the Editor-only `Threadline.Editor` assembly. Foundation tests use `Threadline.Foundation.EditorTests`, referencing that utility and the packages they inspect. A folder named `Tests` alone does not exclude scripts from builds; the test assembly explicitly includes only Editor and enables Test Assemblies.

## Git workflow

Commit source, settings, dependency locks, and `.meta` files together. Never regenerate metadata to fix a reference problem. Start each phase from a clean committed state, create a short-lived branch such as `feature/phase02-framework`, validate a focused change, and commit with a descriptive message. Review before merging into `main`. Never commit credentials, signing keys, generated folders, or build outputs. Remote hosting remains optional until a private destination is chosen.
