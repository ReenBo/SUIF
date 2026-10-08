using UnityEngine.UIElements;

namespace SUIF.Binding
{
    public readonly struct ElementBunch<TElement> where TElement : VisualElement
    {
        public readonly ViewBinder Binder;
        public readonly TElement Element;

        public ElementBunch(ViewBinder binder, TElement element)
        {
            Binder = binder;
            Element = element;
        }
    }
}
