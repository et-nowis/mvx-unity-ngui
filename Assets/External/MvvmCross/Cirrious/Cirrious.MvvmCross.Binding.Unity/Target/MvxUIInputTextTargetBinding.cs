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
    public class MvxUIInputTextTargetBinding : MvxPropertyInfoTargetBinding<UIInput>
    {
        public MvxUIInputTextTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var input = View;
            if (input == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - UIInput is null in MvxUIInputTextTargetBinding");
            }
            else
            {
#if NGUI_3
				EventDelegate.Add( input.onChange, OnValueChanged );
#else
                input.onInputChanged += OnValueChanged;
#endif
            }
        }

#if NGUI_3
        private void OnValueChanged()
        {
            var input = View;
            if (input != null)
            {
                FireValueChanged(UIInput.current.value);
            }
        }
#else		
		private void OnValueChanged(string val)
        {
            var input = View;
            if (input != null)
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
                var input = View;
                if (input != null)
                {
#if NGUI_3
					EventDelegate.Remove( input.onChange, OnValueChanged );
#else
                    input.onInputChanged -= OnValueChanged;
#endif
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
