using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using Cirrious.MvvmCross.Binding.Unity.Views;
using Cirrious.CrossCore.Unity.Views;
using System.Collections.Generic;
using TestTutorial.ViewModels;

[MvxUnityView("TestTutorial_NGUI_3/Views/CollectionView")]
public class CollectionView : BaseCollectionViewController, IMvxModalUnityView
{
    //public UILabel  dialogLabel;
    //public UIButton closeButton;
	public UIButton AddButton;
	public UIButton RemoveButton;
	public UIButton ReplaceButton;
	public UIButton MoveButton;
	public UIButton ReloadButton;
	public UIButton CloseButton;
	
	public UICollectionView List;

    public new CollectionViewModel ViewModel
    {
        get { return (CollectionViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();

		var bindingSet = this.CreateBindingSet<CollectionView, CollectionViewModel>();
		
		var source = new MvxCollectionViewSource(List);
		List.Source = source;
        bindingSet.Bind(source).To(vm => vm.List);
		bindingSet.Bind(source).For(s => s.SelectedItem).To(vm => vm.Current);
		//bindingSet.Bind(source).For("SelectedItemChanged").To(vm => vm.ShowItemCommand).CommandParameter("Hello");
		bindingSet.Bind(source).For("SelectionChangedCommand").To(vm => vm.SelectionChangedCommand);
		
		bindingSet.Bind(AddButton).To( vm => vm.AddCommand );
		bindingSet.Bind(RemoveButton).To( vm => vm.RemoveCommand );
		bindingSet.Bind(ReplaceButton).To( vm => vm.ReplaceCommand );
		bindingSet.Bind(MoveButton).To( vm => vm.MoveCommand );
		bindingSet.Bind(ReloadButton).To( vm => vm.ReloadCommand );
		
		bindingSet.Bind(CloseButton).To( vm => vm.CloseCommand );

		bindingSet.Apply();
		
		//Property SelectedItem with XXXChanged will auto bind to SelectionChangedCommand
		
		
    }

}


