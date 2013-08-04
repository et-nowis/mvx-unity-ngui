using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;

namespace TestTutorial.ViewModels
{
	[MvxUnityView("TestTutorial/Views/MainMenuView")]
    public class MainMenuViewModel : BaseViewModel
    {
        public ICommand StartCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    this.Close(this);
                    this.ShowViewModel<HUDViewModel>();
                });
            }
        }

        public ICommand OptionCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    this.ShowViewModel<OptionViewModel>();
                });
            }
        }
    }
}