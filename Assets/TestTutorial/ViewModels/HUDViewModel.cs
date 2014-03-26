using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace TestTutorial.ViewModels
{
    public class HUDViewModel : BaseViewModel
    {
        private float _tipValue;
        public float TipValue
        {
            get { return _tipValue; }
            set { _tipValue = value; RaisePropertyChanged(() => TipValue); }
        }
		
		private SubViewModel _current;
        public SubViewModel Current
        {
            get { return _current; }
            set { _current = value; RaisePropertyChanged(() => Current); }
        }
		
        public HUDViewModel()
        {
        }

        //public void Init( float First, string Second, string Answer )
        //{
        //	UnityEngine.Debug.Log( First );
        //}

        public ICommand TipView1Command
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<TipViewModel>();
                });
            }
        }

        public ICommand ShowModalDialogCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<ModalDialogViewModel>();
                });
            }
        }

        public ICommand ShowDialogCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<DialogViewModel>();
                });
            }
        }

        public ICommand BuildCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
					ShowViewModel<CollectionViewModel>();
                });
            }
        }
		
		public ICommand AlertCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    AlertViewModel.Parameters param = new AlertViewModel.Parameters();
                    param.Title = "Start?";
                    param.Message = "Hello world";
                    param.ButtonTexts = "OK,Cancel";
                    param.UIIconNames = "";
                    param.UIIconTexts = "";
                    param.ItemIconNames = "";
                    param.ItemIconTexts = "";
                    param.ConfirmButtonTexts = "";
					
					ShowViewModel<AlertViewModel>(param);
                });
            }
        }
		
		public void Init()
		{
			Current = new SubViewModel();
			Current.Title = "Hello World";
		}

    }

}
