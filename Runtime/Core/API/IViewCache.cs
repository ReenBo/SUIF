using System;
using System.Collections.Generic;

namespace SUIF.API
{
    public interface IViewCache
    {
        TView Get<TView>() where TView : class, IView;
        void Add<TView>(TView view) where TView : class, IView;
        void Remove<TView>(TView view) where TView : class, IView;
        bool TryGet<TView>(out TView view) where TView : class, IView;
        IEnumerable<IView> GetAllViews();
    }
}
