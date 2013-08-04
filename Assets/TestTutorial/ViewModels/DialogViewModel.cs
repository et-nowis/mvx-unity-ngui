using Cirrious.MvvmCross.Unity.Views;

namespace TestTutorial.ViewModels
{
	[MvxUnityView("TestTutorial/Views/DialogView")]
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