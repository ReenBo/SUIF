# 🧠 Project Memory & Active Status: SUIF (Scalable UI Framework)

> **Note:** This file is read by Antigravity agents first. It stores high-level architectural decisions, active features, and gotchas for SUIF.
> Maintained in `.foragents/` at the root of the SUIF repository.

---

## 📌 1. Active Status & Task Tracker

- [x] Cloned `https://github.com/ReenBo/SUIF.git` and configured `develop` branch.
- [x] Connected `.agents` (AI Workflow v1.1.0) as submodule.
- [x] Initialized `.foragents/` infrastructure.
- [/] **In Progress**: Setup UPM package structure (`package.json`, `Runtime/Core`, `Runtime/Integrations`, `Editor`, `Documentation~`, `Samples~`).
- [ ] **Next**: Port and modularize proven UIFramework source files into `SUIF.Core`, `SUIF.VContainer`, `SUIF.Addressables`, and `SUIF.Reactive`.
- [ ] **Next**: Create comprehensive documentation (`Documentation~/`) and UPM samples (`Samples~/`).

---

## 📐 2. Architectural Decisions Record (ADR)

* **Architecture**: Declarative MVVM + UI Toolkit + SMACSS CSS Architecture for Unity 6+.
* **Package Format**: Root UPM Package (`package.json` at repo root, installable via UPM Git URL or submodule).
* **Dependency Strategy**: **Inverted Dependencies with Version Defines**:
  * `SUIF.Core` (asmdef: `com.reenbo.suif.core`): Zero-dependency core contracts (`IUIWindow`, `IUIView`, `IUIViewModel`, `IUIAssetProvider`, `IUIDependencyResolver`).
  * `SUIF.VContainer` (asmdef: `com.reenbo.suif.vcontainer`): First-class VContainer support, auto-activated via `versionDefines` (`jp.hadashikick.vcontainer`).
  * `SUIF.Addressables` (asmdef: `com.reenbo.suif.addressables`): First-class Addressables support, auto-activated via `versionDefines` (`com.unity.addressables`).
  * `SUIF.Reactive` (asmdef: `com.reenbo.suif.reactive`): Reactive UI Toolkit bindings, auto-activated via `versionDefines` (`R3`).
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
* *Version Defines*: Always declare `versionDefines` in integration asmdefs so absence of external packages does not produce compiler errors.

---

## 📜 4. Last Session Log

* **Date**: 2026-10-08
* **Role**: Primary Agent / Task Analyzer & Architect
* **Summary**: Initialized SUIF repository on develop branch. Added .agents submodule and .foragents memory tracking. Commencing UPM package scaffolding and code migration.
