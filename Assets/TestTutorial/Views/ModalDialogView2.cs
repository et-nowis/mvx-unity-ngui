using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial_NGUI_3/Views/ModalDialogView2")]
public class ModalDialogView2 : BaseViewController, IMvxModalUnityView
{
    public UILabel dialogLabel;
    public UIButton closeButton;
    public UIButton okButton;
    public UIButton cancelButton;

    public new ModalDialogViewModel2 ViewModel
    {
        get { return (ModalDialogViewModel2)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();
		
		var bindingSet = this.CreateBindingSet<ModalDialogView2, ModalDialogViewModel2>();

 			bindingSet.Bind(closeButton).To(vm => vm.CloseCommand);
			bindingSet.Bind(okButton).To(vm => vm.OkCommand);
			bindingSet.Bind(cancelButton).To(vm => vm.CancelCommand);

		bindingSet.Apply();

        //this.ViewModel.DialogText = "Hello World (Modal)";
    }

}


