# 🎨 SMACSS Architecture for UI Toolkit

SUIF organizes all stylesheets into 5 distinct, strictly prioritized layers following the **SMACSS** (Scalable and Modular Architecture for CSS) methodology.

---

## 🗂️ The 5 SMACSS Layers

```text
Runtime/Core/Styles/
├── 1_Base/         # Global resets, box-sizing, root typography, CSS variables
├── 2_Layout/       # Structural layouts (grid, containers, screen wrappers)
├── 3_Modules/      # Self-contained reusable UI components (button, card, input)
├── 4_States/       # Interactive and modifier states (.is-active, .is-hidden)
└── 5_Themes/       # Palette swaps, runtime theme overrides, font TSS sheets
```

---

## 📋 Strict Rules & Best Practices

1. **Picking Mode Ignore**: All transparent layout containers and wrappers MUST have `picking-mode="Ignore"` in UXML so they don't block clicks to interactive children.
2. **No Inline Overrides**: Never modify font size, colors, or margins directly in C# code. All visuals are controlled by USS classes.
3. **BEM Naming Convention**:
   * Block: `.c-button`, `.c-window`
   * Element: `.c-window__header`, `.c-window__close-btn`
   * Modifier: `.c-button--primary`, `.c-button--danger`
   * State: `.is-active`, `.is-disabled`, `.is-hidden`
