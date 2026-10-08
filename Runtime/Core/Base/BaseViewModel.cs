using System;
using System.Collections.Generic;
using SUIF.API;

namespace SUIF.Base
{
    public abstract class BaseViewModel : IViewModel, IDisposable
    {
        private readonly List<IDisposable> _disposables = new();

        public void AddDisposable(IDisposable disposable)
        {
            if (disposable != null)
            {
                _disposables.Add(disposable);
            }
        }

        public virtual void Dispose()
        {
            for (var i = 0; i < _disposables.Count; i++)
            {
                _disposables[i]?.Dispose();
            }
            _disposables.Clear();
        }
    }
}
