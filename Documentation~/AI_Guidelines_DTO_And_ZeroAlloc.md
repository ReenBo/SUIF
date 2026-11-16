# AI Agent Specification: SUIF Universal Data & Zero-Realloc UI Standard

> **Scope:** Mandatory generation and code-review constraints for AI coding agents working with `com.reenbo.suif` (Unity 6+ UI Toolkit).

---

## Mandatory Architectural Invariants

1. **Optional Universal Data Contracts (`TData`)**
   * Views MAY bind to any `TData` (`readonly struct`, domain `class`, server payload, or `ScriptableObject`) without implementing any forced SUIF marker interface.
   * Data contracts MUST NOT reference `UnityEngine.UIElements`, pre-formatted UI display strings, RichText tags, or USS class names.

2. **Thin ViewModel & Thin View Separation**
   * **Domain Services / `ScriptableObject`s**: Own all calculations, counters, sorting/filtering, timestamps, data arrays, and async `UniTask.WhenAll` preloading.
   * **ViewModel (`BaseViewModel`)**: Thin reactive adapter only—exposes `Observable<TData>` / `ReadOnlyReactiveProperty<TData>` (implementing `IReactiveDataViewModel<TData>` for single-payload Views) and forwards commands to Domain Services. MUST contain zero loops, arithmetic, `static` data arrays, or UI strings.
   * **Formatter Service (`Singleton`)**: Injectable DI instance (`ITextFormatter`) that pre-caches `FixedTextWriter<T>` delegates once in its constructor at startup. Inject into the `ViewModel` and expose read-only (`ViewModel.Formatter`), never directly via `[Inject]` on Views. Avoid `static` formatter classes so presets/locales can swap dynamically without global state.
   * **View (`BaseReactiveDataView<TViewModel, TData>` or `BaseView<TViewModel>`)**: Container-agnostic UI binding layer ONLY—declares `[UQuery]` fields, registers slots via `Binder.FixedSlot(...)` & `FixedElementPool<TElement>`, and binds purely against `ViewModel` (`using VContainer;` / `[Inject]` inside Views is strictly forbidden).

3. **Zero-Realloc Text Rendering (`ViewBinder.FixedSlot` / `FixedSlotLabel`)**
   * Register slots via `Binder.FixedSlot(element, capacity)` instead of writing local helper methods per View.
   * Dynamic `Label`, `Button`, or `TextElement` instances MUST NOT be updated directly via `.text = ...` when text length or content changes.
   * Always avoid per-update heap closures (`static` lambdas or startup-cached `FixedTextWriter<T>` delegates + explicit `TState`).
   * Keep `FixedTextSlot`'s `AttachToPanelEvent` deferral intact—never force `TextElement.SetText(char[], ...)` while detached (`panel == null`).

4. **Fixed DOM Pooling & Async Preloading**
   * Repeated list/grid/log containers MUST pre-allocate `FixedElementPool<TElement>` once in `OnBind()` / `Bind()` and toggle visibility via `DisplayStyle.None` (never `Visibility.Hidden`).
   * Heavy startup/dataset loads should implement parallel `UniTask.WhenAll` preloading paired with a cached `UILayer.Topmost` spinner View using fixed-width `FixedTextSlot` frames.

---

## Pre-Commit Agent Verification Checklist

- [ ] Zero `$"..."` or `string.Format` in any `*ViewModel.cs` or `*View.cs`.
- [ ] Zero business calculations, counters, timestamps, or `static` data arrays in any `*ViewModel.cs`.
- [ ] Zero forced marker interfaces (`IViewDto`) or `struct` restrictions blocking `ScriptableObject` / class payloads.
- [ ] Single-payload Views use `BaseReactiveDataView<TViewModel, TData>` + `IReactiveDataViewModel<TData>`; slots use `Binder.FixedSlot(...)`; Views contain ZERO `[Inject]` attributes or `using VContainer;`.
- [ ] Zero `//` or `/* */` comments in C# source files (per project style).
- [ ] Script recompilation (`compilationFailed: false`), EditMode tests, and Play Mode (`US.dc` settled, `0` console warnings/errors, `game_view`) pass.

---

## Reference Documentation

* Human Guide: [DTO_Standard_And_ZeroAlloc_UI.md](DTO_Standard_And_ZeroAlloc_UI.md)
* Performance Rules: [Performance_And_Optimization.md](Performance_And_Optimization.md)
* Architecture Manual: [Architecture.md](Architecture.md)
