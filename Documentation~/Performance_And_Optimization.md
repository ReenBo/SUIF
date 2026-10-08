# ⚡ SUIF Performance & Optimization Guide

> SUIF is designed from the ground up for **zero heap allocations** in runtime updates and high-frequency loops.

---

## 🚀 Key Performance Techniques

### 1. Zero-Allocation String Formatting (`ZString`)
Traditional string formatting (`$"HP: {hp}/{maxHp}"` or `val.ToString()`) generates heap garbage every single frame. SUIF integrates `ZString` in its reactive binding extensions:
```csharp
bunch.Element.BindText(viewModel.Health, (val, label) => label.text = ZString.Concat("HP: ", val));
```

### 2. Cached Reflection for Element Queries (`UQueryResolver`)
Looking up visual elements via `root.Q("element-name")` every frame or using raw reflection on instantiation is slow. SUIF pre-compiles and caches field metadata per Type on first access (`UQueryResolver.Cache`), ensuring instant initialization with zero subsequent reflection allocations.

### 3. Virtualized List Recycling with Clean Disposals
When scrolling large inventories or leaderboards, standard UI systems instantiate hundreds of gameobjects or allocate lambdas. SUIF's `ListViewBindingExtensions` uses UI Toolkit's native virtualized element pooling:
* **`makeItem`**: Instantiates only visible elements (~10-15 items).
* **`bindItem`**: Re-initializes the cached `IView` with new model data.
* **`unbindItem`**: Calls `view.Unbind()` without destroying the ViewModel or pooling container.

### 4. Class-Based Visibility Over Inline Styles
Setting `element.style.display = DisplayStyle.None` invalidates inline style structs in UI Toolkit. SUIF enforces the **`.is-hidden`** USS class:
```csharp
// Fast, batched style re-computation:
VisualElement.AddToClassList("is-hidden");
```
