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
        Debug.Log("CompositeView ViewDidLoad ");
        base.ViewDidLoad();

        if (ViewModel == null) return;

        var viewControllers = new UIViewController[]
                              {
                                CreateTabFor("Tip", "", ViewModel.Tip),
                                CreateTabFor("Tip2", "", ViewModel.Tip2)
                              };


        // set our selected item
        //SelectedViewController = ViewControllers[0];
    }

    private UIViewController CreateTabFor(string title, string imageName, IMvxViewModel viewModel)
    {
        Debug.Log("CreateTabFor ");
        var innerView = (UIViewController)this.CreateViewControllerFor(viewModel);

        PresentViewController(innerView, false, () => { });
        //innerView.Title = title;
        //innerView.TabBarItem = new UITabBarItem(
        //                        title, 
        //                        UIImage.FromBundle("Images/Tabs/" + imageName + ".png"),
        //                        _createdSoFarCount++);
        return innerView;
    }

}
