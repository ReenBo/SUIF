using System.Threading;
using Cysharp.Threading.Tasks;

namespace SUIF.API
{
    public interface IUIFlow
    {
        UniTask<TView> OpenViewAsync<TView>(CancellationToken ct = default) where TView : class, IView;
        UniTask CloseViewAsync<TView>(CancellationToken ct = default) where TView : class, IView;
    }
}
