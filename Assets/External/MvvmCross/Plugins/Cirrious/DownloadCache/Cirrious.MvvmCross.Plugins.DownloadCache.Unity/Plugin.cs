// Plugin.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Plugins;
//using MonoTouch.UIKit;
using UnityEngine;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Unity
{
#warning One day I would like to decouple this plugin from the FileStore plugin
    public class Plugin
        : IMvxConfigurablePlugin
    {
        private MvxDownloadCacheConfiguration _configuration;

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration != null && !(configuration is MvxDownloadCacheConfiguration))
            {
                throw new MvxException("You must use a MvxDownloadCacheConfiguration object for configuring the DownloadCache, but you supplied {0}", configuration.GetType().Name);
            }
            _configuration = (MvxDownloadCacheConfiguration)configuration;
        }

        public void Load()
        {
            Mvx.RegisterSingleton<IMvxHttpFileDownloader>(() => new MvxHttpFileDownloader());
            Mvx.RegisterSingleton<IMvxImageCache<Texture2D>>(() => CreateCache());
			Mvx.RegisterType<IMvxImageHelper<Texture2D>, MvxDynamicImageHelper<Texture2D>>();
			Mvx.RegisterSingleton<IMvxLocalFileImageLoader<Texture2D>>(() => new MvxUnityLocalFileImageLoader());
        }

        private MvxImageCache<Texture2D> CreateCache()
        {
            var configuration = _configuration ?? MvxDownloadCacheConfiguration.Default;

            var fileDownloadCache = new MvxFileDownloadCache(configuration.CacheName,
                                                             configuration.CacheFolderPath,
                                                             configuration.MaxFiles,
                                                             configuration.MaxFileAge);
			var fileCache = new MvxImageCache<Texture2D>(fileDownloadCache, configuration.MaxInMemoryFiles, configuration.MaxInMemoryBytes);
            return fileCache;
        }
    }
}