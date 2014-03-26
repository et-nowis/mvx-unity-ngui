using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial_NGUI_3/Views/DialogView")]
public class DialogView : BaseViewController
{
    public UILabel dialogLabel;
    public UIButton closeButton;

    public new DialogViewModel ViewModel
    {
        get { return (DialogViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();
		
		var bindingSet = this.CreateBindingSet<DialogView, DialogViewModel>();
		
			bindingSet.Bind(dialogLabel).To(vm => vm.DialogText);
			bindingSet.Bind(closeButton).To(vm => vm.CloseCommand);
			
		bindingSet.Apply();
		
		this.ViewModel.DialogText = "Hello World";
    }

}


