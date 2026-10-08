# 🗺️ SUIF Project Roadmap & Core Philosophy

## 1. Core Philosophy

**SUIF (Scalable UI Framework)** is engineered from the ground up for Unity 6+ with a strict focus on **Zero Allocation**, **Extreme Performance**, and **Declarative MVVM**. The primary objective is to create a system that is both blisteringly fast at runtime and extraordinarily elegant for developers, eliminating boilerplate code through modern C# features and reactive data flows.

### Core Technology Stack:
* **Rendering & Layout:** Unity 6 UI Toolkit (USS / UXML)
* **Async Engine:** **UniTask** (Zero-Allocation struct async/await — Gold Standard)
* **Reactivity:** **R3** (UniRx Next Generation)
* **DI Container:** **VContainer** (via modular `versionDefines`)
* **String Handling:** **ZString** (Zero-garbage runtime formatting)
* **Asset Streaming:** **Addressables** (via modular `versionDefines`)
* **Code Generation / Boilerplate Elimination:** Cached Reflection (Current) & C# Source Generators (Roadmap)

---

## 2. Implementation Strategy by Category

### 1. Zero Allocation & Performance
* **No Heap-Allocating Loops:** Strictly forbid `foreach` on `IEnumerable`. Mandate `for` loops on `IReadOnlyList<T>` or `T[]`, or `foreach` on concrete types with struct enumerators (`List<T>`).
* **No LINQ:** `System.Linq` is strictly prohibited in runtime code paths. Filtering and projection are done via manual loops or custom, allocation-free R3 operators.
* **ZString by Default:** All runtime string formatting and concatenation must use `ZString.Format` or `ZString.Concat`. `string.Format`, `+`, and `$` are only permissible in `throw` statements, `[Conditional]` debug logs, and editor-only code.
* **List Pre-allocation:** When creating collections, always pre-allocate capacity if approximate size is known (e.g., `new List<T>(10)`).
* **Conditional Debugging:** Logging calls must be marked with `[Conditional("UNITY_EDITOR")]` or `[Conditional("DEVELOPMENT_BUILD")]` to ensure zero CPU overhead in release builds.

### 2. Bindings & Boilerplate Reduction
* **Foundation:** `UQueryResolver` (for finding elements via cached reflection) and `ElementBunch` (for fluent binding syntax).
* **Complex List Binding:** `ListViewBindingExtensions` binds an `ObservableList<T>` from a ViewModel directly to a UI Toolkit `ListView`, internally handling `makeItem`, `bindItem`, and `unbindItem` with virtualized pooling.
* **Two-Way Binding:** Dedicated extensions for `Toggle`, `Slider`, and `TextField` that listen to `RegisterValueChangedCallback` and update `ReactiveProperty<T>` without cyclic feedback loops.
* **Source Generators (Future):** Implement C# Source Generators to replace reflection-based `UQueryResolver`, providing compile-time type-safety and 100% zero-reflection element querying.

### 3. SMACSS & Dynamic Styling
* **Foundation:** Strict adherence to the 5 SMACSS categories (`1_Base`, `2_Layout`, `3_Modules`, `4_States`, `5_Themes`).
* **Data-Driven State Classes:** Direct binding of `ReactiveProperty<bool>` to USS state classes (`.is-active`, `.is-hidden`, `.is-disabled`).
* **Avoid Dynamic Inline Styles:** No `element.style.color = ...` in C#. All visual styling is defined in USS and toggled via classes.
* **View Recycling:** When cached views are re-shown, ViewModels reset state, automatically updating bound USS classes.

### 4. R3 (Reactive Extensions)
* **Approved Operators:** Use allocation-free or minimal-allocation operators with cached static delegates.
* **Strict Disposable Management:** Every `Subscribe()` call must be registered into `Binder.AddDisposable()`. Zero subscription leaks!

### 5. UniTask (Async Gold Standard)
* **Zero-Allocation Async:** All window open/close and asset loading workflows use struct-based `UniTask`.
* **Mandatory Cancellation Tokens:** Every asynchronous method accepts `CancellationToken ct = default` and calls `ct.ThrowIfCancellationRequested()`.
* **Async Modals:** Stacking dialogs utilize `UniTaskCompletionSource<T>` for awaiting user confirmation without callback spaghetti.

### 6. Unity 6 & UI Toolkit Evolution
* **Benchmarking Official Data Binding:** Evaluate Unity 6 native runtime data-binding against our custom Zero-Alloc solution.
* **Profiler Audits:** Continuous memory profiling with Unity Profile Analyzer to verify zero Garbage Collection spikes.

### 7. VContainer & Scoped Lifetimes
* **ViewModel Lifetime:** All ViewModels are strictly registered with `Lifetime.Transient` to prevent shared state bugs.
* **Service Lifetime:** Core UI managers (`UIFlow`, `UIWindowManager`, `ViewCache`) are `Lifetime.Singleton`.
* **Startup Initialization:** Asynchronous bootstrapping leverages VContainer's `IAsyncStartable`.

---

## 3. Future Milestones & Evolution Backlog (v2.0)

### [SUIF-TASK-1] First-Class DI & Scoped Window Hierarchies
- **Objective:** Eliminate the dual abstraction of `IUIDependencyResolver` in favor of direct compile-time defines (`#if SUIF_VCONTAINER_SUPPORT`).
- **Feature:** Implement automatic child DI container creation (`LifetimeScope.CreateChild()`) for complex full-screen views, allowing isolated sub-systems to be disposed alongside the view.

### [SUIF-TASK-2] Roslyn Source Generators for Zero-Reflection UI Binding
- **Objective:** Completely replace runtime reflection in `UQueryResolver` and `UIViewAttributeCache` with compile-time code generation.
- **Feature:** Generate `partial class` view extensions that bind `root.Q<T>()` directly during compilation and produce static window registries for `ViewFactory`.
