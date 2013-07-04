using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial/Views/MainMenuView")]
public class MainMenuView : BaseViewController
{
    public UIButton startButton;
    public UIButton optionButton;

    public new MainMenuViewModel ViewModel
    {
        get { return (MainMenuViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();

        this.AddBindings(
            new Dictionary<object, string>() {
			    { startButton, "onClick StartCommand" },
			    { optionButton, "onClick OptionCommand" }
            }
        );
    }
}