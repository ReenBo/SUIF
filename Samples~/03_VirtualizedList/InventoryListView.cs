using ObservableCollections;
using SUIF.Attributes;
using SUIF.Base;
using SUIF.R3.Extensions;
using UnityEngine.UIElements;

namespace SUIF.Samples.VirtualizedList
{
    [UIView("InventoryListView", SUIF.API.UILayer.Screens)]
    public class InventoryListView : BaseView<InventoryListViewModel>
    {
        [UQuery("inventory-list")] private ListView _listView;
        private VisualTreeAsset _itemTemplate;

        public InventoryListView(VisualElement root) : base(root) { }

        public void SetItemTemplate(VisualTreeAsset template) => _itemTemplate = template;

        protected override void Bind()
        {
            if (_itemTemplate != null)
            {
                Binder.Bind(_listView).ToDataSource<InventoryItemViewModel, InventoryItemView>(ViewModel.Items, _itemTemplate);
            }
        }
    }

    public class InventoryItemView : BaseView<InventoryItemViewModel>
    {
        [UQuery("item-name")] private Label _nameLabel;

        public InventoryItemView(VisualElement root) : base(root) { }

        protected override void Bind()
        {
            _nameLabel.text = ViewModel?.ItemName;
        }
    }
}
