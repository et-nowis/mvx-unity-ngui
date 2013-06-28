// MvxUICheckboxIsCheckedTargetBinding.cs
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
    public class MvxUICheckboxIsCheckedTargetBinding : MvxPropertyInfoTargetBinding<UICheckbox>
    {
        FieldInfo _onStateChangeFieldInfo;
        Delegate _onStateChangedDelegate;

        public MvxUICheckboxIsCheckedTargetBinding(object target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var checkbox = View;
            if (checkbox == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - UICheckbox is null in MvxUICheckboxIsCheckedTargetBinding");
            }
            else
            {
                if ((_onStateChangeFieldInfo = typeof(UICheckbox).GetField("onStateChange")) != null)
                {
                    //checkbox.onStateChange += OnStateChanged;
                    Delegate onStateChange = _onStateChangeFieldInfo.GetValue(checkbox) as Delegate;
                    _onStateChangedDelegate = Delegate.CreateDelegate(_onStateChangeFieldInfo.FieldType, this, "OnStateChanged");
                    _onStateChangeFieldInfo.SetValue(checkbox, Delegate.Combine(onStateChange, _onStateChangedDelegate));
                }
                else
                {
                    checkbox.eventReceiver = checkbox.gameObject;
                    MvxUICheckboxOnActivateEventHandler handler = checkbox.GetComponent<MvxUICheckboxOnActivateEventHandler>();
                    if (handler == null)
                        handler = checkbox.gameObject.AddComponent<MvxUICheckboxOnActivateEventHandler>();
                    handler.onStateChange += OnStateChanged;
                }
            }
        }

        public void OnStateChanged(bool state)
        {
            var checkbox = View;
            if (checkbox != null)
            {
                FireValueChanged(state);
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
                var checkbox = View;
                if (checkbox != null)
                {
                    if (_onStateChangeFieldInfo != null)
                    {
                        //checkbox.onStateChange -= OnStateChanged;
                        Delegate onStateChange = _onStateChangeFieldInfo.GetValue(checkbox) as Delegate;
                        _onStateChangeFieldInfo.SetValue(checkbox, Delegate.Remove(onStateChange, _onStateChangedDelegate));
                    }
                    else
                    {
                        MvxUICheckboxOnActivateEventHandler handler = checkbox.GetComponent<MvxUICheckboxOnActivateEventHandler>();
                        if (handler != null)
                            handler.onStateChange -= OnStateChanged;
                    }
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
