using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using SUIF.API;
using UnityEngine;

namespace SUIF.Common
{
    public class DirectAssetProvider : IUIAssetProvider
    {
        private readonly Dictionary<string, Object> _assets = new();

        public void RegisterAsset(string key, Object asset)
        {
            _assets[key] = asset;
        }

        public UniTask<T> LoadAssetAsync<T>(string key, CancellationToken ct = default) where T : Object
        {
            ct.ThrowIfCancellationRequested();
            if (_assets.TryGetValue(key, out var obj) && obj is T typed)
            {
                return UniTask.FromResult(typed);
            }

            var loaded = Resources.Load<T>(key);
            return UniTask.FromResult(loaded);
        }

        public void ReleaseAsset(string key)
        {
        }
    }
}
