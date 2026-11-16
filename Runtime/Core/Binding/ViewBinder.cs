using System;
using System.Collections.Generic;
using SUIF.Controls;
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

        public FixedTextSlot FixedSlot(
            TextElement element,
            int capacity = 32,
            string initialText = null,
            bool clampToCapacity = true)
        {
            if (element == null)
            {
                return null;
            }

            if (element is FixedSlotLabel slotLabel)
            {
                slotLabel.Capacity = capacity;
                var internalSlot = slotLabel.Slot;
                if (initialText != null)
                {
                    internalSlot.SetText(initialText);
                }

                AddDisposable(internalSlot);
                return internalSlot;
            }

            var slot = new FixedTextSlot(element, capacity, initialText, clampToCapacity);
            AddDisposable(slot);
            return slot;
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
