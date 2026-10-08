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
- [x] Elevated **UniTask** to Gold Standard with mandatory `CancellationToken` passing throughout all async APIs.
- [x] Integrated all accumulated knowledge base instructions (ROADMAP, 8 Zero-Allocation rules, SMACSS guidelines & engine gotchas) into `Documentation~/`.
- [x] Implemented 1-click Window Wizard Editor tool (`Tools > SUIF > Create UI Window Wizard...`).
- [x] Created all 5 UPM Samples in `Samples~/`.
- [x] Created `CHANGELOG.md` (v1.0.1).
- [ ] **Next**: Validate package installation into test project or host project via UPM Git URL or submodule.

---

## 📐 2. Architectural Decisions Record (ADR)

* **Architecture**: Declarative MVVM + UI Toolkit + SMACSS CSS Architecture for Unity 6+.
* **Async Engine**: **UniTask as Gold Standard**:
  * Struct-based value-type async/await throughout.
  * Mandatory `CancellationToken ct = default` across all async APIs (`IUIFlow`, `IUIAssetProvider`, `IViewFactory`, `IUIThemeService`, `IUIWindowManager`).
  * Modal dialogs and confirmations use `UniTaskCompletionSource<T>`.
  * Startup initialization uses VContainer `IAsyncStartable`.
* **Dependency Strategy**: **Inverted Dependencies with Version Defines**:
  * `SUIF.Core` (asmdef: `SUIF.Core`): Zero-dependency core contracts (`IUIWindow`, `IUIView`, `IUIViewModel`, `IUIAssetProvider`, `IUIDependencyResolver`).
  * `SUIF.VContainer` (asmdef: `SUIF.VContainer`): First-class VContainer support, auto-activated via `versionDefines` (`jp.hadashikick.vcontainer`).
  * `SUIF.Addressables` (asmdef: `SUIF.Addressables`): First-class Addressables support, auto-activated via `versionDefines` (`com.unity.addressables`).
  * `SUIF.R3` (asmdef: `SUIF.R3`): Reactive UI Toolkit bindings, auto-activated via `R3`.
* **8 Mandatory Zero-Allocation Rules**:
  1. Strings & Enums: Switch expressions with string constants (no `.ToString()` or `$"..."`).
  2. Dictionaries over loops: O(1) state tracking.
  3. No idle UI Toolkit calls: Check before `BringToFront()` or `RemoveFromClassList()`.
  4. No over-engineering for raw visuals: Lightweight direct element loading for `ModalBlocker`.
  5. No allocations for debugger aesthetics: Static names only.
  6. Strict restriction on `static`: Only stateless generic caches (`UIViewAttributeCache<T>`).
  7. No magic strings: Extracted into constants.
  8. Strict prohibition of LINQ: Manual indexed loops and break statements.
* **UI Toolkit Engine Gotchas**:
  * `TemplateContainer` 0x0 collapse fix: `.l-layer > * { flex-grow: 1; }`.
  * `picking-mode` is a UXML attribute, NOT a USS property: set `picking-mode="Ignore"` inline in UXML.

---

## 📜 4. Last Session Log

* **Date**: 2026-10-08
* **Role**: Primary Agent / Task Analyzer & Code Architect
* **Summary**: Version 1.0.1 released on develop. Fully incorporated accumulated knowledge base (ROADMAP, 8 Zero-Allocation rules, SMACSS guide, layout gotchas). Formally established UniTask as the first-class Gold Standard across all async methods with mandatory CancellationToken support.
