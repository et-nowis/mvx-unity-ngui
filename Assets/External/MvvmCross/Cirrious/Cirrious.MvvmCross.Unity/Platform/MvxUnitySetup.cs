// MvxUnitySetup.cs
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
using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
using Cirrious.CrossCore.Unity.Views;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Bindings.Target.Construction;
using Cirrious.MvvmCross.Binding.Unity;
using Cirrious.MvvmCross.Platform;
using Cirrious.MvvmCross.Unity.Views;
using Cirrious.MvvmCross.Unity.Views.Presenters;
using Cirrious.MvvmCross.Views;

namespace Cirrious.MvvmCross.Unity.Platform
{
    public abstract class MvxUnitySetup
        : MvxSetup
    {
        private readonly MvxApplicationDelegate _applicationDelegate;

        private IMvxUnityViewPresenter _presenter;

        protected MvxUnitySetup(MvxApplicationDelegate applicationDelegate)
        {
            _applicationDelegate = applicationDelegate;
        }

        protected MvxUnitySetup(MvxApplicationDelegate applicationDelegate, IMvxUnityViewPresenter presenter)
        {
            _applicationDelegate = applicationDelegate;
            _presenter = presenter;
        }

        protected MvxApplicationDelegate ApplicationDelegate
        {
            get { return _applicationDelegate; }
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new MvxDebugTrace();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            var toReturn = new MvxLoaderPluginManager();
            var registry = new MvxLoaderPluginRegistry(".Unity", toReturn.Finders);
            AddPluginsLoaders(registry);
            return toReturn;
        }

        protected virtual void AddPluginsLoaders(MvxLoaderPluginRegistry loaders)
        {
            // none added by default
        }

        protected override sealed MvxViewsContainer CreateViewsContainer()
        {
            var container = new MvxUnityViewsContainer();
            RegisterUnityViewCreator(container);
            return container;
        }

        protected virtual void RegisterUnityViewCreator(MvxUnityViewsContainer container)
        {
            Mvx.RegisterSingleton<IMvxUnityViewCreator>(container);
            Mvx.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxUnityViewDispatcher(Presenter);
        }

        protected override void InitializePlatformServices()
        {
            RegisterPlatformProperties();
            // for now we continue to register the old style platform properties
            RegisterOldStylePlatformProperties();
            RegisterPresenter();
        }

        protected virtual void RegisterPlatformProperties()
        {
        }

        [Obsolete("In the future I expect to see something implemented in the core project for this functionality - including something that can be called statically during startup")]
        protected virtual void RegisterOldStylePlatformProperties()
        {
            Mvx.RegisterSingleton<IMvxUnityPlatformProperties>(new MvxUnityPlatformProperties());
        }

        protected IMvxUnityViewPresenter Presenter
        {
            get
            {
                _presenter = _presenter ?? CreatePresenter();
                return _presenter;
            }
        }

        protected virtual IMvxUnityViewPresenter CreatePresenter()
        {
            return new MvxUnityViewPresenter(_applicationDelegate);
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = Presenter;
            Mvx.RegisterSingleton(presenter);
        }

        protected override void InitializeLastChance()
        {
            InitialiseBindingBuilder();
            base.InitializeLastChance();
        }

        protected virtual void InitialiseBindingBuilder()
        {
            RegisterBindingBuilderCallbacks();
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration();
        }

        protected override void PerformBootstrapActions()
        {

        }

        protected virtual void RegisterBindingBuilderCallbacks()
        {
            Mvx.CallbackWhenRegistered<IMvxValueConverterRegistry>(FillValueConverters);
            Mvx.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(FillTargetFactories);
            Mvx.CallbackWhenRegistered<IMvxBindingNameRegistry>(FillBindingNames);
        }

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            var bindingBuilder = new MvxUnityBindingBuilder();
            return bindingBuilder;
        }

        protected virtual void FillBindingNames(IMvxBindingNameRegistry obj)
        {
            // this base class does nothing
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual List<Type> ValueConverterHolders
        {
            get { return new List<Type>(); }
        }

        protected virtual List<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(GetViewModelAssemblies());
                toReturn.AddRange(GetViewAssemblies());
                toReturn.Add(typeof(Cirrious.MvvmCross.Localization.MvxLanguageConverter).Assembly);
                return toReturn;
            }
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }
    }
}