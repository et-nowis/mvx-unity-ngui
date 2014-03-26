using System.Collections.ObjectModel;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using TestTutorial.Messages;

namespace TestTutorial.ViewModels
{
    public class AlertViewModel : BaseViewModel
    {
        private InteractionRequest<ButtonClickMessage> _buttonClicklInteractionRequest;
        public IInteractionRequest ButtonClickInteractionRequest
        {
            get { return _buttonClicklInteractionRequest ?? (_buttonClicklInteractionRequest = new InteractionRequest<ButtonClickMessage>()); }
        }

        private Collection<string> _buttonTexts = new Collection<string>();
        public Collection<string> ButtonTexts
        {
            get { return _buttonTexts; }
            set
            {
                _buttonTexts = value;
            }
        }

        private Collection<string> _uiIconNames = new Collection<string>();
        public Collection<string> UIIconNames
        {
            get { return _uiIconNames; }
            set
            {
                _uiIconNames = value;
            }
        }

        private Collection<string> _uiIconTexts = new Collection<string>();
        public Collection<string> UIIconTexts
        {
            get { return _uiIconTexts; }
            set
            {
                _uiIconTexts = value;
            }
        }

        private Collection<string> _itemIconNames = new Collection<string>();
        public Collection<string> ItemIconNames
        {
            get { return _itemIconNames; }
            set
            {
                _itemIconNames = value;
            }
        }

        private Collection<string> _itemIconTexts = new Collection<string>();
        public Collection<string> ItemIconTexts
        {
            get { return _itemIconTexts; }
            set
            {
                _itemIconTexts = value;
            }
        }

        private Collection<string> _confirmButtonTexts = new Collection<string>();
        public Collection<string> ConfirmButtonTexts
        {
            get { return _confirmButtonTexts; }
            set
            {
                _confirmButtonTexts = value;
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public class Parameters
        {
            public string Title { get; set; }
            public string Message { get; set; }
            public string ButtonTexts { get; set; }
            public string UIIconNames { get; set; }
            public string UIIconTexts { get; set; }
            public string ItemIconNames { get; set; }
            public string ItemIconTexts { get; set; }
            public string ConfirmButtonTexts { get; set; }
        }

        public void Init(Parameters parameters)
        {
            this.Message = parameters.Message;
            this.Title = parameters.Title;
            string[] buttonTexts = parameters.ButtonTexts.Split(',');
            string[] uiIconNames = parameters.UIIconNames.Split(',');
            string[] uiIconTexts = parameters.UIIconTexts.Split(',');
            string[] itemIconNames = parameters.ItemIconNames.Split(',');
            string[] itemIconTexts = parameters.ItemIconTexts.Split(',');
            string[] confirmButtonTexts = parameters.ConfirmButtonTexts.Split(',');

            foreach (string title in buttonTexts)
            {
                ButtonTexts.Add(title);
            }

            foreach (string name in uiIconNames)
            {
                UIIconNames.Add(name);
            }

            foreach (string text in uiIconTexts)
            {
                UIIconTexts.Add(text);
            }

            foreach (string name in itemIconNames)
            {
                ItemIconNames.Add(name);
            }

            foreach (string text in itemIconTexts)
            {
                ItemIconTexts.Add(text);
            }

            foreach (string text in confirmButtonTexts)
            {
                ConfirmButtonTexts.Add(text);
            }
        }

        public string ButtonTextAtIndex(int i)
        {
            return ButtonTexts[i];
        }

        private ICommand _buttonClickCommand;
        public ICommand ButtonClickCommand
        {
            get
            {
                return _buttonClickCommand ?? (_buttonClickCommand = new MvxCommand<int>(OnButtonClick));
            }
        }

        private void OnButtonClick(int buttonIndex)
        {
            this.Close(this);

            //            Publish(new ButtonClickMessage(this)
            //            {
            //                ButtonIndex = buttonIndex,
            //                ButtonAction = ButtonTexts[buttonIndex]
            //            });

            RaiseRequest(_buttonClicklInteractionRequest
                , new ButtonClickMessage(this)
                {
                    ButtonIndex = buttonIndex,
                    ButtonAction = ButtonTexts[buttonIndex]
                } 
                , (ButtonClickMessage c) =>
                {
                    UnityEngine.Debug.Log(this + " OnButtonClick CallBack " + c.ButtonAction);
                }
            );
        }

        private ICommand _confirmButtonClickCommand;
        public ICommand ConfirmButtonClickCommand
        {
            get
            {
                return _confirmButtonClickCommand ?? (_confirmButtonClickCommand = new MvxCommand(OnConfirmButtonClick));
            }
        }

        private void OnConfirmButtonClick()
        {
            this.Close(this);

            //            Publish(new ButtonClickMessage(this)
            //            {
            //                ButtonIndex = 0,
            //                ButtonAction = "Confirm",
            //            });

            RaiseRequest(_buttonClicklInteractionRequest
                , new ButtonClickMessage(this)
                {
                    ButtonIndex = 0,
                    ButtonAction = "Confirm"
                }
                , (ButtonClickMessage c) =>
                {
                    UnityEngine.Debug.Log(this + " OnButtonClick CallBack " + c.ButtonAction);
                }
            );
        }

        protected override bool DoClose()
        {
            base.DoClose();

            //            Publish(_buttonClickMessage ?? new ButtonClickMessage(this)
            //            {
            //                ButtonIndex = 0,
            //                ButtonAction = "Close"
            //            });

            RaiseRequest(_buttonClicklInteractionRequest
                , new ButtonClickMessage(this)
                {
                    ButtonIndex = 0,
                    ButtonAction = "Close"
                }
                , (ButtonClickMessage c) =>
                {
                    UnityEngine.Debug.Log(this + " OnButtonClick CallBack " + c.ButtonAction);
                }
            );

            return true;
        }
     
    }
}