using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SUIF.API;
using SUIF.Attributes;

namespace SUIF.ViewSystems
{
    public class UIFlow : IUIFlow
    {
        private readonly IViewFactory _viewFactory;
        private readonly IViewCache _viewCache;
        private readonly IUIWindowManager _windowManager;

        public UIFlow(IViewFactory viewFactory, IViewCache viewCache, IUIWindowManager windowManager)
        {
            _viewFactory = viewFactory;
            _viewCache = viewCache;
            _windowManager = windowManager;
        }

        public UniTask<TView> OpenViewAsync<TView>(CancellationToken ct = default) where TView : class, IView => OpenAsync<TView>(ct);

        public async UniTask<TView> OpenAsync<TView>(CancellationToken ct = default) where TView : class, IView
        {
            ct.ThrowIfCancellationRequested();
            var type = typeof(TView);

            if (_viewCache.TryGetView(type, out var viewData))
            {
                viewData.View.Show();
                await _windowManager.OnViewOpenedAsync(viewData, ct);
                return (TView)viewData.View;
            }

            var view = await _viewFactory.CreateAsync<TView>(ct);
            ct.ThrowIfCancellationRequested();

            var attr = UIViewAttributeCache<TView>.Attribute;

            viewData = new ViewData
            {
                ViewType = type,
                View = view,
                LoadingMode = attr?.LoadingMode ?? UILoadingMode.Destroy,
                Layer = attr?.Layer ?? UILayer.Default,
                IsModal = attr?.IsModal ?? false,
                ViewKey = attr?.ViewKey
            };

            _viewCache.Register(type, viewData);

            view.Show();
            await _windowManager.OnViewOpenedAsync(viewData, ct);

            return view;
        }

        public void Close<TView>() where TView : class, IView
        {
            var type = typeof(TView);
            if (_viewCache.TryGetView(type, out var viewData))
            {
                CloseOrDestroy(viewData);
            }
        }

        public async UniTask CloseViewAsync<TView>(CancellationToken ct = default) where TView : class, IView
        {
            Close<TView>();
            await UniTask.Yield(ct);
        }

        public void CloseAll()
        {
            var views = _viewCache.GetAllViews();
            for (var i = views.Count - 1; i >= 0; i--)
            {
                CloseOrDestroy(views[i]);
            }
        }

        private void CloseOrDestroy(ViewData viewData)
        {
            if (viewData.LoadingMode == UILoadingMode.Cached)
            {
                viewData.View.Hide();
                _windowManager.OnViewClosed(viewData);
            }
            else
            {
                DestroyView(viewData);
            }
        }

        private void DestroyView(ViewData viewData)
        {
            _windowManager.OnViewDestroyed(viewData);
            viewData.View.Dispose();
            viewData.View.VisualElement?.RemoveFromHierarchy();

            _viewCache.Unregister(viewData.ViewType);

            if (!string.IsNullOrEmpty(viewData.ViewKey))
            {
                _viewFactory.ReleaseAsset(viewData.ViewKey);
            }
        }
    }
}
