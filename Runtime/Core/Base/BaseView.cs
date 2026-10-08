using System;
using SUIF.API;
using SUIF.Binding;
using UnityEngine.UIElements;

namespace SUIF.Base
{
    public abstract class BaseView<TViewModel> : IView where TViewModel : class, IViewModel
    {
        public VisualElement VisualElement { get; }
        protected TViewModel ViewModel { get; private set; }

        protected ViewBinder Binder { get; } = new();

        protected BaseView(VisualElement root)
        {
            VisualElement = root ?? throw new ArgumentNullException(nameof(root));
            SetVisualElements();
        }

        public virtual void Initialize(IViewModel model)
        {
            if (model is not TViewModel typedModel)
            {
                throw new ArgumentException($"Expected ViewModel of type {typeof(TViewModel).Name}");
            }

            ViewModel = typedModel;
            Bind();
        }

        protected virtual void SetVisualElements()
        {
            UQueryResolver.Resolve(this, VisualElement);
        }

        protected abstract void Bind();

        private const string HiddenClass = "is-hidden";

        protected void AddDisposable(IDisposable disposable)
        {
            Binder.AddDisposable(disposable);
        }

        public virtual void Show()
        {
            if (VisualElement.ClassListContains(HiddenClass))
            {
                VisualElement.RemoveFromClassList(HiddenClass);
            }
            if (VisualElement.style.display == DisplayStyle.None)
            {
                VisualElement.style.display = StyleKeyword.Null;
            }
        }

        public virtual void Hide()
        {
            if (!VisualElement.ClassListContains(HiddenClass))
            {
                VisualElement.AddToClassList(HiddenClass);
            }
        }

        public virtual void Unbind()
        {
            Binder.Clear();
            ViewModel = null;
        }

        public virtual void Dispose()
        {
            Binder.Dispose();
            if (ViewModel is IDisposable disposable)
            {
                disposable.Dispose();
            }
            ViewModel = null;
        }
    }
}
