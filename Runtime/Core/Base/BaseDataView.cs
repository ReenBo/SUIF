using SUIF.API;
using UnityEngine.UIElements;

namespace SUIF.Base
{
    public abstract class BaseDataView<TViewModel, TData> : BaseView<TViewModel>
        where TViewModel : class, IViewModel
    {
        protected BaseDataView(VisualElement root) : base(root)
        {
        }

        protected abstract void RenderData(TData data);
    }
}
