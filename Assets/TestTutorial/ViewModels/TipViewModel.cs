using System;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using TestTutorial.Messages;
using Cirrious.MvvmCross.Unity.Views;

namespace TestTutorial.ViewModels
{
	[MvxUnityView("TestTutorial/Views/TipView")]
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

        public TipViewModel()
        {
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
                    Mvx.Trace("ClickCommand");
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
                    Mvx.Trace("PressCommand");
                });
            }
        }
    }
}