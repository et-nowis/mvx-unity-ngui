// MvxUnityBindingBuilder.cs
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
using System.Windows.Interactivity;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Unity.Target;
using Cirrious.MvvmCross.Binding.Unity.Views;
using UnityEngine;

namespace Cirrious.MvvmCross.Binding.Unity
{
    public class MvxUnityBindingBuilder
        : MvxBindingBuilder
    {
        private readonly Action<IMvxTargetBindingFactoryRegistry> _fillRegistryAction;
        private readonly Action<IMvxValueConverterRegistry> _fillValueConvertersAction;
        private readonly Action<IMvxBindingNameRegistry> _fillBindingNamesAction;

        public MvxUnityBindingBuilder(Action<IMvxTargetBindingFactoryRegistry> fillRegistryAction = null,
                                      Action<IMvxValueConverterRegistry> fillValueConvertersAction = null,
                                      Action<IMvxBindingNameRegistry> fillBindingNamesAction = null)
        {
            _fillRegistryAction = fillRegistryAction;
            _fillValueConvertersAction = fillValueConvertersAction;
            _fillBindingNamesAction = fillBindingNamesAction;
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            base.FillTargetFactories(registry);

#if NGUI_3
            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUIProgressBarValueTargetBinding), typeof(UIProgressBar), "value");
            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUISliderSliderValueTargetBinding), typeof(UISlider), "value");
            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUIToggleIsCheckedTargetBinding), typeof(UIToggle), "isChecked");
#else
            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUISliderSliderValueTargetBinding), typeof(UISlider), "sliderValue");
            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUICheckboxIsCheckedTargetBinding), typeof(UICheckbox), "isChecked");
#endif

            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUIInputTextTargetBinding), typeof(UIInput), "text");

            registry.RegisterCustomBindingFactory<GameObject>("active", (gameObject) => new MvxGameObjectActiveTargetBinding(gameObject));

            registry.RegisterCustomBindingFactory<GameObject>("onClick", (button) => new MvxGameObjectOnEventTargetBinding(button, "onClick"));
            registry.RegisterCustomBindingFactory<GameObject>("onPress", (button) => new MvxGameObjectOnEventTargetBinding(button, "onPress"));
            registry.RegisterCustomBindingFactory<GameObject>("onSelect", (button) => new MvxGameObjectOnEventTargetBinding(button, "onSelect"));
            registry.RegisterCustomBindingFactory<GameObject>("onDrag", (button) => new MvxGameObjectOnEventTargetBinding(button, "onDrag"));
            registry.RegisterCustomBindingFactory<GameObject>("onDrop", (button) => new MvxGameObjectOnEventTargetBinding(button, "onDrop"));
            registry.RegisterCustomBindingFactory<GameObject>("onSubmit", (gameObject) => new MvxGameObjectOnEventTargetBinding(gameObject, "onSubmit"));

            registry.RegisterCustomBindingFactory<Component>("onClick", (button) => new MvxComponentOnEventTargetBinding(button, "onClick"));
            registry.RegisterCustomBindingFactory<Component>("onPress", (button) => new MvxComponentOnEventTargetBinding(button, "onPress"));
            registry.RegisterCustomBindingFactory<Component>("onSelect", (button) => new MvxComponentOnEventTargetBinding(button, "onSelect"));
            registry.RegisterCustomBindingFactory<Component>("onDrag", (button) => new MvxComponentOnEventTargetBinding(button, "onDrag"));
            registry.RegisterCustomBindingFactory<Component>("onDrop", (button) => new MvxComponentOnEventTargetBinding(button, "onDrop"));
            registry.RegisterCustomBindingFactory<Component>("onSubmit", (component) => new MvxComponentOnEventTargetBinding(component, "onSubmit"));

			registry.RegisterCustomBindingFactory<InteractionRequestTrigger>("SourceObject", (trigger) => new MvxTriggerInteractionRequestTargetBinding(trigger));

            if (_fillRegistryAction != null)
                _fillRegistryAction(registry);
        }

        protected override void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            base.FillValueConverters(registry);

            if (_fillValueConvertersAction != null)
                _fillValueConvertersAction(registry);
        }

        protected override void FillDefaultBindingNames(IMvxBindingNameRegistry registry)
        {
            base.FillDefaultBindingNames(registry);

            registry.AddOrOverwrite(typeof(MvxCollectionViewSource), "ItemsSource");
            registry.AddOrOverwrite(typeof(UISprite), "spriteName");
            registry.AddOrOverwrite(typeof(UILabel), "text");
            registry.AddOrOverwrite(typeof(UIInput), "text");
            registry.AddOrOverwrite(typeof(GameObject), "active");

#if NGUI_3
            registry.AddOrOverwrite(typeof(UIProgressBar), "value");
            registry.AddOrOverwrite(typeof(UISlider), "value");
            registry.AddOrOverwrite(typeof(UIToggle), "isChecked");
#else
            registry.AddOrOverwrite(typeof(UISlider), "sliderValue");
            registry.AddOrOverwrite(typeof(UICheckbox), "isChecked");
#endif

            registry.AddOrOverwrite(typeof(UIButton), "onClick");
            registry.AddOrOverwrite(typeof(UIButtonMessage), "onClick");
			
			registry.AddOrOverwrite(typeof(InteractionRequestTrigger), "SourceObject");

            if (_fillBindingNamesAction != null)
                _fillBindingNamesAction(registry);
        }
    }
}
