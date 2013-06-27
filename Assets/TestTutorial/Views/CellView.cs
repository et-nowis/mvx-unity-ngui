using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial/Views/CellView")]
public class CellView : BaseViewController
{
    public UIButton clickButton;
    public UILabel cellLabel;

    public new CellViewModel ViewModel
    {
        get { return (CellViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();

        //			this.AddBindings(
        //        	new Dictionary<object, string>()
        //            {
        //				 { cellLabel, "text CellValue, Converter=StringFormat, ConverterParameter='{0}'" },
        //    			 { clickButton , "onClick ClickCommand" },
        //	         });

        var bindings = this.CreateBindingSet<CellView, CellViewModel>();
        bindings.Bind(cellLabel).To(vm => vm.CellValue);
        bindings.Bind(clickButton).To(vm => vm.ClickCommand);
        bindings.Apply();

    }



}


