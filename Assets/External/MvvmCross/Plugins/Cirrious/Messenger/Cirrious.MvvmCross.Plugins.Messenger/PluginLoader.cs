﻿// PluginLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore.Plugins;
using Cirrious.CrossCore;

namespace Cirrious.MvvmCross.Plugins.Messenger
{
    public class PluginLoader
        : IMvxPluginLoader
    {
        public static readonly PluginLoader Instance = new PluginLoader();

        //private bool _loaded;

        public void EnsureLoaded()
        {
            if (Mvx.CanResolve<IMvxMessenger>())
            //if (_loaded)
            {
                return;
            }

            Mvx.RegisterSingleton<IMvxMessenger>(new MvxMessengerHub());
            //_loaded = true;
        }
    }
}