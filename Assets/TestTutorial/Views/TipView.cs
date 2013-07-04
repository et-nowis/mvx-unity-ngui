using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial/Views/TipView")]
public class TipView : BaseViewController
{
    public UILabel tipValueLabel;
    public UIButton clickButton;
    public UIButton closeButton;
    public UIButton pressButton;
    public UISlider tipValueSlider;
    public UISprite testSprite;

    public new TipViewModel ViewModel
    {
        get { return (TipViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();

        this.AddBindings(
            new Dictionary<object, string>()
                {
                     { tipValueLabel, "text TipValue, Converter=StringFormat, ConverterParameter='{0:0.000}';" },
					 { tipValueSlider, "sliderValue TipValue" },
					 { tipValueLabel.gameObject, "active IsTipLabelActive" },
					 { testSprite, "spriteName TestSpriteName"}
                });

        this.AddBindings(
            new Dictionary<object, string>()
                {
					 { clickButton , "onClick ClickCommand" },
					 { closeButton , "onClick CloseCommand" },
					 { pressButton , "onPress PressCommand" },
                });

        this.ViewModel.TipValue = 0.25f;
        this.ViewModel.IsTipLabelActive = true;
        this.ViewModel.TestSpriteName = "Light";
    }
}