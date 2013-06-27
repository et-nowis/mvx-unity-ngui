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
                    UnityEngine.Debug.Log("TipView1Command");
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

    }

}
