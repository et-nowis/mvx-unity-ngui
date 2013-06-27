using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial/Views/ModalDialogView")]
public class ModalDialogView : BaseViewController, IMvxModalUnityView
{
    public UILabel dialogLabel;
    public UIButton closeButton;
    public UIButton okButton;
    public UIButton cancelButton;

    public new ModalDialogViewModel ViewModel
    {
        get { return (ModalDialogViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();

        this.AddBindings(
            new Dictionary<object, string>()
                {
                     { dialogLabel, "text Message" },
					 { dialogLabel.gameObject, "active IsDialogLabelVisible" }
                });


        this.AddBindings(
            new Dictionary<object, string>()
                {
                    // { TipValueLabel, "{'Text':{'Path':'TipValue'}}" }
					 { closeButton, "onClick CloseCommand" },
					 { okButton, "onClick OkCommand" },
					 { cancelButton, "onClick CancelCommand" }
                 });

        this.ViewModel.Message = "Hello World (Modal)";
    }

}


