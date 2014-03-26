// MvxImageViewLoader.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding.Views;
using UnityEngine;

namespace Cirrious.MvvmCross.Binding.Unity.Views
{
	public class MvxImageViewLoader
		: MvxBaseImageViewLoader<Texture2D>
	{
		public MvxImageViewLoader(Func<UITexture> imageViewAccess, Action afterImageChangeAction = null)
			: base((image) =>
			       {
				OnImage(imageViewAccess(), image);
				if (afterImageChangeAction != null)
					afterImageChangeAction();
			})
		{
		}
		
		private static void OnImage(UITexture imageView, Texture2D image)
		{
			if (imageView != null && image != null)
				imageView.mainTexture = image;
		}
	}
}