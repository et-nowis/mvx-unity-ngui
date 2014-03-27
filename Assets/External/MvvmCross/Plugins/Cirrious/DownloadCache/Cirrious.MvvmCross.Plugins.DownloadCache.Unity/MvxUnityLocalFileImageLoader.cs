// MvxTouchLocalFileImageLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.File;
//using MonoTouch.Foundation;
//using MonoTouch.UIKit;
using UnityEngine;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Unity
{
	public class MvxUnityLocalFileImageLoader
        : IMvxLocalFileImageLoader<Texture2D>    
    {
		private const string ResourcePrefix = "Resources/";

        public MvxImage<Texture2D> Load(string localPath, bool shouldCache)
        {
			Texture2D uiImage;
            if (localPath.StartsWith(ResourcePrefix))
			{
				var resourcePath = localPath.Substring(ResourcePrefix.Length);
				uiImage = LoadResourceImage(resourcePath, shouldCache);
			}
			else
			{
				uiImage = LoadTexture2D(localPath);
			}
            return new MvxUnityImage(uiImage);
        }

		private Texture2D LoadTexture2D(string localPath)
        {
            var file = Mvx.Resolve<IMvxFileStore>();
            byte[] data = null;
            if (!file.TryReadBinaryFile(localPath, stream =>
                {
                    var memoryStream = new System.IO.MemoryStream();
                    stream.CopyTo(memoryStream);
                    data = memoryStream.GetBuffer();
                    return true;
                }))
                return null;

//            var imageData = NSData.FromArray(data);
//            return UIImage.LoadFromData(imageData);

			Texture2D tex = new Texture2D(4, 4);
			tex.LoadImage(data);
			return tex;
        }

		private Texture2D LoadResourceImage(string resourcePath, bool shouldCache)
		{
//			if (shouldCache)
//				return UIImage.FromFile(resourcePath);
//			else
//				return UIImage.FromFileUncached(resourcePath);

			if (shouldCache)
				return Resources.Load(resourcePath, typeof(Texture2D)) as Texture2D;
			else
				return Resources.Load(resourcePath, typeof(Texture2D)) as Texture2D;
		}
    }
}