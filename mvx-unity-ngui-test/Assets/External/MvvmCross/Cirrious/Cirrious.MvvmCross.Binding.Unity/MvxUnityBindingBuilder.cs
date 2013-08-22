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

            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUISliderSliderValueTargetBinding), typeof(UISlider), "sliderValue");
            registry.RegisterPropertyInfoBindingFactory(typeof(MvxUICheckboxIsCheckedTargetBinding), typeof(UICheckbox), "isChecked");
            registry.RegisterCustomBindingFactory<GameObject>("active", (gameObject) => new MvxGameObjectActiveTargetBinding(gameObject));
            registry.RegisterCustomBindingFactory<GameObject>("onClick", (button) => new MvxGameObjectOnEventTargetBinding(button, "onClick"));
            registry.RegisterCustomBindingFactory<GameObject>("onPress", (button) => new MvxGameObjectOnEventTargetBinding(button, "onPress"));
            registry.RegisterCustomBindingFactory<Component>("onClick", (button) => new MvxComponentOnEventTargetBinding(button, "onClick"));
            registry.RegisterCustomBindingFactory<Component>("onPress", (button) => new MvxComponentOnEventTargetBinding(button, "onPress"));

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
            registry.AddOrOverwrite(typeof(UISlider), "sliderValue");
            registry.AddOrOverwrite(typeof(UICheckbox), "isChecked");
            registry.AddOrOverwrite(typeof(UIButton), "onClick");
            registry.AddOrOverwrite(typeof(UIButtonMessage), "onClick");
            registry.AddOrOverwrite(typeof(GameObject), "active");

            if (_fillBindingNamesAction != null)
                _fillBindingNamesAction(registry);
        }
    }
}
