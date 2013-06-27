using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial/Views/DialogView")]
public class DialogView : BaseViewController
{
    public UILabel dialogLabel;
    public UIButton closeButton;

    public new DialogViewModel ViewModel
    {
        get { return (DialogViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();

        this.AddBindings(
        new Dictionary<object, string>()
            {
                 { dialogLabel, "text DialogText" }
            });

        this.AddBindings(
        new Dictionary<object, string>()
            {
                // { TipValueLabel, "{'Text':{'Path':'TipValue'}}" }
				 { closeButton , "onClick CloseCommand" }
             });

        this.ViewModel.DialogText = "Hello World";

        //		this.ViewModel.Closed += HandleViewModelhandleClosed;

    }

    /*	protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ViewModel.Closed -= HandleViewModelhandleClosed;
            }
            base.Dispose(disposing);	
        }

        void HandleViewModelhandleClosed (object sender, EventArgs e)
        {
            GameObject.Destroy( this.gameObject );
        }
    */

}


