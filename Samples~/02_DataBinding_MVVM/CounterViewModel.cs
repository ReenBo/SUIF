using R3;
using SUIF.Base;

namespace SUIF.Samples.DataBinding
{
    public class CounterViewModel : BaseViewModel
    {
        public BindableReactiveProperty<int> Count { get; } = new(0);

        public void Increment()
        {
            Count.Value++;
        }
    }
}
