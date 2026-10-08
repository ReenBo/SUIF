# 📦 SUIF — Scalable UI Framework for Unity 6+

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Unity 6+](https://img.shields.io/badge/Unity-6000.0%2B-black.svg)](https://unity.com/)
[![Architecture: MVVM](https://img.shields.io/badge/Architecture-MVVM-green.svg)](#)
[![Performance: Zero--GC](https://img.shields.io/badge/Performance-Zero--GC-brightgreen.svg)](#)
[![Async: UniTask](https://img.shields.io/badge/Async-UniTask%20Gold%20Standard-orange.svg)](#)

**SUIF** is an ultra-performant, modular, declarative MVVM UI Toolkit framework engineered for **Unity 6+**. Features zero-allocation reactive data binding, SMACSS 5-layer styling, scoped window focus management, first-class **UniTask** async execution, and out-of-the-box VContainer and Addressables integration.

---

## ⚡ Highlights

* **🏎️ Extreme Performance**: Strict Zero Heap Allocation in runtime update loops using `ZString`, indexed loops, and virtualized element recycling.
* **⚡ UniTask as Gold Standard**: Struct-based value-type async/await with mandatory `CancellationToken` support across all operations.
* **🧩 Inverted Dependency Architecture**: Clean POCO core (`SUIF.Core`) with auto-activating VContainer, Addressables, and R3 modules via `versionDefines`.
* **🎨 SMACSS 5-Layer Styling**: Eliminates style conflicts and enables instant runtime theme swapping (`TSS.tss`) without scene reloading.
* **🧙 1-Click Window Wizard**: Built-in editor generator for rapid View + ViewModel + UXML + USS scaffolding (`Tools > SUIF > Create UI Window Wizard...`).
* **💡 Complete Samples Included**: 5 dedicated UPM samples covering all framework capabilities.

---

## 📚 Complete Documentation Suite

* 🗺️ [Project Roadmap & Core Philosophy](Documentation~/ROADMAP.md)
* ⚡ [UniTask: The Async Gold Standard](Documentation~/UniTask_Gold_Standard.md)
* 📘 [Architecture Manual](Documentation~/Architecture.md)
* 🚀 [Performance & 8 Zero-Allocation Rules](Documentation~/Performance_And_Optimization.md)
* 🎨 [SMACSS Architecture Guide & Layout Gotchas](Documentation~/CSS_Architecture_SMACSS.md)
* ⚖️ [Strengths & Trade-offs](Documentation~/Strengths_And_Tradeoffs.md)
* 🚀 [Getting Started Guide](Documentation~/Getting_Started.md)

---

## 📦 Installation

Add via Unity Package Manager:
```text
https://github.com/ReenBo/SUIF.git
```

Or as a Git Submodule:
```bash
git submodule add -b develop https://github.com/ReenBo/SUIF.git Packages/com.reenbo.suif
```

## 📄 License
MIT License. Developed by ReenBo.
