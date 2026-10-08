using Cysharp.Threading.Tasks;

namespace SUIF.API
{
    public interface IUIFlow
    {
        UniTask<TView> OpenViewAsync<TView>() where TView : class, IView;
        UniTask CloseViewAsync<TView>() where TView : class, IView;
    }
}
