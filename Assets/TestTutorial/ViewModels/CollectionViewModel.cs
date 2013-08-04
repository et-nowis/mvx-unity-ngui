using System.Collections.ObjectModel;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using TestTutorial.ViewModels;
using Cirrious.MvvmCross.Unity.Views;

[MvxUnityView("TestTutorial/Views/CollectionView")]
public class CollectionViewModel : BaseViewModel
{
    private ObservableCollection<CellViewModel> _list = new ObservableCollection<CellViewModel>();

    public ObservableCollection<CellViewModel> List
    {
        get { return _list; }
        set
        {
            _list = value;
            RaisePropertyChanged(() => List);
        }
    }

    public void Init()
    {
        for (int i = 1; i <= 3; i++)
        {
            CellViewModel cell = new CellViewModel(this);
            cell.Init(new CellViewModel.Parameters() { CellValue = (float)i });
            List.Add(cell);
        }
    }

    public ICommand ShowItemCommand
    {
        get
        {
            return new MvxCommand<CellViewModel>(item =>
            {
                Mvx.Trace("{0}", item.CellValue);
            });
        }
    }
}