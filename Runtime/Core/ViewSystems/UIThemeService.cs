using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using SUIF.API;
using UnityEngine.UIElements;

namespace SUIF.ViewSystems
{
    public class UIThemeService : IUIThemeService, IDisposable
    {
        private readonly IUIRoot _uiRoot;
        private readonly IUIAssetProvider _assetProvider;
        private StyleSheet _currentTypographyTheme;
        private string _currentThemeKey;

        public UIThemeService(IUIRoot uiRoot, IUIAssetProvider assetProvider)
        {
            _uiRoot = uiRoot;
            _assetProvider = assetProvider;
        }

        public async UniTask SetTypographyThemeAsync(string addressableKey, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            if (_currentTypographyTheme != null)
            {
                _uiRoot.Container.styleSheets.Remove(_currentTypographyTheme);
                if (!string.IsNullOrEmpty(_currentThemeKey))
                {
                    _assetProvider.ReleaseAsset(_currentThemeKey);
                }
                _currentTypographyTheme = null;
                _currentThemeKey = null;
            }

            if (string.IsNullOrEmpty(addressableKey)) return;

            _currentThemeKey = addressableKey;
            var styleSheet = await _assetProvider.LoadAssetAsync<StyleSheet>(addressableKey, ct);
            ct.ThrowIfCancellationRequested();

            if (styleSheet != null)
            {
                _currentTypographyTheme = styleSheet;
                _uiRoot.Container.styleSheets.Add(_currentTypographyTheme);
            }
        }

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(_currentThemeKey))
            {
                _assetProvider.ReleaseAsset(_currentThemeKey);
                _currentThemeKey = null;
            }
        }
    }
}
