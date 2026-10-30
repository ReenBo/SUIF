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
                if (existingHandle.IsValid() && existingHandle.IsDone && existingHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    return existingHandle.Result as T;
                }
                if (!existingHandle.IsValid() || existingHandle.Status == AsyncOperationStatus.Failed)
                {
                    _handles.Remove(key);
                }
                else if (!existingHandle.IsDone)
                {
                    await existingHandle.WithCancellation(ct);
                    return existingHandle.Result as T;
                }
            }

            var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<T>(key);
            _handles[key] = handle;

            try
            {
                var asset = await handle.WithCancellation(ct);
                return asset;
            }
            catch
            {
                _handles.Remove(key);
                if (handle.IsValid())
                {
                    UnityEngine.AddressableAssets.Addressables.Release(handle);
                }
                throw;
            }
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
