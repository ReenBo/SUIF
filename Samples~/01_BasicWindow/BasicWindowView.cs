using SUIF.Attributes;
using SUIF.Base;
using UnityEngine.UIElements;

namespace SUIF.Samples.BasicWindow
{
    [UIView("BasicWindow", SUIF.API.UILayer.Windows)]
    public class BasicWindowView : BaseView<BasicWindowViewModel>
    {
        public BasicWindowView(VisualElement root) : base(root) { }

        protected override void Bind()
        {
            // Simple visual element setup
        }
    }
}
