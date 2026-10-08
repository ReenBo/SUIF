using System;
using System.Collections.Generic;
using SUIF.ViewSystems;

namespace SUIF.API
{
    public interface IViewCache
    {
        bool TryGetView(Type type, out ViewData viewData);
        void Register(Type type, ViewData viewData);
        void Unregister(Type type);
        IReadOnlyList<ViewData> GetAllViews();
    }
}
