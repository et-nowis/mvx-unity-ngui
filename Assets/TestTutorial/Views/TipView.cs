using System.ComponentModel;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.Converters;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial_NGUI_3/Views/TipView")]
public class TipView : BaseViewController
{
    public UILabel tipValueLabel;
    public UIButton clickButton;
    public UIButton closeButton;
    public UIButton pressButton;
    public UISlider tipValueSlider;
    public UISprite testSprite;
	public UIInput input;
	public UILabel inputLabel;

    public new TipViewModel ViewModel
    {
        get { return (TipViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();
		
		var bindingSet = this.CreateBindingSet<TipView, TipViewModel>();

 			bindingSet.Bind(tipValueLabel).To(vm => vm.TipValue).WithConversion(Converters.StringFormat,"{0:0.000}");
			bindingSet.Bind(tipValueSlider).To(vm => vm.TipValue);
			bindingSet.Bind(input).To(vm => vm.TipInput);
			bindingSet.Bind(inputLabel).To(vm => vm.TipInput);
			bindingSet.Bind(testSprite).To(vm => vm.TestSpriteName);
			bindingSet.Bind(clickButton).To(vm => vm.ClickCommand);
			bindingSet.Bind(closeButton).To(vm => vm.CloseCommand);
			bindingSet.Bind(pressButton).To(vm => vm.PressCommand);

		bindingSet.Apply();
		
        //Debug.Log( this.ViewModel );
//        this.ViewModel.TipValue = 0.25f;
//        this.ViewModel.IsTipLabelActive = true;
//        this.ViewModel.TestSpriteName = "Light";
		
		AddDisposable(this.ViewModel.WeakSubscribe(() => ViewModel.TipValue, UpdateProgressInfo));
		
    }
	
	private void UpdateProgressInfo(object sender, PropertyChangedEventArgs e)
    {
        UpdateProgressInfo();
    }
    
	private void UpdateProgressInfo()
    {
		UnityEngine.Debug.Log( 	"UpdateProgressInfo" );
	}
	
}


