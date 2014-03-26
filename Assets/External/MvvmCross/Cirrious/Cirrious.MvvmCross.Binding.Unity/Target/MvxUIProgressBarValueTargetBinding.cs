// MvxUIProgressBarValueTargetBinding.cs
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

#if NGUI_3

using System;
using System.Reflection;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;

namespace Cirrious.MvvmCross.Binding.Unity.Target
{
    public class MvxUIProgressBarValueTargetBinding : MvxPropertyInfoTargetBinding<UIProgressBar>
    {
        public MvxUIProgressBarValueTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var progressBar = View;
            if (progressBar == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - UIProgressBar is null in MvxUIProgressBarValueTargetBinding");
            }
            else
            {
                EventDelegate.Add(progressBar.onChange, OnValueChanged);
            }
        }

        private void OnValueChanged()
        {
            var progressBar = View;
            if (progressBar != null)
            {
                FireValueChanged(UIProgressBar.current.value);
            }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.TwoWay; }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                var progressBar = View;
                if (progressBar != null)
                {
                    EventDelegate.Remove(progressBar.onChange, OnValueChanged);
                }
            }
            base.Dispose(isDisposing);
        }
    }
}

#endif
