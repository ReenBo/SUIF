# 📦 SUIF — Scalable UI Framework for Unity 6+

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Unity 6+](https://img.shields.io/badge/Unity-6000.0%2B-black.svg)](https://unity.com/)
[![Architecture: MVVM](https://img.shields.io/badge/Architecture-MVVM-green.svg)](#)
[![Performance: Zero--GC](https://img.shields.io/badge/Performance-Zero--GC-brightgreen.svg)](#)

**SUIF** is an ultra-performant, modular, declarative MVVM UI Toolkit framework engineered for **Unity 6+**. Features zero-allocation reactive data binding, SMACSS 5-layer styling, scoped window focus management, and out-of-the-box VContainer and Addressables integration.

---

## ⚡ Highlights

* **🏎️ Extreme Performance**: Zero heap allocations in runtime update loops using `ZString` and virtualized element recycling.
* **🧩 Inverted Dependency Architecture**: Clean POCO core (`SUIF.Core`) with auto-activating VContainer, Addressables, and R3 modules via `versionDefines`.
* **🎨 SMACSS 5-Layer Styling**: Eliminates style conflicts and enables instant runtime theme swapping (`TSS.tss`).
* **🧙 1-Click Window Wizard**: Built-in editor generator for rapid View + ViewModel + UXML + USS scaffolding.
* **💡 Complete Samples Included**: Dedicated UPM samples for every feature.

---

## 📚 Documentation
* 📘 [Architecture Manual](Documentation~/Architecture.md)
* ⚡ [Performance & Optimization Guide](Documentation~/Performance_And_Optimization.md)
* 🎨 [SMACSS Architecture Guide](Documentation~/CSS_Architecture_SMACSS.md)
* ⚖️ [Strengths & Trade-offs](Documentation~/Strengths_And_Tradeoffs.md)
* 🚀 [Getting Started Guide](Documentation~/Getting_Started.md)

---

## 📦 Installation

Add via Unity Package Manager:
```text
https://github.com/ReenBo/SUIF.git
```

## 📄 License
MIT License. Developed by ReenBo.
