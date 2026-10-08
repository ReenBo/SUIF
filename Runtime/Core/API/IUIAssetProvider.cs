using System.Threading;
using Cysharp.Threading.Tasks;

namespace SUIF.API
{
    public interface IUIAssetProvider
    {
        UniTask<T> LoadAssetAsync<T>(string key, CancellationToken ct = default) where T : UnityEngine.Object;
        void ReleaseAsset(string key);
    }
}
