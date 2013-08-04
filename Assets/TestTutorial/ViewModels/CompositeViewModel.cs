using Cirrious.MvvmCross.ViewModels;
using TestTutorial.ViewModels;
using Cirrious.MvvmCross.Unity.Views;

public class CompositeViewModel
    : MvxViewModel
{
    public TipViewModel Tip { get; private set; }
    public TipViewModel Tip2 { get; private set; }

    public CompositeViewModel()
    {
        Tip = new TipViewModel();
        Tip2 = new TipViewModel();
    }
}
