using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using SUIF.API;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SUIF.Addressables
{
    public class AddressablesUIAssetProvider : IUIAssetProvider
    {
        private readonly Dictionary<string, AsyncOperationHandle> _handles = new();

        public async UniTask<T> LoadAssetAsync<T>(string key, CancellationToken ct = default) where T : Object
        {
            ct.ThrowIfCancellationRequested();
            if (_handles.TryGetValue(key, out var existingHandle))
            {
                return existingHandle.Result as T;
            }

            var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(key);
            _handles[key] = handle;

            var asset = await handle.ToUniTask(cancellationToken: ct);
            return asset;
        }

        public void ReleaseAsset(string key)
        {
            if (_handles.Remove(key, out var handle))
            {
                if (handle.IsValid())
                {
                    UnityEngine.AddressableAssets.Addressables.Release(handle);
                }
            }
        }
    }
}
