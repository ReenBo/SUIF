using System.Threading;
using Cysharp.Threading.Tasks;

namespace SUIF.API
{
    public interface IViewFactory
    {
        UniTask<TView> CreateAsync<TView>(CancellationToken ct = default) where TView : class, IView;
        void ReleaseAsset(string key);
    }
}
