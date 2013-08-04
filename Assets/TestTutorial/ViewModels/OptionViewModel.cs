using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Unity.Views;

namespace TestTutorial.ViewModels
{
	[MvxUnityView("TestTutorial/Views/OptionView")]
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