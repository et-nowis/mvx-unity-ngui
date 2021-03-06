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
#if NGUI_3
                EventDelegate.Add(slider.onChange, OnValueChanged);
#else
                slider.onValueChange += OnValueChanged;
#endif

            }
        }
#if NGUI_3
        private void OnValueChanged()
        {
            var slider = View;
            if (slider != null)
            {
                FireValueChanged(UISlider.current.value);
            }
        }
#else		
        private void OnValueChanged(float val)
        {
            var slider = View;
            if (slider != null)
            {
                FireValueChanged(val);
            }
        }		
#endif
        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var slider = View;
                if (slider != null)
                {
#if NGUI_3
                    EventDelegate.Remove(slider.onChange, OnValueChanged);
#else
					slider.onValueChange -= OnValueChanged;
#endif
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
