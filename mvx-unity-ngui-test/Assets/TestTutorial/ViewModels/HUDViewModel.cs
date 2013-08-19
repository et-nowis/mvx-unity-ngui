using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Unity.Views;
using Cirrious.MvvmCross.Binding.Attributes;

namespace TestTutorial.ViewModels
{
	[MvxView("TestTutorial/Views/HUDView")]
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

    }

}
