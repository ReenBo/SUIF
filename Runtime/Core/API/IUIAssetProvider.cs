using Cysharp.Threading.Tasks;

namespace SUIF.API
{
    public interface IUIAssetProvider
    {
        UniTask<T> LoadAssetAsync<T>(string key) where T : UnityEngine.Object;
        void ReleaseAsset(string key);
    }
}
