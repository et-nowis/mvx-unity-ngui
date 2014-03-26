using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial_NGUI_3/Views/TimerView")]
public class TimerView : BaseViewController
{
    public new TimerViewModel ViewModel
    {
        get { return (TimerViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();
    }

    //	protected override void Dispose(bool disposing)
    //	{
    //		base.Dispose(disposing);
    //	}

}


