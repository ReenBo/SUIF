using System;
using Cysharp.Threading.Tasks;
using SUIF.API;
using SUIF.Attributes;
using UnityEngine.UIElements;

namespace SUIF.ViewSystems
{
    public class ViewFactory : IViewFactory
    {
        private readonly IUIDependencyResolver _resolver;
        private readonly IUIRoot _uiRoot;
        private readonly IUIAssetProvider _assetProvider;

        public ViewFactory(IUIDependencyResolver resolver, IUIRoot uiRoot, IUIAssetProvider assetProvider)
        {
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            _uiRoot = uiRoot ?? throw new ArgumentNullException(nameof(uiRoot));
            _assetProvider = assetProvider ?? throw new ArgumentNullException(nameof(assetProvider));
        }

        public async UniTask<TView> CreateAsync<TView>() where TView : class, IView
        {
            var type = typeof(TView);
            var attr = UIViewAttributeCache<TView>.Attribute;

            if (attr is null)
            {
                throw new InvalidOperationException($"View {type.Name} must have UIView attribute.");
            }

            var visualTreeAsset = await _assetProvider.LoadAssetAsync<VisualTreeAsset>(attr.ViewKey);
            if (visualTreeAsset is null)
            {
                throw new InvalidOperationException($"Failed to load VisualTreeAsset for key '{attr.ViewKey}'.");
            }

            var layerContainer = _uiRoot.Container.Q(attr.Layer.ToContainerName());
            if (layerContainer is null)
            {
                throw new InvalidOperationException($"Failed to find layer '{attr.Layer}' in UIRoot.");
            }

            var template = visualTreeAsset.Instantiate();
            var viewElement = template.childCount > 0 ? template[0] : template;
            viewElement.name = type.Name;

            layerContainer.Add(viewElement);

            var view = ViewTypeCache<TView>.CreateInstance(viewElement);
            var viewModelType = ViewTypeCache<TView>.ViewModelType;
            var viewModel = (IViewModel)_resolver.Resolve(viewModelType);

            view.Initialize(viewModel);
            return view;
        }

        public void ReleaseAsset(string key)
        {
            _assetProvider.ReleaseAsset(key);
        }
    }
}
