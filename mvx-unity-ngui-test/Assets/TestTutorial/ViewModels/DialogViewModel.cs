using Cirrious.MvvmCross.Unity.Views;
using Cirrious.MvvmCross.Binding.Attributes;

namespace TestTutorial.ViewModels
{
	[MvxView("TestTutorial/Views/DialogView")]
    public class DialogViewModel : BaseViewModel
    {
        private string _dialogText;
        public string DialogText
        {
            get { return _dialogText; }
            set { _dialogText = value; RaisePropertyChanged(() => DialogText); }
        }
    }
}