# 🎲 Zonk Core Showcase

![.NET CI](https://github.com/w1r2s/zonk-core-showcase/actions/workflows/dotnet.yml/badge.svg)

A showcase of **core logic and AI strategy** for the dice game *Zonk (Farkle)*, implemented in **C# / .NET 8**.  
This repository contains only the **algorithmic layer** — without Unity scenes or assets.

---

## 🧩 Overview

The project demonstrates:

- Modular architecture (Core + Tests)
- Scoring and turn logic system
- Multiple AI agents with distinct behaviors
- Full **xUnit** test suite with CI integration
- FSM-based adaptive decision-making

---

## 🤖 AI Strategies

The project implements four autonomous agents:

- **AggressiveAI** — pushes for a high per-turn score; continues rolling while risk is acceptable, often targeting ≥600 points and occasionally chasing extra gain.  
- **ConservativeAI** — prefers steady progress; banks early once a minimal safe score (≈300–1500 pts) is achieved or few dice remain.  
- **CautiousAI** — greedily takes **all available scoring combinations** on the roll and **ends the turn** (no rerolls); emphasizes guaranteed gain over exploration.  
- **FSMAdaptiveAI** — hybrid finite-state agent that switches between Aggressive and Conservative modes depending on game context (score difference, zonks, etc.).

| Agent            | Roll behavior                          | Banking policy                    |
|------------------|----------------------------------------|-----------------------------------|
| AggressiveAI     | Continues if risk acceptable            | Banks on high gain / low dice     |
| ConservativeAI   | Rerolls only with enough dice left      | Banks early (safe thresholds)     |
| CautiousAI       | Takes **all** scoring combos, no reroll | **Immediately banks**             |
| FSMAdaptiveAI    | Depends on current mode                 | Inherits mode’s policy            |

---

## 🎮 AI Match Demo

Short, accelerated 30-second clip showing a complete match  
between two autonomous agents (Conservative vs. Adaptive).

![Gameplay demo](docs/zonk-demo.gif)

---

## 🧪 Testing & CI

- **xUnit** test suite — all tests passing  
- **GitHub Actions CI** — automated build and test validation  
- Optional HTML test report (via `dorny/test-reporter`)

---

## License
See [LICENSE](./LICENSE) for usage terms.
