# 🚀 Getting Started with SUIF

---

## 📦 1. Installation

### Option A: Install via Git URL (Unity Package Manager)
1. Open Unity Editor -> **Window > Package Manager**.
2. Click **+** -> **Add package from git URL...**.
3. Enter:
   ```text
   https://github.com/ReenBo/SUIF.git
   ```

### Option B: Git Submodule
In your Unity project root:
```bash
git submodule add https://github.com/ReenBo/SUIF.git Packages/com.reenbo.suif
```

---

## 🛠️ 2. Quick Setup with VContainer

1. In your `LifetimeScope`:
   ```csharp
   using SUIF.VContainer;

   public class GameLifetimeScope : LifetimeScope
   {
       [SerializeField] private UIRoot _uiRootPrefab;

       protected override void Configure(IContainerBuilder builder)
       {
           // Registers all SUIF core systems in one line!
           builder.RegisterSUIF();
           
           // Register your ViewModels
           builder.Register<MyWindowViewModel>(Lifetime.Transient);
       }
   }
   ```

2. Open window from any system or service:
   ```csharp
   public class GameController
   {
       private readonly IUIFlow _uiFlow;

       public GameController(IUIFlow uiFlow) => _uiFlow = uiFlow;

       public async UniTask ShowMainMenu()
       {
           var view = await _uiFlow.OpenViewAsync<MyWindowView>();
       }
   }
   ```

---

## 🧙‍♂️ 3. Window Creation Wizard
In Unity Editor, click **Tools > SUIF > Create UI Window Wizard...** to automatically generate your View, ViewModel, UXML, and USS template files.
