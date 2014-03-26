using System.Windows.Input;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using TestTutorial.Messages;

namespace TestTutorial.ViewModels
{
    public class MainMenuViewModel : BaseViewModel
    {
        //private MvxSubscriptionToken _onButtonClickedToken;

        public ICommand StartCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Subscribe<ButtonClickMessage>(OnButtonClicked);

                    AlertViewModel.Parameters param = new AlertViewModel.Parameters();
                    param.Title = "Start?";
                    param.Message = "Hello world";
                    param.ButtonTexts = "OK,Cancel";
                    param.UIIconNames = "";
                    param.UIIconTexts = "";
                    param.ItemIconNames = "";
                    param.ItemIconTexts = "";
                    param.ConfirmButtonTexts = "";

                    //this.ShowViewModel<AlertViewModel>( param );

                    this.Close(this);

                    this.ShowViewModel<HUDViewModel>();
                }
                                        );
            }
        }

        public ICommand OptionCommand
        {
            get
            {
                return new MvxCommand(() =>
                {

                    this.ShowViewModel<OptionViewModel>();

                }
                                      );
            }
        }

        void OnButtonClicked(ButtonClickMessage message)
        {
            string buttonTitle = (message.Sender as AlertViewModel).ButtonTextAtIndex(message.ButtonIndex);

            if (buttonTitle == "OK")
            {
                this.Close(this);
                this.ShowViewModel<HUDViewModel>();
            }


        }


    }

}