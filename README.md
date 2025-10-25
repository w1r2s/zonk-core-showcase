# Zonk / Farkle — Core Logic

This repository contains the **core gameplay logic** extracted from a private Unity/Android implementation of the classic dice game *Zonk (Farkle)*.

It demonstrates:
- deterministic scoring rules,
- AI decision-making strategies,
- turn state management,
- and clear separation between Unity-dependent code and pure C# logic.

---

## Structure

| Folder | Purpose |
|---------|----------|
| `core/Scoring` | Scoring rules, combination detection, Zonk checks |
| `core/TurnEngine` | Turn-state updates (keep, bank, zonk) |
| `core/AI` | Base class and strategy implementations (Aggressive, Conservative, Cautious, FSM) |
| `tests` | Unit tests validating scoring and strategy behavior |

---

## Runtime layering (original project)

| Unity class | Responsibility | Core equivalent |
|--------------|----------------|-----------------|
| **GameControl** | Match controller: manages rounds, player turns, and win conditions | Omitted (Unity adapter) |
| **GameRound** | Human turn adapter: handles UI/input, holds dice, applies keep/bank actions | Omitted (Unity adapter) |
| **AIController** | Agent turn adapter: holds strategy, calls decisions, applies result | Omitted (Unity adapter) |
| **BaseAI + strategies** | Decision logic for agents | Present in `core/AI` (pure C#) |
| **Scoring/Combo logic** | Calculated inline in GameRound/BaseAI | Extracted to `core/Scoring` |
| — | Aggregated state for decisions | `core/TurnEngine/GameState` |
| — | Turn reducers (keep, bank, zonk) | `core/TurnEngine/TurnLogic` |

> The Unity adapters (GameControl, GameRound, AIController) are intentionally not included.  
> They call into this core logic layer via `BaseAI.Decide(...)` and apply results in the scene.

---

## Zonk definition

A roll is considered a **Zonk** when:
- it is **not** a straight (1–6),
- it contains **no** set of three or more identical dice,
- and it has **no** single `1` or `5`.

Implemented as `ScoringService.IsZonkStrict(...)`.

---

## Notes

- The logic here mirrors the private Unity project one-to-one, excluding scene/UI code.  
- All calculations and AI behaviors are deterministic and testable.  
- A Unity adapter in the original project connects this logic to the visual layer.

---

## License

Copyright (c) 2025 [w1r2s]

This code is provided for educational and demonstration purposes only.  
Redistribution, modification, or use of this code (in whole or in part)  
for commercial or production purposes is **not permitted** without prior written consent of the author.

The repository contains only the algorithmic layer of the game and  
cannot be used to produce a playable build without additional assets and Unity-specific code.
