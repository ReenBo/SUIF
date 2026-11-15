using SUIF.API;
using UnityEngine.UIElements;

namespace SUIF.Base
{
    public abstract class BaseDtoView<TViewModel, TDto> : BaseView<TViewModel>
        where TViewModel : class, IViewModel
        where TDto : struct, IViewDto
    {
        protected BaseDtoView(VisualElement root) : base(root)
        {
        }

        protected abstract void RenderDto(in TDto dto);
    }
}
