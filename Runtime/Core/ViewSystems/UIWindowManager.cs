using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using SUIF.API;
using UnityEngine.UIElements;

namespace SUIF.ViewSystems
{
    public class UIWindowManager : IUIWindowManager
    {
        private const string WindowClass = "c-window";
        private const string ActiveClass = "is-active";
        private const string InactiveClass = "is-inactive";
        private const string HiddenClass = "is-hidden";

        private readonly IUIRoot _uiRoot;
        private readonly IUIAssetProvider _assetProvider;

        private readonly Dictionary<UILayer, List<IView>> _windowsByLayer = new();
        private readonly Dictionary<VisualElement, IView> _viewsByElement = new();
        private readonly Dictionary<IView, UILayer> _viewLayers = new();
        private readonly Dictionary<UILayer, IView> _activeWindowsByLayer = new();
        private readonly Dictionary<IView, VisualElement> _modalBlockers = new();

        public UIWindowManager(IUIRoot uiRoot, IUIAssetProvider assetProvider)
        {
            _uiRoot = uiRoot;
            _assetProvider = assetProvider;
        }

        public async UniTask OnViewOpenedAsync(ViewData viewData, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if (viewData.IsModal)
            {
                await CreateModalBlockerAsync(viewData, ct);
            }

            if (viewData.Layer is UILayer.Windows or UILayer.Popups)
            {
                RegisterWindowFocus(viewData);
                FocusWindow(viewData);
            }
        }

        public void OnViewClosed(ViewData viewData)
        {
            if (viewData.IsModal)
            {
                RemoveModalBlocker(viewData);
            }

            if (viewData.Layer is UILayer.Windows or UILayer.Popups)
            {
                RemoveWindowFocus(viewData);
            }
        }

        public void OnViewDestroyed(ViewData viewData)
        {
            if (viewData.IsModal)
            {
                DestroyModalBlocker(viewData);
            }

            if (viewData.Layer is UILayer.Windows or UILayer.Popups)
            {
                UnregisterWindowFocus(viewData);
            }
        }

        private async UniTask CreateModalBlockerAsync(ViewData viewData, CancellationToken ct = default)
        {
            if (_modalBlockers.ContainsKey(viewData.View))
            {
                var existingBlocker = _modalBlockers[viewData.View];
                existingBlocker.RemoveFromClassList(HiddenClass);
                existingBlocker.BringToFront();
                viewData.View.VisualElement.BringToFront();
                return;
            }

            var blockerAsset = await _assetProvider.LoadAssetAsync<VisualTreeAsset>("ModalBlocker", ct);
            if (blockerAsset == null) return;

            var blocker = blockerAsset.Instantiate();
            var targetLayer = _uiRoot.Container.Q(viewData.Layer.ToContainerName());
            if (targetLayer == null) return;

            targetLayer.Add(blocker);
            blocker.BringToFront();
            viewData.View.VisualElement.BringToFront();

            _modalBlockers[viewData.View] = blocker;
        }

        private void RemoveModalBlocker(ViewData viewData)
        {
            if (_modalBlockers.TryGetValue(viewData.View, out var blocker))
            {
                blocker.AddToClassList(HiddenClass);
            }
        }

        private void DestroyModalBlocker(ViewData viewData)
        {
            if (_modalBlockers.Remove(viewData.View, out var blocker))
            {
                blocker.RemoveFromHierarchy();
                _assetProvider.ReleaseAsset("ModalBlocker");
            }
        }

        private void RegisterWindowFocus(ViewData viewData)
        {
            if (!_windowsByLayer.TryGetValue(viewData.Layer, out var layerWindows))
            {
                layerWindows = new List<IView>();
                _windowsByLayer[viewData.Layer] = layerWindows;
            }

            if (!layerWindows.Contains(viewData.View))
            {
                layerWindows.Add(viewData.View);
            }

            _viewLayers[viewData.View] = viewData.Layer;
            var visualElement = viewData.View.VisualElement;
            if (visualElement == null) return;

            _viewsByElement[visualElement] = viewData.View;
            visualElement.RegisterCallback<PointerDownEvent>(OnWindowPointerDown, TrickleDown.TrickleDown);
        }

        private void UnregisterWindowFocus(ViewData viewData)
        {
            if (_windowsByLayer.TryGetValue(viewData.Layer, out var layerWindows))
            {
                layerWindows.Remove(viewData.View);
            }
            _viewLayers.Remove(viewData.View);

            var visualElement = viewData.View.VisualElement;
            if (visualElement == null) return;

            _viewsByElement.Remove(visualElement);
            visualElement.UnregisterCallback<PointerDownEvent>(OnWindowPointerDown, TrickleDown.TrickleDown);
        }

        private void RemoveWindowFocus(ViewData viewData)
        {
            var element = viewData.View.VisualElement;
            if (element != null)
            {
                element.RemoveFromClassList(ActiveClass);
                element.AddToClassList(InactiveClass);
            }
            FocusTopmostWindow(viewData.Layer);
        }

        private void OnWindowPointerDown(PointerDownEvent evt)
        {
            if (evt.currentTarget is VisualElement visualElement &&
                _viewsByElement.TryGetValue(visualElement, out var view))
            {
                FocusWindow(view);
            }
        }

        private void FocusWindow(IView view)
        {
            var targetElement = view.VisualElement;
            if (targetElement == null) return;

            var parent = targetElement.parent;
            if (parent != null && parent[parent.childCount - 1] != targetElement)
            {
                if (_modalBlockers.TryGetValue(view, out var blocker))
                {
                    blocker.BringToFront();
                }
                targetElement.BringToFront();
            }

            var viewLayer = _viewLayers.TryGetValue(view, out var layer) ? layer : UILayer.Default;

            if (_activeWindowsByLayer.TryGetValue(viewLayer, out var currentActiveView))
            {
                if (currentActiveView == view) return;
                var currentElement = currentActiveView.VisualElement;
                if (currentElement != null)
                {
                    currentElement.RemoveFromClassList(ActiveClass);
                    currentElement.AddToClassList(InactiveClass);
                }
            }

            targetElement.RemoveFromClassList(InactiveClass);
            targetElement.AddToClassList(ActiveClass);
            _activeWindowsByLayer[viewLayer] = view;
        }

        private void FocusWindow(ViewData viewData) => FocusWindow(viewData.View);

        private void FocusTopmostWindow(UILayer layerType)
        {
            var layer = _uiRoot.Container.Q(layerType.ToContainerName());
            if (layer == null) return;

            for (var i = layer.childCount - 1; i >= 0; i--)
            {
                var child = layer[i];
                if (child.ClassListContains(HiddenClass) ||
                    child.style.display == DisplayStyle.None ||
                    (!child.ClassListContains(WindowClass) && !child.ClassListContains(InactiveClass)))
                {
                    continue;
                }

                child.RemoveFromClassList(InactiveClass);
                child.AddToClassList(ActiveClass);

                if (_viewsByElement.TryGetValue(child, out var topmostView))
                {
                    _activeWindowsByLayer[layerType] = topmostView;
                }
                break;
            }
        }
    }
}
