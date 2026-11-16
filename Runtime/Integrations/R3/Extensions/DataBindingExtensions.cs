using System;
using global::R3;
using SUIF.Binding;

namespace SUIF.R3.Extensions
{
    public static class DataBindingExtensions
    {
        public static void ToData<TData>(this ViewBinder binder, Observable<TData> property, Action<TData> onChanged)
        {
            if (binder == null || property == null || onChanged == null)
            {
                return;
            }

            binder.AddDisposable(property.Subscribe(onChanged));
        }

        public static void ToData<TData, TState>(
            this ViewBinder binder,
            Observable<TData> property,
            TState state,
            Action<TData, TState> onChanged)
        {
            if (binder == null || property == null || onChanged == null)
            {
                return;
            }

            binder.AddDisposable(property.Subscribe((state, onChanged), static (data, ctx) => ctx.onChanged(data, ctx.state)));
        }
    }
}
