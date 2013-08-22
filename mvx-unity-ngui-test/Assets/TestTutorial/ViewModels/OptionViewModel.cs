using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Unity.Views;
using Cirrious.MvvmCross.Binding.Attributes;

namespace TestTutorial.ViewModels
{
	[MvxView("TestTutorial/Views/OptionView")]
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