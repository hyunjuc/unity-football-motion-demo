# ⚽ Unity Football Animation Demo

A playable Unity prototype exploring Humanoid animation retargeting, Blend Tree locomotion, and NavMesh-based AI — built as a portfolio piece to apply for a Unity-based gameplay/animation role (Sports Interactive / Football Manager).

Unity에서 Humanoid 애니메이션 리타겟, Blend Tree 로코모션, NavMesh 기반 AI를 직접 설계/구현한 플레이 가능한 프로토타입입니다. Sports Interactive(Football Manager, Unity 기반) 지원을 위한 포트폴리오 데모로 제작했습니다.

---

## 🎥 Demo Videos

| Version | Description | Link |
|---|---|---|
| **Final (Aug 18, 2026)** | Full playable demo — locomotion, kick/dribble, NPC AI, tackle/steal system, minimap HUD | [Watch on YouTube](https://www.youtube.com/watch?v=2js0veelY08) |
| Aug 17, 2026 | NPC AI + tackle system in progress (debug build) | [Watch on YouTube](https://www.youtube.com/watch?v=FSCnZR0TyOM) |
| Aug 14, 2026 | Early locomotion + kick prototype (debug build) | [Watch on YouTube](https://www.youtube.com/watch?v=20wgbTpsvus) |

최종본(8/18)이 완성된 데모이며, 나머지 두 영상은 개발 과정을 보여주는 디버그 빌드입니다.

---

## ✨ Features

- **WASD movement** via CharacterController with a hand-built 2D Freeform Directional **Blend Tree** (Idle / Walk / Run / Turn)
- **Humanoid retargeting pipeline** — Mixamo clips retargeted onto a shared Avatar, reused across Player and NPC
- **Kick / dribble action** — physics-based ball (Rigidbody impulse on kick, kinematic dribble-follow otherwise, real soccer-ball texture)
- **NavMesh-driven NPC opponent** — chases the player, reuses the exact same Animator Controller as the player
- **Tackle / steal system** — bidirectional: NPC can tackle the player to steal the ball, and the player can tackle the NPC back; includes possession-aware flee/wait AI once a side has the ball
- **Minimap HUD** — top-down camera + RenderTexture, radar-style player/NPC position tracking (like a real football game)
- **Cinemachine 3rd-person camera** with a broadcast-style framing (world-space binding, lower-third composition)

---

## 🛠 Tech Stack

- Unity 6 (6000.0.63f1), URP
- Animator (Blend Tree, Any-State transitions, shared Controller for Player/NPC)
- NavMesh / AI Navigation package
- Cinemachine
- Mixamo (Humanoid animation source)
- C# (CharacterController-driven movement, no root motion — all motion is scripted for full control)

---

## 📝 Cover Letter Blurb

> Drawing on experience with motion-matching-based locomotion in UE5, I built a Unity football-character prototype to validate the same concepts on a different engine. Since Unity has no native motion-matching solution following the deprecation of the experimental Kinematica package, I implemented a hand-authored Animator Blend Tree and a Humanoid retargeting pipeline instead — resulting in a playable demo with natural Idle-Walk-Run-Turn transitions, a physics-based kick/dribble action, a NavMesh-driven NPC opponent, and a bidirectional tackle/steal mechanic shared across both characters via a single Animator Controller.

> UE5 환경에서 모션매칭 기반 로코모션 시스템을 다뤄온 경험을 바탕으로, Unity에서도 동일한 개념을 검증하기 위한 축구 캐릭터 프로토타입을 제작했습니다. Unity는 실험적이었던 Kinematica 패키지가 폐기된 이후 네이티브 모션매칭을 지원하지 않기 때문에, 직접 설계한 Animator 블렌드트리와 Humanoid 리타겟 파이프라인을 활용해 Idle-Walk-Run-Turn 간의 자연스러운 전환, 물리 기반 킥/드리블 액션, NavMesh 기반 NPC, 그리고 플레이어와 NPC가 하나의 Animator Controller를 공유하며 서로 태클/공 뺏기를 주고받는 시스템까지 포함한 플레이 가능한 데모를 구현했습니다.

---

## 📌 Notes

- Debug visualization tools (animation state HUD, direction arrows, trajectory prediction) were built during development and disabled before the final recording.
- Foot sliding and animation popping were minimized but not fully solved via Foot IK — scoped out due to time constraints (documented tradeoff, not an oversight).

---

## 📄 Credits & License

- Character models and animation clips (Idle, Walk, Run, Kick, Soccer Tackle, Soccer Tackle In Place, Soccer Tackle In Run, etc.) sourced from **Adobe Mixamo** (mixamo.com), used under Mixamo's free license for this portfolio demonstration.
- Raw Mixamo assets (`Assets/Football/Animations/`) are **not redistributed** in this repository per Mixamo's license terms — only original code, scenes, and project configuration are included. To run this project locally, download the equivalent clips from mixamo.com for the "Y Bot" character and place them in `Assets/Football/Animations/`.
- All gameplay code, Animator/Blend Tree design, and system architecture (locomotion, ball physics, NPC AI, tackle/steal system, minimap) are original work by the author.

- 캐릭터 모델 및 애니메이션 클립(Idle, Walk, Run, Kick, Soccer Tackle, Soccer Tackle In Place, Soccer Tackle In Run 등)은 **Adobe Mixamo**(mixamo.com)에서 제공받았으며, 포트폴리오 데모 목적으로 Mixamo 무료 라이선스 하에 사용했습니다.
- Mixamo 원본 에셋(`Assets/Football/Animations/`)은 라이선스 정책상 이 repository에 재배포하지 않으며, 코드/씬/프로젝트 설정만 포함되어 있습니다. 로컬에서 직접 실행하려면 mixamo.com에서 "Y Bot" 캐릭터 기준으로 동일한 클립을 받아 `Assets/Football/Animations/` 경로에 넣어주세요.
- 이동/애니메이션 시스템, 공 물리, NPC AI, 태클/스틸 시스템, 미니맵 등 모든 게임플레이 코드와 설계는 직접 제작한 오리지널 작업입니다.
