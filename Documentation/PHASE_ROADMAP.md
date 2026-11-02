# THREADLINE phase roadmap

Only Phase 01 is implemented by the current work. Later entries are plans, not shipped systems.

1. **Project Foundation & Environment Setup** — Unity project, URP, settings, packages, folders, scenes, Git, and documentation.
2. **Core Architecture & Game Framework** — Lifecycle, state management, scene loading, events, interfaces, configuration, save architecture, and logging.
3. **Player Controller & Movement** — Character movement, three-lane movement, forward motion, jumping, sliding, collision, and basic animation.
4. **Kinetic Traversal & Animation** — Vaulting, wall running, rail grinding, dash, animation blending, IK, camera behavior, and Flow.
5. **Reality Weaving & Dimension System** — Reality, Glitch, and Void switching, route changes, energy/cooldowns, collision management, visual transitions, and route validation.
6. **Environment & World Art Production** — Art direction/bible, futuristic city, modular environments, props, materials, lighting, LODs, and environment animation.
7. **Procedural World Generation** — Modular track segments, traversal graph, streaming, pooling, route selection, seeded generation, safe-path validation, and difficulty scaling.
8. **Obstacles, Hazards & Gameplay Systems** — Obstacles, hazards, collectibles, power-ups, environmental events, Null Chase, and collision systems.
9. **Scoring, Flow & Progression** — Scoring, Flow combinations, rewards, missions, cosmetics, upgrades, achievements, and progression.
10. **Character & Animation Production** — Final protagonist, rigging, animation library, reactions, transitions, and cosmetics.
11. **UI/UX & Audio Production** — Main menu, HUD, pause, game over, missions, customization, accessibility, sound effects, and music.
12. **Online Services & Multi-user Systems** — Authentication, profiles, cloud saves, leaderboards, asynchronous ghost races, run validation, and secure APIs. Real-time multiplayer is excluded from version 1.0.
13. **World Expansion & Content Completion** — Neo City, Reactor Core, Glitch District, The Void, Frozen Timeline, Null Realm, missions, and story content.
14. **Optimization, QA & Compliance** — Performance, device testing, profiling, security, fairness, accessibility, licensing, privacy, and store compliance.
15. **Release & Post-launch** — Signing, store preparation, submission, release, monitoring, patches, and updates.

Future performance targets: at least 60 FPS on the selected PC baseline, 60 FPS on selected Android devices, and a 30 FPS fallback on lower-end supported Android devices. These require profiling and device validation. iOS remains an additional target subject to platform prerequisites and testing. Core gameplay stays offline-first; online features are asynchronous.
