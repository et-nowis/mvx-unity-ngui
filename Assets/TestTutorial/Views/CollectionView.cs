using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;

[MvxUnityView("TestTutorial/Views/CollectionView")]
public class CollectionView : BaseCollectionViewController, IMvxModalUnityView
{
    //public UILabel  dialogLabel;
    //public UIButton closeButton;

    public MvxCollectionViewSource CollectionViewList;

    public new CollectionViewModel ViewModel
    {
        get { return (CollectionViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();

        /*this.AddBindings(
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

        this.ViewModel.DialogText = "Hello World";*/

        this.CreateBinding(CollectionViewList).To<CollectionViewModel>(vm => vm.List).Apply();
    }

}


