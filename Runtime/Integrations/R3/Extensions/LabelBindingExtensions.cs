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
            var slot = bunch.Binder.FixedSlot(bunch.Element, capacity);
            if (slot != null && property != null)
            {
                bunch.Binder.AddDisposable(property.Subscribe(slot, static (text, target) => target.SetText(text)));
            }
            return slot;
        }

        public static FixedTextSlot ToFixedText<TElement>(this ElementBunch<TElement> bunch, Observable<int> property, int capacity = 16, string format = "{0}")
            where TElement : TextElement
        {
            return BindFormatted(bunch, property, capacity, format);
        }

        public static FixedTextSlot ToFixedText<TElement>(this ElementBunch<TElement> bunch, Observable<long> property, int capacity = 16, string format = "{0}")
            where TElement : TextElement
        {
            return BindFormatted(bunch, property, capacity, format);
        }

        public static FixedTextSlot ToFixedText<TElement>(this ElementBunch<TElement> bunch, Observable<float> property, int capacity = 16, string format = "{0:F1}")
            where TElement : TextElement
        {
            return BindFormatted(bunch, property, capacity, format);
        }

        public static FixedTextSlot ToFixedWritten<TElement, TData>(
            this ElementBunch<TElement> bunch,
            Observable<TData> property,
            int capacity,
            FixedTextWriter<TData> writer)
            where TElement : TextElement
        {
            var slot = bunch.Binder.FixedSlot(bunch.Element, capacity);
            if (slot != null && property != null && writer != null)
            {
                bunch.Binder.AddDisposable(property.Subscribe((slot, writer), static (data, ctx) => ctx.slot.SetWritten(data, ctx.writer)));
            }
            return slot;
        }

        public static FixedTextSlot ToFixedWritten<TElement, TData, TState>(
            this ElementBunch<TElement> bunch,
            Observable<TData> property,
            int capacity,
            TState state,
            FixedTextWriter<(TData Data, TState State)> writer)
            where TElement : TextElement
        {
            var slot = bunch.Binder.FixedSlot(bunch.Element, capacity);
            if (slot != null && property != null && writer != null)
            {
                bunch.Binder.AddDisposable(property.Subscribe((slot, state, writer), static (data, ctx) => ctx.slot.SetWritten((data, ctx.state), ctx.writer)));
            }
            return slot;
        }

        private static FixedTextSlot BindFormatted<TElement, TValue>(
            ElementBunch<TElement> bunch,
            Observable<TValue> property,
            int capacity,
            string format)
            where TElement : TextElement
        {
            var slot = bunch.Binder.FixedSlot(bunch.Element, capacity);
            if (slot != null && property != null)
            {
                bunch.Binder.AddDisposable(property.Subscribe((slot, format), static (val, ctx) => ctx.slot.SetFormat(ctx.format, val)));
            }
            return slot;
        }
    }
}
