# 🧠 Project Memory & Active Status: SUIF (Scalable UI Framework)

> **Note:** This file is read by Antigravity agents first. It stores high-level architectural decisions, active features, and gotchas for SUIF.
> Maintained in `.foragents/` at the root of the SUIF repository.

---

## 📌 1. Active Status & Task Tracker

- [x] Cloned `https://github.com/ReenBo/SUIF.git` and configured `develop` branch.
- [x] Connected `.agents` (AI Workflow v1.1.0) as submodule.
- [x] Initialized `.foragents/` infrastructure.
- [x] Setup UPM package structure (`package.json`, `Runtime/Core`, `Runtime/Integrations`, `Editor`, `Documentation~`, `Samples~`).
- [x] Ported and modularized proven UIFramework source files into `SUIF.Core`, `SUIF.VContainer`, `SUIF.Addressables`, and `SUIF.R3`.
- [x] Implemented 1-click Window Wizard Editor tool (`Tools > SUIF > Create UI Window Wizard...`).
- [x] Created comprehensive documentation suite in `Documentation~/` and `README.md`.
- [x] Created all 5 UPM Samples in `Samples~/`.
- [x] Created `CHANGELOG.md` for version 1.0.0.
- [ ] **Next**: Validate package installation into test project or host project via UPM Git URL or submodule.

---

## 📐 2. Architectural Decisions Record (ADR)

* **Architecture**: Declarative MVVM + UI Toolkit + SMACSS CSS Architecture for Unity 6+.
* **Package Format**: Root UPM Package (`package.json` at repo root, installable via UPM Git URL or submodule).
* **Dependency Strategy**: **Inverted Dependencies with Version Defines**:
  * `SUIF.Core` (asmdef: `SUIF.Core`): Zero-dependency core contracts (`IUIWindow`, `IUIView`, `IUIViewModel`, `IUIAssetProvider`, `IUIDependencyResolver`).
  * `SUIF.VContainer` (asmdef: `SUIF.VContainer`): First-class VContainer support, auto-activated via `versionDefines` (`jp.hadashikick.vcontainer`).
  * `SUIF.Addressables` (asmdef: `SUIF.Addressables`): First-class Addressables support, auto-activated via `versionDefines` (`com.unity.addressables`).
  * `SUIF.R3` (asmdef: `SUIF.R3`): Reactive UI Toolkit bindings, auto-activated via `R3`.
* **Zero-Allocation**: No heap allocations, boxing, or LINQ in frequent runtime loops, binding callbacks, or `ITickable.Tick()`.
* **String Formatting**: `ZString` for high-frequency runtime text updates.
* **UI Toolkit Rules**:
  * Strict SMACSS layering (`1_Base`, `2_Layout`, `3_Modules`, `4_States`, `5_Themes`).
  * `picking-mode="Ignore"` inline on transparent wrapper layers in UXML.
  * Virtualized ListView recycling uses `IView.Unbind()` / `ViewBinder.Clear()` to preserve ViewModels and CompositeDisposables.

---

## ⚠️ 3. Known Gotchas & Technical Debt

* *Samples Folder*: Must use `Samples~` with tilde `~` so Unity doesn't import sample assets into users' projects automatically until imported via Package Manager.
* *Documentation Folder*: Must use `Documentation~` with tilde `~` so Unity ignores markdown files during asset importing.
* *Version Defines*: Integration asmdefs require `versionDefines` matching exact package name identifiers.

---

## 📜 4. Last Session Log

* **Date**: 2026-10-08
* **Role**: Primary Agent / Task Analyzer & Code Architect
* **Summary**: Built complete SUIF 1.0.0 package on develop branch. Decoupled Core from VContainer/Addressables via IUIDependencyResolver/IUIAssetProvider with versionDefines modules. Created Editor Window Wizard, 5-sample suite, and comprehensive documentation manuals.
