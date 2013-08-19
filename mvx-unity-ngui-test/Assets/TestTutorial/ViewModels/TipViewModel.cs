using System;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.ViewModels;
using TestTutorial.Messages;
using Cirrious.MvvmCross.Unity.Views;
using Cirrious.MvvmCross.Binding.Attributes;

namespace TestTutorial.ViewModels
{
	[MvxView("TestTutorial/Views/TipView")]
    public class TipViewModel : BaseViewModel
    {
        public TipViewModel()
        {
            TipValue = .5f;
            IsTipLabelActive = true;
            TestSpriteName = "Light";

            ShowTipLabel = 1;
            SetTipLabelColor();
        }

        private float _tipValue;
        public float TipValue
        {
            get { return _tipValue; }
            set
            {
                _tipValue = value;
                RaisePropertyChanged(() => TipValue);

                ShowHideTipLabel();
                SetTipLabelColor();
            }
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

        private int _showTipLabel;
        public int ShowTipLabel
        {
            get { return _showTipLabel; }
            set { _showTipLabel = value; RaisePropertyChanged(() => ShowTipLabel); }
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

        private UnityEngine.Color _tipLabelColor;
        public UnityEngine.Color TipLabelColor
        {
            get { return _tipLabelColor; }
            set { _tipLabelColor = value; RaisePropertyChanged(() => TipLabelColor); }
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

        private void ShowHideTipLabel()
        {
            // Hide tip value label if tip value is 0
            ShowTipLabel = TipValue > 0 ? 1 : 0;
        }

        private void SetTipLabelColor()
        {
            if (TipValue < .3)
                TipLabelColor = UnityEngine.Color.red;
            else if (TipValue < .6)
                TipLabelColor = UnityEngine.Color.yellow;
            else
                TipLabelColor = UnityEngine.Color.green;
        }
    }
}