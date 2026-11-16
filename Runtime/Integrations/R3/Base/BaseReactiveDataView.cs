using global::R3;
using SUIF.API;
using SUIF.Base;
using SUIF.R3.Extensions;
using UnityEngine.UIElements;

namespace SUIF.R3.Base
{
    public interface IReactiveDataViewModel<TData> : IViewModel
    {
        Observable<TData> Data { get; }
    }

    public abstract class BaseReactiveDataView<TViewModel, TData> : BaseDataView<TViewModel, TData>
        where TViewModel : class, IReactiveDataViewModel<TData>
    {
        protected BaseReactiveDataView(VisualElement root) : base(root)
        {
        }

        protected override void Bind()
        {
            OnBind();
            Binder.ToData(ViewModel.Data, this, static (data, view) => view.RenderData(data));
        }

        protected virtual void OnBind()
        {
        }
    }
}
