# 🎨 SMACSS Architecture for UI Toolkit (Comprehensive Guide)

> This document describes the storage structure, naming conventions, and styling application mechanisms for UI Toolkit within SUIF. The architecture is based on the **SMACSS** methodology (Base, Layout, Module, State, Theme), adapted specifically for Unity 6+.

---

## 🗂️ 1. Folder & File Hierarchy (SMACSS)

All framework styles are strictly partitioned into 5 categories:

```text
Runtime/Core/Styles/
├── 1_Base/
│   ├── _variables.uss         (Tokens: base colors, fonts, margins in :root)
│   └── _resets.uss            (Style resets, base text settings for the entire UI)
├── 2_Layout/
│   ├── _containers.uss        (Main wrappers: .l-screen, .l-popup-container, .l-layer)
│   └── _grid.uss              (Structural layout for lists and grids)
├── 3_Modules/
│   ├── _button.uss            (Independent components: .fw-btn, .c-button)
│   ├── _card.uss              (Inventory cards, items: .fw-card, .c-card)
│   └── _input.uss             (Text fields, sliders: .fw-input, .c-input)
├── 4_States/
│   └── _states.uss            (Global states: .is-active, .is-hidden, .is-disabled)
└── 5_Themes/
    ├── Theme-Default.uss      (Main default theme assembly file)
    ├── Theme-IOS.uss          (Alternative iOS theme file)
    └── TSS.tss                (Theme style sheet linking fonts and base variables)
```
*(The `_` prefix denotes a partial fragment imported via `@import` into master theme files).*

---

## 🏷️ 2. Category Descriptions and Naming Conventions

To avoid collisions with project-level user code, a strict prefix convention is enforced:

### 1. Base (`1_Base/`)
The design foundation. Contains `:root` definitions, CSS variables (tokens), and base fonts.
* **Variable Prefix:** `--fw-` (e.g., `--fw-color-primary`, `--fw-radius-base`, `--fw-space-md`).

### 2. Layout (`2_Layout/`)
Structural containers forming the screen skeleton (layers, headers, modal wrappers, grids).
* **Class Prefix:** `.l-` (layout).
* *Examples:* `.l-header`, `.l-window-wrapper`, `.l-layer`, `.l-screen`.

### 3. Module (`3_Modules/`)
Reusable, isolated UI components (buttons, cards, badges, text inputs).
* **Class Prefix:** `.fw-` or `.c-` (component).
* *Examples:* `.fw-btn`, `.c-button`, `.fw-card`, `.c-window`.

### 4. State (`4_States/`)
Classes overriding or modifying styles during dynamic runtime state changes. Managed via C# bindings.
* **Class Prefix:** `.is-` or `.has-`.
* *Examples:* `.is-active`, `.is-hidden`, `.is-disabled`, `.has-error`.

### 5. Theme (`5_Themes/`)
Master sheets overriding color palettes and fonts to change visual appearance across the entire game.
* **Class Prefix:** `.theme-`.
* *Examples:* `.theme-default`, `.theme-dark`, `.theme-ios`.

---

## ⛔ 3. Separation of Responsibilities

### ❌ FORBIDDEN in Modules (`3_Modules/`):
1. **Hardcoding Width/Height:** Except for icons or fixed checkboxes, components must remain flexible and let parent layout containers control sizing.
2. **Setting Outer Margins (`margin`):** Outer spacing is the exclusive responsibility of Layout containers.
3. **Hardcoding Raw Hex Colors:** `background-color: #ffffff;` is forbidden. Always use CSS variables: `var(--fw-color-bg);`.

### ✅ ALLOWED in Modules (`3_Modules/`):
* `background-color`, `border-color`, `color` (via CSS variables).
* `border-radius`, `border-width`.
* Inner spacing (`padding`) to maintain internal component ergonomics.
* `transition-duration`, `transition-property`.

---

## 🎭 4. Theme Assembly: The Hybrid Approach

SUIF employs a **Hybrid Approach** for theme application:
1. **Global Default Theme:** The master `Theme-Default.uss` (or `TSS.tss`) is assigned in Unity's **Panel Settings -> Theme Style Sheet** inspector. This ensures UI Builder automatically previews all framework classes and CSS variables without manual linking in every `.uxml`.
2. **Runtime Theme Switching:** Alternative themes (e.g., `Theme-IOS.uss`) are applied at runtime by adding the class (`.theme-ios`) to the `UIRoot` root visual element. CSS variables cascade instantly across all open windows without scene reloading!

---

## ⚠️ 5. Critical UI Toolkit Layout & Event Gotchas

### Gotcha 1: `TemplateContainer` Sizing (The 0x0 Collapse)
When instantiating a `.uxml` template via C# (`visualTreeAsset.Instantiate()`), Unity wraps content in a `TemplateContainer`. By default, this generated element **does not stretch**. If your inner element uses `position: absolute;` or expects to fill the layer, it will collapse to `0x0`.
* **Mandatory Fix:** Ensure all instantiated `TemplateContainer`s fill their parent layer via direct child CSS selector:
```css
/* _containers.uss */
.l-layer > * {
    flex-grow: 1; /* Forces TemplateContainer to expand to 100% of the layer */
}
```

### Gotcha 2: Picking Mode and Invisible Walls (Click Blocking)
By default, full-screen transparent containers catch pointer events (`picking-mode: position`). If you have transparent wrapper layers (`Layer-Topmost`, `Layer-Popups`) sitting on top of your game UI, they will block all clicks from reaching buttons underneath!
* **Strict Rule:** `picking-mode` is **NOT** a USS property! Adding `picking-mode: ignore;` in a `.uss` file triggers Unity compiler warnings.
* **Mandatory Fix:** Set `picking-mode="Ignore"` directly in UXML:
```xml
<!-- UIRootUXMLContainer.uxml -->
<ui:VisualElement name="Layer-Topmost" class="l-layer" picking-mode="Ignore" />
```
*(Note: Children inside an ignored wrapper remain fully clickable).*
