using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;

namespace TestTutorial.ViewModels
{
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
                    UnityEngine.Debug.Log("ClickCommand");

                });
            }
        }

        public ICommand PressCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    UnityEngine.Debug.Log("PressCommand");

                });
            }
        }


    }

}
