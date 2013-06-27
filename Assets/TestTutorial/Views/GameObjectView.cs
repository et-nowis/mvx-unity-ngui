using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial/Views/GameObjectView")]
public class GameObjectView : BaseViewController
{
    public new GameObjectViewModel ViewModel
    {
        get { return (GameObjectViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();

        /*this.AddBindings(
            new Dictionary<object, string>()
                {
                     { tipValueLabel, "text TipValue, Converter=StringFormat, ConverterParameter='{0:0.000}';" },
                });
		
        this.AddBindings(
            new Dictionary<object, string>()
                {
                   // { TipValueLabel, "{'Text':{'Path':'TipValue'}}" }
                     { clickButton , "onClick ClickCommand" }
                 });
        */

    }

}


