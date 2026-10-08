using Cysharp.Threading.Tasks;

namespace SUIF.API
{
    public interface IViewFactory
    {
        UniTask<TView> CreateAsync<TView>() where TView : class, IView;
        void ReleaseAsset(string key);
    }
}
