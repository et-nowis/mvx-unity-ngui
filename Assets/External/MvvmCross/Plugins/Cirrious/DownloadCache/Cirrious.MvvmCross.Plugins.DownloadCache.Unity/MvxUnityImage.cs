// MvxTouchImage.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

//using MonoTouch.UIKit;
using UnityEngine;

namespace Cirrious.MvvmCross.Plugins.DownloadCache.Unity
{
    public class MvxUnityImage
        : MvxImage<Texture2D>
    {
		public MvxUnityImage(Texture2D rawImage)
            : base(rawImage)
        {
        }

        public override int GetSizeInBytes()
        {
            if (RawImage == null)
                return 0;

//            var cg = RawImage.CGImage;
//            return cg.BytesPerRow*cg.Height;

			return (int)(RawImage.texelSize.x * RawImage.texelSize.y);
        }
    }
}