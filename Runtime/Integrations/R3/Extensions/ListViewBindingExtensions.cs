using System;
using System.Collections;
using ObservableCollections;
using R3;
using SUIF.API;
using SUIF.Base;
using SUIF.Binding;
using SUIF.ViewSystems;
using UnityEngine.UIElements;

namespace SUIF.R3.Extensions
{
    public static class ListViewBindingExtensions
    {
        public static void ToDataSource<TViewModel, TView>(
            this ElementBunch<ListView> bunch,
            ObservableList<TViewModel> dataSource,
            VisualTreeAsset itemTemplate)
            where TViewModel : class, IViewModel
            where TView : BaseView<TViewModel>
        {
            var listView = bunch.Element;

            listView.makeItem = () =>
            {
                var element = itemTemplate.Instantiate();
                var root = element.childCount > 0 ? element[0] : element;
                var view = ViewTypeCache<TView>.CreateInstance(root);
                root.userData = view;
                return root;
            };

            listView.bindItem = (visualElement, index) =>
            {
                if (index >= 0 && index < dataSource.Count && visualElement.userData is TView view)
                {
                    var viewModel = dataSource[index];
                    view.Initialize(viewModel);
                }
            };

            listView.unbindItem = (visualElement, index) =>
            {
                if (visualElement.userData is TView view)
                {
                    view.Unbind();
                }
            };

            listView.itemsSource = (IList)dataSource;

            bunch.Binder.AddDisposable(dataSource.ObserveAdd().Subscribe(listView, static (_, lv) => lv.RefreshItems()));
            bunch.Binder.AddDisposable(dataSource.ObserveRemove().Subscribe(listView, static (_, lv) => lv.RefreshItems()));
            bunch.Binder.AddDisposable(dataSource.ObserveReset().Subscribe(listView, static (_, lv) => lv.RefreshItems()));
            bunch.Binder.AddDisposable(dataSource.ObserveReplace().Subscribe(listView, static (_, lv) => lv.RefreshItems()));
            bunch.Binder.AddDisposable(dataSource.ObserveMove().Subscribe(listView, static (_, lv) => lv.RefreshItems()));
        }
    }
}
