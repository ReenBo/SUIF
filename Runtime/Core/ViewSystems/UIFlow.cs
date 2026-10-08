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
        private readonly ViewCache _viewCache;
        private readonly IUIWindowManager _windowManager;

        public UIFlow(IViewFactory viewFactory, ViewCache viewCache, IUIWindowManager windowManager)
        {
            _viewFactory = viewFactory;
            _viewCache = viewCache;
            _windowManager = windowManager;
        }

        public async UniTask<TView> OpenViewAsync<TView>(CancellationToken ct = default) where TView : class, IView
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

        public async UniTask CloseViewAsync<TView>(CancellationToken ct = default) where TView : class, IView
        {
            var type = typeof(TView);

            if (!_viewCache.TryGetView(type, out var viewData))
            {
                return;
            }

            viewData.View.Hide();
            _windowManager.OnViewClosed(viewData);

            if (viewData.LoadingMode == UILoadingMode.Destroy)
            {
                _viewCache.Unregister(type);
                _windowManager.OnViewDestroyed(viewData);

                viewData.View.VisualElement.RemoveFromHierarchy();
                viewData.View.Dispose();

                if (!string.IsNullOrEmpty(viewData.ViewKey))
                {
                    _viewFactory.ReleaseAsset(viewData.ViewKey);
                }
            }

            await UniTask.Yield(ct);
        }
    }
}
