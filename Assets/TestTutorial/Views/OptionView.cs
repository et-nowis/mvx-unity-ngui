using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial_NGUI_3/Views/OptionView")]
public class OptionView : BaseViewController
{
    public UIButton backButton;

    public new OptionViewModel ViewModel
    {
        get { return (OptionViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();
		
		var bindingSet = this.CreateBindingSet<OptionView, OptionViewModel>();

 			bindingSet.Bind(backButton).To(vm => vm.BackCommand);

		bindingSet.Apply();
		

    }
}
