# ⚡ UniTask: The SUIF Gold Standard for Asynchronous UI

> **UniTask** is the foundational asynchronous engine of SUIF. Unlike standard C# `Task` / `Task<T>`, which allocate state machines and heap objects on every `await`, UniTask is a struct-based value-type implementation specifically optimized for Unity and the Unity PlayerLoop.

---

## 🏆 Core Principles of UniTask in SUIF

### 1. Value-Type Structs (`UniTask` and `UniTask<T>`)
Every asynchronous operation in SUIF (`IUIFlow.OpenViewAsync`, `IUIAssetProvider.LoadAssetAsync`, `IUIThemeService.SetTypographyThemeAsync`) returns `UniTask` or `UniTask<T>`:
```csharp
// Zero-allocation async execution:
public async UniTask<TView> OpenViewAsync<TView>(CancellationToken ct = default) where TView : class, IView
{
    ct.ThrowIfCancellationRequested();
    var view = await _viewFactory.CreateAsync<TView>(ct);
    view.Show();
    await _windowManager.OnViewOpenedAsync(viewData, ct);
    return view;
}
```

### 2. Mandatory `CancellationToken` Support
In UI development, a user might close a window, change a scene, or navigate away while an asset is still downloading or instantiating. Without cancellation tokens, async operations continue running in the background, causing `MissingReferenceException` or zombie windows.
* **Rule:** All asynchronous methods in SUIF MUST accept a `CancellationToken ct = default`.
* **Rule:** At the start and after every `await` in critical loops, call `ct.ThrowIfCancellationRequested()`.
* **Usage:** Pass `this.GetCancellationTokenOnDestroy()` from MonoBehaviours or link tokens via `CancellationTokenSource.CreateLinkedTokenSource(...)`.

### 3. Modal Dialogs with `UniTaskCompletionSource<T>`
Instead of messy callbacks (`Action<bool> onConfirmed`), SUIF implements modal dialogs that can be awaited as clean synchronous-looking code:
```csharp
public class ConfirmDialogViewModel : BaseViewModel
{
    private UniTaskCompletionSource<bool> _tcs;

    public UniTask<bool> WaitForDecisionAsync(CancellationToken ct = default)
    {
        _tcs = new UniTaskCompletionSource<bool>();
        ct.Register(() => _tcs.TrySetCanceled(ct));
        return _tcs.Task;
    }

    public void Confirm() => _tcs.TrySetResult(true);
    public void Cancel() => _tcs.TrySetResult(false);
}

// In your gameplay code:
var dialog = await uiFlow.OpenViewAsync<ConfirmDialogView>(ct);
bool isConfirmed = await dialog.ViewModel.WaitForDecisionAsync(ct);
if (isConfirmed)
{
    await inventoryService.SellItemAsync(itemId, ct);
}
await uiFlow.CloseViewAsync<ConfirmDialogView>(ct);
```

### 4. VContainer Integration with `IAsyncStartable`
For asynchronous service bootstrapping (e.g., preloading root themes or fonts), SUIF utilizes VContainer's `IAsyncStartable`:
```csharp
public class UIInitializer : IAsyncStartable
{
    private readonly IUIThemeService _themeService;

    public UIInitializer(IUIThemeService themeService) => _themeService = themeService;

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        await _themeService.SetTypographyThemeAsync("Theme-Default", cancellation);
    }
}
```

### 5. Safe Fire-and-Forget with `.Forget()`
When an async operation is triggered from a synchronous button click or event handler where `await` cannot be used directly, never discard the UniTask. Always call `.Forget()`:
```csharp
button.clicked += () => OpenShop().Forget();

private async UniTaskVoid OpenShop()
{
    await _uiFlow.OpenViewAsync<ShopView>();
}
```
