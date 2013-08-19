using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;
using Cirrious.MvvmCross.Binding.Attributes;

[MvxView("TestTutorial/Views/ModalDialogView2")]
public class ModalDialogView2 : BaseViewController, IMvxModalUnityView
{
    public UILabel dialogLabel;
    public UIButton closeButton;
    public UIButton okButton;
    public UIButton cancelButton;

    public new ModalDialogViewModel2 ViewModel
    {
        get { return (ModalDialogViewModel2)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();

        this.AddBindings(
            new Dictionary<object, string>()
                {
                     { dialogLabel, "text Message" }
                });


        this.AddBindings(
            new Dictionary<object, string>()
                {
                    // { TipValueLabel, "{'Text':{'Path':'TipValue'}}" }
					 { closeButton, "onClick CloseCommand" },
					 { okButton, "onClick OkCommand" },
					 { cancelButton, "onClick CancelCommand" }
                 });

        //this.ViewModel.DialogText = "Hello World (Modal)";
    }

}


