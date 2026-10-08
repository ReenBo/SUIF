using System;
using SUIF.Attributes;
using SUIF.Base;
using UnityEngine.UIElements;

namespace SUIF.Samples.Modals
{
    [UIView("ConfirmDialog", SUIF.API.UILayer.Popups, isModal: true)]
    public class ConfirmDialogView : BaseView<ConfirmDialogViewModel>
    {
        [UQuery("confirm-btn")] private Button _confirmBtn;
        [UQuery("cancel-btn")] private Button _cancelBtn;

        public ConfirmDialogView(VisualElement root) : base(root) { }

        protected override void Bind()
        {
            _confirmBtn.clicked += () => ViewModel.Confirm();
            _cancelBtn.clicked += () => ViewModel.Cancel();
        }
    }

    public class ConfirmDialogViewModel : BaseViewModel
    {
        public Action OnConfirmed;
        public Action OnCancelled;

        public void Confirm() => OnConfirmed?.Invoke();
        public void Cancel() => OnCancelled?.Invoke();
    }
}
