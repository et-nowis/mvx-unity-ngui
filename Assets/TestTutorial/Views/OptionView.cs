using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial/Views/OptionView")]
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

        this.AddBindings(
            new Dictionary<object, string>()
                {
					 { backButton , 	   "onClick BackCommand" }
                 });

    }
}
