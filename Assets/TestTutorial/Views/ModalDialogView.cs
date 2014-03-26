using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial_NGUI_3/Views/ModalDialogView")]
public class ModalDialogView : BaseViewController, IMvxModalUnityView
{
    public UILabel dialogLabel;
    public UIButton closeButton;
    public UIButton okButton;
    public UIButton cancelButton;

    public new ModalDialogViewModel ViewModel
    {
        get { return (ModalDialogViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();
		
		var bindingSet = this.CreateBindingSet<ModalDialogView, ModalDialogViewModel>();

 			bindingSet.Bind(dialogLabel).To(vm => vm.Message);
			bindingSet.Bind(dialogLabel.gameObject).To(vm => vm.IsDialogLabelVisible);
			bindingSet.Bind(closeButton).To(vm => vm.CloseCommand);
			bindingSet.Bind(okButton).To(vm => vm.OkCommand);
			bindingSet.Bind(cancelButton).To(vm => vm.CancelCommand);

		bindingSet.Apply();
		
        this.ViewModel.Message = "Hello World (Modal)";
    }

}


