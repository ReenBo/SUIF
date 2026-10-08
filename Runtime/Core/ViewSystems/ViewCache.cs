using System;
using System.Collections.Generic;
using SUIF.API;

namespace SUIF.ViewSystems
{
    public class ViewCache : IViewCache
    {
        private readonly Dictionary<Type, ViewData> _views = new();
        private readonly List<ViewData> _viewsList = new();

        public bool TryGetView(Type type, out ViewData viewData)
        {
            return _views.TryGetValue(type, out viewData);
        }

        public void Register(Type type, ViewData viewData)
        {
            if (_views.TryAdd(type, viewData))
            {
                _viewsList.Add(viewData);
            }
        }

        public void Unregister(Type type)
        {
            if (_views.Remove(type, out var viewData))
            {
                _viewsList.Remove(viewData);
            }
        }

        public IReadOnlyList<ViewData> GetAllViews()
        {
            return _viewsList;
        }

        public TView Get<TView>() where TView : class, IView
        {
            return TryGetView(typeof(TView), out var data) ? (TView)data.View : null;
        }

        public void Add<TView>(TView view) where TView : class, IView
        {
            // Optional direct view caching
        }

        public void Remove<TView>(TView view) where TView : class, IView
        {
            Unregister(typeof(TView));
        }

        public bool TryGet<TView>(out TView view) where TView : class, IView
        {
            if (TryGetView(typeof(TView), out var data))
            {
                view = (TView)data.View;
                return true;
            }
            view = null;
            return false;
        }

        IEnumerable<IView> IViewCache.GetAllViews()
        {
            foreach (var item in _viewsList)
            {
                yield return item.View;
            }
        }
    }
}
