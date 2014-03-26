using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial_NGUI_3/Views/MainMenuView")]
public class MainMenuView : BaseViewController
{
    public UIButton startButton;
    public UIButton optionButton;

    public new MainMenuViewModel ViewModel
    {
        get { return (MainMenuViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    private MvxSubscriptionToken token;

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();
		
		var bindingSet = this.CreateBindingSet<MainMenuView, MainMenuViewModel>();

 			bindingSet.Bind(startButton).To(vm => vm.StartCommand);
			bindingSet.Bind(optionButton).To(vm => vm.OptionCommand);

		bindingSet.Apply();

        //token = this.ViewModel.Subscribe<ClickedButtonMessage>( OnClickedButtonMessage );
    }

    //	protected override void Dispose(bool disposing)
    //	{
    //		//this.ViewModel.Unsubscribe<ClickedButtonMessage>(token);
    //		base.Dispose(disposing);	
    //	}

    /*	private void OnClickedButtonMessage(ClickedButtonMessage message)
        {
            GameObject prefab = Resources.Load( "MeGirlResort/Views/UIAlertView" ) as GameObject;
            UIAlertView alertView = NGUITools.AddChild( NGUITools.FindCameraForLayer( prefab.layer ).gameObject, prefab ).GetComponent<UIAlertView>();
		
            alertView.OnClickedButtonAtIndex += delegate (object sender, OnClickedButtonArgs e) {  
													
                                                        Debug.Log("hello" );
                                                        message.ButtonIndex = e.ButtonIndex;
			
													
			
                                                    };
		
        }
    */
}
