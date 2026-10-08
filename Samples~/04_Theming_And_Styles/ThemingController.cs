using Cysharp.Threading.Tasks;
using SUIF.API;
using SUIF.Base;

namespace SUIF.Samples.Theming
{
    public class ThemingController
    {
        private readonly IUIThemeService _themeService;

        public ThemingController(IUIThemeService themeService)
        {
            _themeService = themeService;
        }

        public async UniTask ApplyDefaultTheme()
        {
            await _themeService.SetTypographyThemeAsync("Theme-Default");
        }

        public async UniTask ApplyIosTheme()
        {
            await _themeService.SetTypographyThemeAsync("Theme-IOS");
        }
    }
}
