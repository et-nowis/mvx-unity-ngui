using Cirrious.CrossCore;
using Cirrious.CrossCore.Unity.Views;
using Cirrious.MvvmCross.Unity.Views;
using Cirrious.MvvmCross.ViewModels;
using UnityEngine;

public class CompositeView : BaseTabBarViewController
{

    public new CompositeViewModel ViewModel
    {
        get { return (CompositeViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        Mvx.Trace("CompositeView ViewDidLoad ");
        base.ViewDidLoad();

        if (ViewModel == null) return;

        var viewControllers = new UIViewController[]
                              {
                                CreateTabFor("Tip", "", ViewModel.Tip),
                                CreateTabFor("Tip2", "", ViewModel.Tip2)
                              };
    }

    private UIViewController CreateTabFor(string title, string imageName, IMvxViewModel viewModel)
    {
        Mvx.Trace("CreateTabFor");
        var innerView = (UIViewController)this.CreateViewControllerFor(viewModel);

        PresentViewController(innerView, false, () => { });
        return innerView;
    }

}
