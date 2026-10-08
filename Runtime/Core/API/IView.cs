using System;
using UnityEngine.UIElements;

namespace SUIF.API
{
    public interface IView : IDisposable
    {
        VisualElement VisualElement { get; }
        void Initialize(IViewModel model);
        void Show();
        void Hide();
        void Unbind();
    }
}
