using Cysharp.Text;
using global::R3;
using SUIF.Binding;
using UnityEngine.UIElements;

namespace SUIF.R3.Extensions
{
    public static class LabelBindingExtensions
    {
        public static void ToText(this ElementBunch<Label> bunch, Observable<string> property)
        {
            bunch.Binder.AddDisposable(property.Subscribe(bunch.Element, (text, label) => label.text = text));
        }

        public static void ToText(this ElementBunch<Label> bunch, Observable<int> property)
        {
            bunch.Binder.AddDisposable(property.Subscribe(bunch.Element, (val, label) => label.text = ZString.Concat(val)));
        }

        public static void ToText(this ElementBunch<Label> bunch, Observable<float> property)
        {
            bunch.Binder.AddDisposable(property.Subscribe(bunch.Element, (val, label) => label.text = ZString.Concat(val)));
        }

        public static void ToText<T>(this ElementBunch<Label> bunch, Observable<T> property)
        {
            bunch.Binder.AddDisposable(property.Subscribe(bunch.Element, (val, label) => label.text = val.ToString()));
        }

        public static FixedTextSlot ToFixedText<TElement>(this ElementBunch<TElement> bunch, Observable<string> property, int capacity = 32)
            where TElement : TextElement
        {
            var slot = new FixedTextSlot(bunch.Element, capacity);
            bunch.Binder.AddDisposable(slot);
            bunch.Binder.AddDisposable(property.Subscribe(slot, (text, target) => target.SetText(text)));
            return slot;
        }

        public static FixedTextSlot ToFixedText<TElement>(this ElementBunch<TElement> bunch, Observable<int> property, int capacity = 16, string format = "{0}")
            where TElement : TextElement
        {
            var slot = new FixedTextSlot(bunch.Element, capacity);
            bunch.Binder.AddDisposable(slot);
            bunch.Binder.AddDisposable(property.Subscribe((slot, format), (val, state) => state.slot.SetFormat(state.format, val)));
            return slot;
        }

        public static FixedTextSlot ToFixedText<TElement>(this ElementBunch<TElement> bunch, Observable<long> property, int capacity = 16, string format = "{0}")
            where TElement : TextElement
        {
            var slot = new FixedTextSlot(bunch.Element, capacity);
            bunch.Binder.AddDisposable(slot);
            bunch.Binder.AddDisposable(property.Subscribe((slot, format), (val, state) => state.slot.SetFormat(state.format, val)));
            return slot;
        }

        public static FixedTextSlot ToFixedText<TElement>(this ElementBunch<TElement> bunch, Observable<float> property, int capacity = 16, string format = "{0:F1}")
            where TElement : TextElement
        {
            var slot = new FixedTextSlot(bunch.Element, capacity);
            bunch.Binder.AddDisposable(slot);
            bunch.Binder.AddDisposable(property.Subscribe((slot, format), (val, state) => state.slot.SetFormat(state.format, val)));
            return slot;
        }
    }
}
