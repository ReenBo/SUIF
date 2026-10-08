# ⚡ SUIF Performance & Zero-Allocation Rules (Strict Standard)

> This document contains architectural performance standards that are **mandatory** when developing with SUIF. The primary goal is achieving strict **Zero-Allocation** in hot runtime code paths and eliminating Garbage Collection (GC) spikes during gameplay.

---

## 📜 8 Mandatory Zero-Allocation Rules

### 1. Strings and Enums (Zero-Allocation Strings)
* **Rule:** Strictly forbid `.ToString()` or string interpolation (`$"..."`) on `Enum` values in runtime code.
* **Why:** Enums cause boxing, reflection, and new heap allocations for every call.
* **How to do it:** Use `switch` expressions returning constant string literals. The compiler stores these in the String Intern Pool with 0 bytes allocated:
```csharp
// ❌ BAD (Heap Allocation):
string name = $"Layer-{layer}";

// ✅ GOOD (Zero Allocation):
string name = layer switch {
    UILayer.Windows => "Layer-Windows",
    UILayer.Popups => "Layer-Popups",
    _ => "Default"
};
```

### 2. Dictionaries Over Loops (O(1) State Tracking)
* **Rule:** Forbid loops (especially `foreach`) to search for active elements during frequent user events (e.g., clicks).
* **Why:** Loop traversal is O(N). Interrogating and modifying styles inside loops triggers redundant layout recalculations in UI Toolkit.
* **How to do it:** Use `Dictionary` caches for O(1) state lookups. Update CSS classes only on the old and new elements.

### 3. Avoiding "Idle" UI Toolkit API Calls
* **Rule:** Do not call layout-modifying methods (`BringToFront()`) or class methods (`RemoveFromClassList()`) without first verifying if the element actually needs updating.
* **Why:** Redundant calls mark elements as dirty, forcing UI Toolkit to recalculate the visual hierarchy.
* **How to do it:** Add state checks before execution:
```csharp
// Only bring to front if not already topmost:
if (parent[parent.childCount - 1] != targetElement) {
    targetElement.BringToFront();
}

// Only remove class if it exists:
if (element.ClassListContains(ActiveClass)) {
    element.RemoveFromClassList(ActiveClass);
}
```

### 4. No Over-Engineering for Raw Visuals
* **Rule:** Purely visual elements containing no logic or data (e.g., `ModalBlocker` - a semi-transparent black overlay) must never be routed through heavy `ViewFactory` reflection pipelines.
* **Why:** Creating `BaseView<T>` and empty ViewModels via reflection just to show a black rectangle wastes CPU cycles and heap memory.
* **How to do it:** Request raw `VisualTreeAsset` directly from `IUIAssetProvider` and add to hierarchy.

### 5. No Allocations for Debugger Aesthetics
* **Rule:** Strictly forbid dynamic string concatenation to set the `name` property of a `VisualElement` solely to make it look nice in the UI Builder Debugger.
* **Why:** Formatting names like `blocker.name = $"Blocker-{type}"` creates garbage on every window open.
* **How to do it:** Keep default names defined in UXML, or use static string constants. Memory savings always take priority over debugger vanity.

### 6. Strict Restriction on Static State
* **Rule:** Strictly forbid using `static` fields to store mutable runtime state (active windows, visual elements, event listeners). `static` is permitted **ONLY** for stateless caching (e.g., compile-time attributes) and extension methods.
* **Why:** Static state causes memory leaks, breaks Unity Fast Play Mode (Domain Reload issues), makes unit testing impossible, and prevents multi-screen/split-screen setups.
* **How to do it:** Use Dependency Injection (VContainer). Register managers as `Lifetime.Singleton` in a scoped container.
```csharp
// ❌ BAD (Memory Leaks, Global State):
public static class WindowManager { public static List<IView> Active; }

// ✅ GOOD (Stateless compile-time generic cache):
public static class UIViewAttributeCache<TView> {
    public static readonly UIViewAttribute Attribute = typeof(TView).GetCustomAttribute<UIViewAttribute>();
}
```

### 7. No "Magic Strings"
* **Rule:** Forbid hardcoded string literals duplicated across the codebase.
* **Why:** Magic strings cause silent typo bugs and impede refactoring.
* **How to do it:** Extract literals into `private const string` fields or a centralized `UIConstants` class:
```csharp
private const string ActiveClass = "is-active";
private const string InactiveClass = "is-inactive";
```

### 8. Strict Prohibition of LINQ (Zero-Allocation Queries)
* **Rule:** Strictly forbid `System.Linq` methods (`.First()`, `.FirstOrDefault()`, `.Where()`, `.Select()`) in runtime code.
* **Why:** LINQ allocates `IEnumerator` instances and heap closure classes on every execution, triggering GC spikes.
* **How to do it:** Use standard indexed `for` loops or `foreach` over concrete collections with manual `break`:
```csharp
// ❌ BAD (Heap Allocations: IEnumerator, Closure):
var active = windowsList.FirstOrDefault(v => v.VisualElement == target);

// ✅ GOOD (Zero Allocation):
IView active = null;
for (var i = 0; i < windowsList.Count; i++) {
    if (windowsList[i].VisualElement == target) {
        active = windowsList[i];
        break;
    }
}
```
