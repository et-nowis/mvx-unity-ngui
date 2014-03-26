// MvxImageViewWrapper.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
//using MonoTouch.UIKit;
using UnityEngine;

namespace Cirrious.MvvmCross.Binding.Unity.Views
{
	public class MvxImageViewWrapper
		: IDisposable
	{
		private readonly Func<UITexture> _imageView;
		private readonly IMvxImageHelper<Texture2D> _imageHelper;
		
		public string ImageUrl
		{
			get { return _imageHelper.ImageUrl; }
			set { _imageHelper.ImageUrl = value; }
		}
		
		public string DefaultImagePath
		{
			get { return _imageHelper.DefaultImagePath; }
			set { _imageHelper.DefaultImagePath = value; }
		}
		
		public string ErrorImagePath
		{
			get { return _imageHelper.ErrorImagePath; }
			set { _imageHelper.ErrorImagePath = value; }
		}
		
		public MvxImageViewWrapper(Func<UITexture> imageView)
		{
			_imageView = imageView;
			_imageHelper = Mvx.Resolve<IMvxImageHelper<Texture2D>>();
			_imageHelper.ImageChanged += ImageHelperOnImageChanged;
		}
		
		~MvxImageViewWrapper()
		{
			Dispose(false);
		}
		
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		private void ImageHelperOnImageChanged(object sender, MvxValueEventArgs<Texture2D> mvxValueEventArgs)
		{
			var imageView = _imageView();
			if (imageView != null && mvxValueEventArgs.Value != null)
				imageView.mainTexture = mvxValueEventArgs.Value;
		}
		
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_imageHelper.Dispose();
			}
		}
	}
}