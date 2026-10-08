using Cysharp.Threading.Tasks;
using SUIF.ViewSystems;

namespace SUIF.API
{
    public interface IUIWindowManager
    {
        UniTask OnViewOpenedAsync(ViewData viewData);
        void OnViewClosed(ViewData viewData);
        void OnViewDestroyed(ViewData viewData);
    }
}
