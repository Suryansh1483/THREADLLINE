# Asset provenance

| Asset | Origin | Rights and evidence | Modifications |
| --- | --- | --- | --- |
| Foundation scene primitives | Unity built-in primitives | Engine-provided primitive meshes; project use under applicable Unity terms | Position, scale, and names only |
| Foundation test materials | Authored during Phase 01 | Project-authored parameter values using Unity URP Lit | Four simple colors and smoothness |
| URP renderer/configuration assets | Existing Universal 3D project template | Unity-provided template and URP package; preserve applicable Unity package licenses | Organized into `_Project`, quality settings adjusted |
| Input UI map | Existing Unity Input System template | Unity-provided template/package; preserve applicable package license | Existing UI map retained; Gameplay actions authored for THREADLINE |

No external art, fonts, models, music, character likenesses, or third-party asset packs were added. UPM dependency versions are recorded in `Packages/packages-lock.json`; each dependency includes its package license in the managed package cache. Preserve any notices required by dependency licenses when distributing builds.

For every future import add: exact asset paths, source URL, author/vendor, version/download date, license name and retained evidence location, commercial-use and redistribution constraints, AI generation tool/reference provenance when applicable, and modifications made. An empty `_External` directory does not imply any external assets are licensed.
