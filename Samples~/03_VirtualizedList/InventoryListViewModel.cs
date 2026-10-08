using ObservableCollections;
using SUIF.Base;

namespace SUIF.Samples.VirtualizedList
{
    public class InventoryItemViewModel : BaseViewModel
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }

    public class InventoryListViewModel : BaseViewModel
    {
        public ObservableList<InventoryItemViewModel> Items { get; } = new();

        public InventoryListViewModel()
        {
            for (var i = 1; i <= 100; i++)
            {
                Items.Add(new InventoryItemViewModel { ItemName = $"Item #{i}", Quantity = i * 2 });
            }
        }
    }
}
