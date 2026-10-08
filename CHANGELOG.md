# Changelog

All notable changes to the `SUIF` package will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.1] - 2026-10-08

### Added
- **UniTask Gold Standard Integration**:
  - Full propagation of `CancellationToken ct = default` across all asynchronous APIs (`IUIFlow`, `IUIAssetProvider`, `IViewFactory`, `IUIThemeService`, `IUIWindowManager`).
  - Added dedicated architectural guide: `Documentation~/UniTask_Gold_Standard.md` covering value-type structs, cancellation tokens, `UniTaskCompletionSource` modals, and `IAsyncStartable`.
- **Knowledge Base Documentation Integration**:
  - Added `Documentation~/ROADMAP.md` covering the 7 core pillars and long-term milestones.
  - Enhanced `Documentation~/CSS_Architecture_SMACSS.md` with layout gotchas (`TemplateContainer` 0x0 collapse fix, UXML `picking-mode="Ignore"` rule).
  - Enhanced `Documentation~/Performance_And_Optimization.md` with the 8 mandatory Zero-Allocation rules (Enum strings, O(1) dictionary lookups, avoiding idle UI Toolkit calls, LINQ prohibition).

## [1.0.0] - 2026-10-08

### Added
- **Core Architecture (`SUIF.Core`)**:
  - Declarative MVVM framework with `BaseView<TViewModel>` and `BaseViewModel`.
  - Scoped window management with `UIWindowManager` supporting stacking, focus elevation, and modal blocking.
  - Inverted dependency contracts: `IUIDependencyResolver` and `IUIAssetProvider`.
  - Fallback zero-dependency implementations: `DefaultDependencyResolver` and `DirectAssetProvider`.
  - Cached reflection element resolver (`UQueryResolver`).
  - 5-layer SMACSS styling system with baseline styles and typography.
- **VContainer Integration (`SUIF.VContainer`)**:
  - First-class VContainer support with `VContainerUIDependencyResolver` and `builder.RegisterSUIF()` extensions.
  - Auto-activated conditionally via Unity `versionDefines` for `jp.hadashikick.vcontainer`.
- **Addressables Integration (`SUIF.Addressables`)**:
  - High-performance asset streaming via `AddressablesUIAssetProvider`.
  - Auto-activated conditionally via Unity `versionDefines` for `com.unity.addressables`.
- **R3 Reactive Extensions (`SUIF.R3`)**:
  - Zero-allocation data binding for Buttons, Labels, Toggles, and virtualized `ListView`.
- **Editor Tooling (`SUIF.Editor`)**:
  - 1-click window generator wizard (`Tools > SUIF > Create UI Window Wizard...`).
- **Comprehensive Documentation Suite**:
  - Full manuals covering Architecture, Performance, SMACSS, Trade-offs, and Getting Started.
- **UPM Samples**:
  - Samples for Basic Window, Data Binding, Virtualized Lists, Dynamic Theming, and Modals.
