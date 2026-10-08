using Cysharp.Text;
using R3;
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
    }
}
