using Cysharp.Threading.Tasks;

namespace SUIF.API
{
    public interface IUIThemeService
    {
        UniTask SetTypographyThemeAsync(string addressableKey);
    }
}
