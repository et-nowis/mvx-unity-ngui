using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Unity.Views;

namespace TestTutorial.ViewModels
{
	[MvxUnityView("TestTutorial/Views/CellView")]
    public class CellViewModel : BaseViewModel
    {
        private float _cellValue;
        public float CellValue
        {
            get { return _cellValue; }
            set { _cellValue = value; RaisePropertyChanged(() => CellValue); }
        }

        private IMvxViewModel _parent;

        public CellViewModel(IMvxViewModel parent)
        {
            _parent = parent;
        }

        public class Parameters
        {
            public float CellValue;
        }

        public void Init(Parameters parameters)
        {
            CellValue = parameters.CellValue;
        }

        public ICommand ClickCommand
        {
            get
            {
                return new MvxCommand(() =>
                {

                    ((BaseViewModel)_parent).CloseCommand.Execute(null);


                });
            }
        }


    }

}
