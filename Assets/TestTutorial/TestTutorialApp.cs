using Cirrious.MvvmCross.ViewModels;
using TestTutorial.ViewModels;

public class TestTutorialApp : MvxApplication
{
    public TestTutorialApp()
    {
        RegisterAppStart<MainMenuViewModel>();
        //RegisterAppStart<CompositeViewModel>();
    }
}

