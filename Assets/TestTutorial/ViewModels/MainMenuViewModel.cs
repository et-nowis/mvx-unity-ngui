using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace TestTutorial.ViewModels
{
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