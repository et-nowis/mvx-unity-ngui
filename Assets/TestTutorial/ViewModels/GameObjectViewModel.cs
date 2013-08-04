using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Unity.Views;

namespace TestTutorial.ViewModels
{
	[MvxUnityView("TestTutorial/Views/GameObjectView")]
    public class GameObjectViewModel : BaseViewModel
    {
        private string _spriteName;
        public string SpriteName
        {
            get { return _spriteName; }
            set { _spriteName = value; RaisePropertyChanged(() => SpriteName); }
        }

        public GameObjectViewModel()
        {
        }

        public ICommand ClickCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Mvx.Trace("ClickCommand");
                });
            }
        }

        public ICommand PressCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Mvx.Trace("PressCommand");
                });
            }
        }
    }
}