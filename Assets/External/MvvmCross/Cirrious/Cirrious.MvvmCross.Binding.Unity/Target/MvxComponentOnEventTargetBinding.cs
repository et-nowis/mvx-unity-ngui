// MvxComponentOnEventTargetBinding.cs
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
using System.Windows.Input;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.Plugins.Sound;
using Cirrious.CrossCore;
using UnityEngine;

namespace Cirrious.MvvmCross.Binding.Unity.Target
{
    public class MvxComponentOnEventTargetBinding : MvxTargetBinding
    {
        private ICommand _currentCommand;

        private string _eventName;
        private UIEventListener _eventTarget;

        public MvxComponentOnEventTargetBinding(Component component, string eventName)
            : base(component)
        {
            _eventName = eventName;
            if (component == null)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - Component is null in MvxComponentOnEventTargetBinding");
            }
            else
            {
                _eventTarget = UIEventListener.Get(component.gameObject);
                switch (_eventName)
                {
                    case "onClick":
                        _eventTarget.onClick += this.OnClick;
                        break;
                    case "onPress":
                        _eventTarget.onPress += this.OnPress;
                        break;
					case "onDrag":
						_eventTarget.onDrag += this.OnDrag;
						break;
					case "onDrop":
						_eventTarget.onDrop += this.OnDrop;
						break;
					case "onSelect":
						_eventTarget.onSelect += this.OnSelect;
						break;
                    case "onSubmit":
                        _eventTarget.onSubmit += this.OnSubmit;
                        break;

                }
            }
        }

        public void OnClick(GameObject go)
        {
            if (Target != null && _currentCommand != null)
            {
				IMvxSound sound;
			
				if ( Mvx.TryResolve<IMvxSound>(out sound) )
				{
					sound.Play("ButtonClick");
				}
				
                _currentCommand.Execute(null);
            }
        }

        public void OnPress(GameObject go, bool state)
        {
            if (Target != null && _currentCommand != null)
            {
                _currentCommand.Execute(state);
            }
        }
		
		public void OnDrag(GameObject go, Vector2 delta)
        {
            if (Target != null && _currentCommand != null)
            {
                _currentCommand.Execute(delta);
            }
        }
		
		public void OnDrop(GameObject go, GameObject draggedObject)
        {
            if (Target != null && _currentCommand != null)
            {
                _currentCommand.Execute(draggedObject);
            }
        }
		
		public void OnSelect(GameObject go, bool selected)
        {
            if (Target != null && _currentCommand != null)
            {
                _currentCommand.Execute(selected);
            }
        }

        public void OnSubmit(GameObject go)
        {
            if (Target != null && _currentCommand != null)
            {
                _currentCommand.Execute(null);
            }
        }

        public override Type TargetType
        {
            get { return typeof(ICommand); }
        }

        public override MvxBindingMode DefaultMode
        {
            get { return MvxBindingMode.OneWay; }
        }


        public override void SetValue(object value)
        {
            var command = value as ICommand;
            _currentCommand = command;
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_eventTarget != null)
                {
                    switch (_eventName)
                    {
                        case "onClick":
                            _eventTarget.onClick -= this.OnClick;
                            break;
                        case "onPress":
                            _eventTarget.onPress -= this.OnPress;
                            break;
						case "onDrag":
                            _eventTarget.onDrag -= this.OnDrag;
                            break;
						case "onDrop":
                            _eventTarget.onDrop -= this.OnDrop;
                            break;	
						case "onSelect":
                            _eventTarget.onSelect -= this.OnSelect;
                            break;
                    }
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
