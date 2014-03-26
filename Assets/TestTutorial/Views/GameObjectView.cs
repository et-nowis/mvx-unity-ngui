using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial_NGUI_3/Views/GameObjectView")]
public class GameObjectView : BaseViewController
{
    public new GameObjectViewModel ViewModel
    {
        get { return (GameObjectViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();
    }

}


