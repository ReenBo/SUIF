# AI Agent Specification: SUIF DTO & Zero-Realloc UI Standard

> **Scope:** Mandatory generation and code-review constraints for AI coding agents working with `com.reenbo.suif` (Unity 6+ UI Toolkit).

---

## Mandatory Architectural Invariants

1. **DTO Contract Per View (`IViewDto`)**
   * Every parameterized View MUST consume a strongly-typed `readonly struct <Name>Dto : IViewDto` (namespace `SUIF.API`).
   * DTO structs MUST store only domain primitives, IDs, counts, booleans, and domain `enum` values (`byte`/`ushort`).
   * DTO structs MUST NOT reference `UnityEngine.UIElements`, localized text, formatted strings (`$"..."`), RichText tags, or USS class names.

2. **Strict Separation of Responsibilities**
   * **ViewModel (`BaseViewModel`)**: Mutates domain state and exposes `ReactiveProperty<TDto>` (or immutable arrays of item DTOs). MUST NEVER construct UI display strings, format numbers, or emit CSS tokens.
   * **Text Formatter (`static class *TextFormatter`)**: Contains pure `FixedTextWriter<TState>` delegates (`ref Utf16ValueStringBuilder builder, TState state`) and zero-allocation `switch` expressions returning constant strings for domain enums.
   * **View (`BaseDtoView<TViewModel, TDto>` or `BaseView<TViewModel>`)**: Binds via `Binder.ToDto(...)`, toggles CSS modifier classes, updates `FixedTextSlot` / `FixedSlotLabel`, and recycles DOM items via `FixedElementPool<TElement>`.

3. **Zero-Realloc Text Rendering (`FixedTextSlot` / `FixedSlotLabel`)**
   * Dynamic `Label`, `Button`, or `TextElement` instances MUST NOT be updated directly via `.text = ...` after initialization when their content can change length or characters.
   * Wrap dynamic UXML elements with `new FixedTextSlot(element, capacity)` (disposed via `AddDisposable(slot)`), use `<suif:FixedSlotLabel capacity="N" />` / `new FixedSlotLabel { Capacity = N }`, or bind via `ElementBunch<TElement>.ToFixedText(...)`.
   * Always pass a static `FixedTextWriter<TState>` or `static` lambda to `SetWritten<TState>` and `Binder.ToDto<TDto, TState>`—never allocate per-update closures.
   * Set `capacity` >= maximum visible rendered glyph count (RichText tags `<...>` and `\r`/`\n` excluded automatically by `FixedTextSlot`).

4. **Fixed DOM Pooling (`FixedElementPool<TElement>`)**
   * Repeated list/grid/log containers with bounded visible slots MUST allocate a `FixedElementPool<TElement>` once in `Bind()` and refresh via `pool.Render(activeCount, state, staticCallback)`.
   * Calling `container.Clear()` + instantiating `new VisualElement()` / `new Button()` on reactive updates is prohibited outside initial `Bind()`.
   * Keep `DisplayStyle.None` for inactive pooled elements and cached views (`DefaultSetActive`); do NOT switch `BaseView.Hide()` to `Visibility.Hidden`.

---

## Pre-Commit Agent Verification Checklist

Before marking any SUIF UI task complete, the AI agent MUST verify:

- [ ] No `$"..."` or `string.Format` in any `*ViewModel.cs` or `*View.cs`.
- [ ] Every `*Dto` struct implements `SUIF.API.IViewDto` and contains zero UI strings or `UIElements` types.
- [ ] Every `Binder.ToDto`, `FixedTextSlot.SetWritten`, and `FixedElementPool.Render` call avoids closure capture (`static` delegate + explicit `TState`).
- [ ] Zero `//` or `/* */` comments in C# source files (when project style enforces comment-free code).
- [ ] Script recompilation passes (`compilationFailed: false`) and EditMode DTO/Formatter unit tests pass.
- [ ] Play Mode verification confirms expected Draw Calls (`US.dc` settled, zero visual clipping via `game_view`).

---

## Reference Documentation

* Human Guide: [DTO_Standard_And_ZeroAlloc_UI.md](DTO_Standard_And_ZeroAlloc_UI.md)
* Performance Rules: [Performance_And_Optimization.md](Performance_And_Optimization.md)
* Architecture Manual: [Architecture.md](Architecture.md)
