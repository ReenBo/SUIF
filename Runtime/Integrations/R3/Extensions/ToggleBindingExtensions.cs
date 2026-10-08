using R3;
using SUIF.Binding;
using UnityEngine.UIElements;

namespace SUIF.R3.Extensions
{
    public static class ToggleBindingExtensions
    {
        public static void ToValue(this ElementBunch<Toggle> bunch, ReactiveProperty<bool> property)
        {
            bunch.Binder.AddDisposable(Observable.FromEvent<EventCallback<ChangeEvent<bool>>, ChangeEvent<bool>>(
                h => evt => h(evt),
                h => bunch.Element.RegisterValueChangedCallback(h),
                h => bunch.Element.UnregisterValueChangedCallback(h))
                .Subscribe(property, (evt, prop) => prop.Value = evt.newValue));

            bunch.Binder.AddDisposable(property.Subscribe(bunch.Element, (value, toggle) => toggle.SetValueWithoutNotify(value)));
        }
    }
}
