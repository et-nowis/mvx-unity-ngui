using System;
using System.Windows.Input;
using Cirrious.MvvmCross.ViewModels;
using TestTutorial.Messages;

namespace TestTutorial.ViewModels
{
    public class TipViewModel : BaseViewModel
    {
        private float _tipValue;
        public float TipValue
        {
            get { return _tipValue; }
            set { _tipValue = value; RaisePropertyChanged(() => TipValue); }
        }

        private float _total;
        public float Total
        {
            get { return _total; }
            set { _total = value; RaisePropertyChanged(() => Total); }
        }

        private float _subTotal;
        public float SubTotal
        {
            get { return _subTotal; }
            set { _subTotal = value; RaisePropertyChanged(() => SubTotal); Recalculate(); }
        }

        private int _tipPercent;
        public int TipPercent
        {
            get { return _tipPercent; }
            set { _tipPercent = value; RaisePropertyChanged(() => TipPercent); Recalculate(); }
        }

        private bool _isTipLabelActive;
        public bool IsTipLabelActive
        {
            get { return _isTipLabelActive; }
            set { _isTipLabelActive = value; RaisePropertyChanged(() => IsTipLabelActive); }
        }

        private string _testSpriteName;
        public string TestSpriteName
        {
            get { return _testSpriteName; }
            set { _testSpriteName = value; RaisePropertyChanged(() => TestSpriteName); }
        }
		
		private string _tipInput;
        public string TipInput
        {
            get { return _tipInput; }
            set { _tipInput = value; RaisePropertyChanged(() => TipInput); }
        }


        //private readonly MvxSubscriptionToken _mvxSubscription;

        public TipViewModel()
        {
            // SubTotal = 60.0f;
            // TipPercent = 12;
            // Recalculate();

            //_mvxSubscription = Subscribe<TestChangedMessage>(OnTestChangedMessage);
			
			TipValue = 0.5f;

        }

        //public void Init( float First, string Second, string Answer )
        //{
        //	UnityEngine.Debug.Log( First );
        //}

        private void OnTestChangedMessage(TestChangedMessage message)
        {
            UnityEngine.Debug.Log("OnTestChangedMessage");
        }

        private void Recalculate()
        {
            TipValue = ((int)Math.Round(SubTotal * TipPercent)) / 100.0f;
            Total = TipValue + SubTotal;

        }

        public ICommand ClickCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    UnityEngine.Debug.Log("ClickCommand");
                    //Publish(new TestChangedMessage(this));

                    ShowViewModel<TipViewModel>();

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
