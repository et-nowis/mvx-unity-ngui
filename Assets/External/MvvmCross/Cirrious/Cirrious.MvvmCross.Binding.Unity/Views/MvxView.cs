// MvxView.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.ViewModels;
using UnityEngine;

namespace Cirrious.MvvmCross.Binding.Unity.Views
{
    public class MvxView
        : MonoBehaviour
          , IMvxBindable
		  , IDisposable
    {
        public IMvxBindingContext BindingContext { get; set; }
		
		protected virtual void Awake()
		{
			this.CreateBindingContext();
		}
		
      	protected void OnDestroy()
	    {
	        this.Dispose();
	    }
	
	    public void Dispose()
	    {
	        Dispose(true);
	        GC.SuppressFinalize(this);
	    }
	
	    protected virtual void Dispose(bool disposing)
	    {
			if (disposing)
            {
                BindingContext.ClearAllBindings();
            }
	    }
		
	    [MvxSetToNullAfterBinding]
        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }
		
		public IMvxViewModel ViewModel
        {
            get { return (IMvxViewModel)DataContext; }
            set { DataContext = value; }
        }
    }
}