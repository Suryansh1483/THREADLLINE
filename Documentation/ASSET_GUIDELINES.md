# Asset guidelines

## Ownership and provenance

Original work belongs in `Assets/_Project`. Third-party imports belong in `Assets/_External/<Vendor>/<Pack>` unless vendor tooling requires another path. UPM packages stay in `Packages` and their managed cache. Preserve vendor namespaces and folder dependencies. Record every imported asset in `ASSET_PROVENANCE.md`, including source URL, author, version/date, license evidence, permitted redistribution, and modifications. Never commit an asset whose license forbids the intended repository distribution.

Unity packages are identified by the manifest/lock and carry their own package license files. Do not copy package cache contents into project source. Foundation test meshes are Unity primitives; foundation materials use Unity's URP Lit shader.

## Names and imports

Follow `CODING_STANDARDS.md`. Retain `.meta` files and move assets through Unity. Never enable Read/Write merely as a precaution; enable it only for a real CPU-side consumer. Do not apply blanket lossy conversions to source files. Keep editable originals and reimportable settings.

### Textures

Choose dimensions by visible screen coverage and profiling. Start with power-of-two textures where appropriate, use mipmaps for 3D surfaces, and disable mipmaps for pixel-aligned UI sprites when justified. Color textures normally use sRGB; masks and other numeric data normally do not. Import normal textures as Normal map. Use Windows platform compression appropriate to the content and supported hardware. Android starts with ASTC for the selected modern-device tier; use a separate ETC2 profile or tested texture targeting if broader device coverage is required. ASTC is a device-support decision, not a guarantee for every GLES3 phone. Avoid unnecessary 4K textures on mobile.

### Models

Use a consistent meter scale (1 Unity unit = 1 meter), deliberate pivots, and applied source transforms. Import only needed animation clips, blend shapes, and materials. Keep production materials under project ownership rather than extracting duplicates repeatedly. Generate or author simple colliders explicitly later; complex render meshes are not a default collision solution. Add LODs from measured distance/screen-size needs.

### Animation

Choose Humanoid or Generic deliberately. Record source rig, avatar, root motion expectations, clip ranges, loop behavior, and compression tradeoffs. No player rig, animator controller, blend tree, or root-motion policy is implemented in Phase 01. Animation Rigging is available for the later traversal phase.

### Audio

Keep licensed source audio. Select mono for genuinely spatial point-source SFX, stereo for appropriate music/ambience, and test codec artifacts. Choose Decompress On Load for short frequently reused sounds, Compressed In Memory for suitable medium clips, and Streaming for long music only when profiling supports it. Route future sources through Music, SFX, and UI groups in an Audio Mixer when the first audio assets are introduced. No empty mixer or audio playback manager is required now.

### UI, fonts, and VFX

Use licensed fonts. uGUI includes TextMeshPro support in this Unity version; import TMP Essential Resources when the first TMP text is authored, then record font provenance. No UI or font assets are needed for camera-only foundation scenes. Prefer Particle System for the initial portable effects. VFX Graph and Recorder remain deferred until a concrete production use justifies their dependency and platform cost. Shader Graph comes through URP with matching package versions.

## Temporary and generated files

Keep experiments under ignored `_Temporary` or outside `Assets`. Ignoring a file in Git does not prevent it entering a Unity build: committed scenes, Resources, and persistent assets must not reference it. Do not store downloads, captures, builds, or generated reports in the source asset tree. Build outputs and verification captures go under ignored `Builds` and `Artifacts`.
