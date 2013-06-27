using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace TestTutorial.ViewModels
{
    public class OptionViewModel : BaseViewModel
    {
        public ICommand BackCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<MainMenuViewModel>();
                });
            }
        }


    }

}