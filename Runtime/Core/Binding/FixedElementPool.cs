using System;
using UnityEngine.UIElements;

namespace SUIF.Binding
{
    public sealed class FixedElementPool<TElement> : IDisposable where TElement : VisualElement
    {
        private readonly VisualElement _container;
        private readonly TElement[] _slots;
        private readonly Action<TElement, bool> _setActive;
        private bool _disposed;

        public int Capacity => _slots.Length;

        public FixedElementPool(
            VisualElement container,
            int capacity,
            Func<int, TElement> factory,
            Action<TElement, bool> setActive = null)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            if (capacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be at least 1.");
            }

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            _setActive = setActive ?? DefaultSetActive;
            _slots = new TElement[capacity];

            for (int i = 0; i < capacity; i++)
            {
                var element = factory(i);
                _slots[i] = element;
                _setActive(element, false);
                _container.Add(element);
            }
        }

        public TElement GetSlot(int index)
        {
            return _slots[index];
        }

        public void Render(int activeCount, Action<TElement, int> onBind)
        {
            if (_disposed)
            {
                return;
            }

            int clamped = ClampCount(activeCount);
            for (int i = 0; i < _slots.Length; i++)
            {
                var slot = _slots[i];
                bool isActive = i < clamped;
                _setActive(slot, isActive);
                if (isActive)
                {
                    onBind?.Invoke(slot, i);
                }
            }
        }

        public void Render<TState>(int activeCount, TState state, Action<TElement, int, TState> onBind)
        {
            if (_disposed)
            {
                return;
            }

            int clamped = ClampCount(activeCount);
            for (int i = 0; i < _slots.Length; i++)
            {
                var slot = _slots[i];
                bool isActive = i < clamped;
                _setActive(slot, isActive);
                if (isActive)
                {
                    onBind?.Invoke(slot, i, state);
                }
            }
        }

        private int ClampCount(int activeCount)
        {
            if (activeCount <= 0)
            {
                return 0;
            }

            return activeCount > _slots.Length ? _slots.Length : activeCount;
        }

        private static void DefaultSetActive(TElement element, bool isActive)
        {
            element.style.display = isActive ? StyleKeyword.Null : DisplayStyle.None;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i] = null;
            }
        }
    }
}
