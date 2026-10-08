using System;
using R3;
using SUIF.Binding;
using UnityEngine.UIElements;

namespace SUIF.R3.Extensions
{
    public static class ButtonBindingExtensions
    {
        public static void ToClick(this ElementBunch<Button> bunch, Action action)
        {
            bunch.Element.clicked += action;
            bunch.Binder.AddDisposable(Disposable.Create((Element: bunch.Element, Action: action), state =>
            {
                state.Element.clicked -= state.Action;
            }));
        }

        public static void ToClick(this ElementBunch<Button> bunch, ReactiveCommand<Unit> command)
        {
            Action action = () => command.Execute(Unit.Default);
            bunch.Element.clicked += action;
            bunch.Binder.AddDisposable(Disposable.Create((Element: bunch.Element, Action: action), state =>
            {
                state.Element.clicked -= state.Action;
            }));
        }
    }
}
