using System;
using global::R3;
using SUIF.API;
using SUIF.Binding;

namespace SUIF.R3.Extensions
{
    public static class DtoBindingExtensions
    {
        public static void ToDto<TDto>(this ViewBinder binder, Observable<TDto> property, Action<TDto> onChanged)
            where TDto : struct, IViewDto
        {
            if (binder == null || property == null || onChanged == null)
            {
                return;
            }

            binder.AddDisposable(property.Subscribe(onChanged));
        }

        public static void ToDto<TDto, TState>(
            this ViewBinder binder,
            Observable<TDto> property,
            TState state,
            Action<TDto, TState> onChanged)
            where TDto : struct, IViewDto
        {
            if (binder == null || property == null || onChanged == null)
            {
                return;
            }

            binder.AddDisposable(property.Subscribe((state, onChanged), static (dto, ctx) => ctx.onChanged(dto, ctx.state)));
        }
    }
}
