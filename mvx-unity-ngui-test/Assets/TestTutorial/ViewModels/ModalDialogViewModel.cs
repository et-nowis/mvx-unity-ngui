using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using TestTutorial.Messages;
using Cirrious.MvvmCross.Unity.Views;
using Cirrious.MvvmCross.Binding.Attributes;

namespace TestTutorial.ViewModels
{
	[MvxView("TestTutorial/Views/ModalDialogView")]
    public class ModalDialogViewModel : BaseViewModel
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; RaisePropertyChanged(() => Message); }
        }

        private bool _isDialogLabelVisible;
        public bool IsDialogLabelVisible
        {
            get { return _isDialogLabelVisible; }
            set { _isDialogLabelVisible = value; RaisePropertyChanged(() => IsDialogLabelVisible); }
        }

        public class Parameters
        {
            public string message { get; set; }
        }

        public void Init(Parameters parameters)
        {
            this.Message = parameters.message;
        }


        public override ICommand CloseCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Mvx.Trace("ClickCommand");
                    this.Close(this);
                    ButtonClickMessage message = new ButtonClickMessage(this);
                    message.ButtonIndex = 0;
                    Publish(message);
                });
            }
        }

        public ICommand OkCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Mvx.Trace("OkCommand");
                    ButtonClickMessage message = new ButtonClickMessage(this);
                    message.ButtonIndex = 1;
                    Publish(message);
                    ShowViewModel<ModalDialogViewModel2>();
                });
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    Mvx.Trace("CancelCommand");
                    this.Close(this);
                    ButtonClickMessage message = new ButtonClickMessage(this);
                    message.ButtonIndex = 2;
                    Publish(message);
                });
            }
        }
    }
}