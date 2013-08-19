using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;
using Cirrious.MvvmCross.Binding.Attributes;

[MvxView("TestTutorial/Views/HUDView")]
public class HUDView : BaseViewController
{
    public UILabel tipValueLabel;
    public UIButton tipView1Button;

    public UIButton showModalDialogButton;
    public UIButton showDialogButton;
    public UIButton buildButton;

    public new HUDViewModel ViewModel
    {
        get { return (HUDViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();

        this.AddBindings(
        new Dictionary<object, string>()
            {
                 { tipValueLabel, "text TipValue, Converter=StringFormat, ConverterParameter='{0:0.000}'" }
            });

        this.AddBindings(
            new Dictionary<object, string>()
                {
					 { tipView1Button , "onClick TipView1Command" },
					 { showDialogButton , "onClick ShowDialogCommand" },
					 { showModalDialogButton , "onClick ShowModalDialogCommand" },
					 { buildButton , "onClick BuildCommand" },
                 });

        this.ViewModel.TipValue = 0.25f;

    }

}