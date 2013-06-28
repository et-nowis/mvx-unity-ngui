// MvxUISliderSliderValueTargetBinding.cs
//
// Copyright (c) 2013 Frenzoo Ltd.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;

namespace Cirrious.MvvmCross.Binding.Unity.Target
{
    public class MvxUISliderSliderValueTargetBinding : MvxPropertyInfoTargetBinding<UISlider>
    {
        FieldInfo _onValueChangeFieldInfo;
        Delegate _onValueChangedDelegate;
        
        public MvxUISliderSliderValueTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var slider = View;
            if (slider == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - UISlider is null in MvxUISliderSliderValueTargetBinding");
            }
            else
            {
                if ((_onValueChangeFieldInfo = typeof(UISlider).GetField("onValueChange")) != null)
                {
                    //slider.onValueChange += OnValueChanged;
                    Delegate onValueChange = _onValueChangeFieldInfo.GetValue(slider) as Delegate;
                    _onValueChangedDelegate = Delegate.CreateDelegate(_onValueChangeFieldInfo.FieldType, this, "OnValueChanged");
                    _onValueChangeFieldInfo.SetValue(slider, Delegate.Combine(onValueChange, _onValueChangedDelegate));
                }
                else
                {
                    slider.eventReceiver = slider.gameObject;
                    MvxUISliderOnSliderChangeEventHandler handler = slider.GetComponent<MvxUISliderOnSliderChangeEventHandler>();
                    if (handler == null)
                        handler = slider.gameObject.AddComponent<MvxUISliderOnSliderChangeEventHandler>();
                    handler.onValueChange += OnValueChanged;
                }
            }
        }

        private void OnValueChanged(float val)
        {
            var slider = View;
            if (slider != null)
            {
                FireValueChanged(val);
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (isDisposing)
            {
                var slider = View;
                if (slider != null)
                {
                    if (_onValueChangeFieldInfo != null)
                    {
                        //slider.onValueChange -= OnValueChanged;
                        Delegate onValueChange = _onValueChangeFieldInfo.GetValue(slider) as Delegate;
                        _onValueChangeFieldInfo.SetValue(slider, Delegate.Remove(onValueChange, _onValueChangedDelegate));
                    }
                    else
                    {
                        MvxUISliderOnSliderChangeEventHandler handler = slider.GetComponent<MvxUISliderOnSliderChangeEventHandler>();
                        if (handler != null)
                            handler.onValueChange -= OnValueChanged;
                    }
                }
            }
        }
    }
}
