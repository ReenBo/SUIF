using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace SUIF.Binding
{
    public class ViewBinder : IDisposable
    {
        private readonly List<IDisposable> _disposables = new();

        public ElementBunch<TElement> Bind<TElement>(TElement element) where TElement : VisualElement
        {
            return new ElementBunch<TElement>(this, element);
        }

        public void AddDisposable(IDisposable disposable)
        {
            if (disposable != null)
            {
                _disposables.Add(disposable);
            }
        }

        public void Clear()
        {
            for (var i = 0; i < _disposables.Count; i++)
            {
                _disposables[i]?.Dispose();
            }
            _disposables.Clear();
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
