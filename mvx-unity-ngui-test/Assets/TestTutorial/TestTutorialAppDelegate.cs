using Cirrious.CrossCore;
using Cirrious.MvvmCross.Unity.Platform;
using Cirrious.MvvmCross.ViewModels;

public class TestTutorialAppDelegate : MvxApplicationDelegate
{

    void Start()
    {

        var setup = new TestTutorialSetup(this);
        setup.Initialize();

        var start = Mvx.Resolve<IMvxAppStart>();
        start.Start();

    }

}
