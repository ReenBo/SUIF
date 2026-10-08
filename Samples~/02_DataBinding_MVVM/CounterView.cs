using SUIF.Attributes;
using SUIF.Base;
using SUIF.R3.Extensions;
using UnityEngine.UIElements;

namespace SUIF.Samples.DataBinding
{
    [UIView("CounterView", SUIF.API.UILayer.Windows)]
    public class CounterView : BaseView<CounterViewModel>
    {
        [UQuery("counter-label")] private Label _counterLabel;
        [UQuery("increment-btn")] private Button _incrementBtn;

        public CounterView(VisualElement root) : base(root) { }

        protected override void Bind()
        {
            Binder.Bind(_counterLabel).ToText(ViewModel.Count);
            Binder.Bind(_incrementBtn).ToClick(ViewModel.Increment);
        }
    }
}
