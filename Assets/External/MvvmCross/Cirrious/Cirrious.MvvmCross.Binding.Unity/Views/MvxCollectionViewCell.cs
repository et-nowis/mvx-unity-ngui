// MvxCollectionViewCell.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Binders;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.CrossCore.Unity.Views;
using Cirrious.CrossCore;
using UnityEngine;

namespace Cirrious.MvvmCross.Binding.Unity.Views
{
    public class MvxCollectionViewCell
        : UICollectionViewCell
          , IMvxBindable
    {
        public IMvxBindingContext BindingContext { get; set; }

		protected override void Awake()
		{
			this.CreateBindingContext();
		}
		
	    protected override void Dispose(bool disposing)
	    {
	        if (disposing)
	        {	  
                BindingContext.ClearAllBindings();
	            foreach (IDisposable disposable in _disposables)
	            {
	                disposable.Dispose();
	            }
	            _disposables.Clear();
	        }
	        base.Dispose(disposing);
	    }
	
	    private List<IDisposable> _disposables = new List<IDisposable>();
	
	    protected void AddDisposable(IDisposable disposable)
	    {
	        _disposables.Add(disposable);
	    }
	
	    protected void RemoveDisposable(IDisposable disposable)
	    {
	        _disposables.Remove(disposable);
	    }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }
		
		protected IMvxMessenger MvxMessenger
	    {
	        get
	        {
	            return Mvx.Resolve<IMvxMessenger>();
	        }
	    }
	
	    public void Publish(MvxMessage message)
	    {
	        MvxMessenger.Publish(message);
	    }
	
	    public MvxSubscriptionToken Subscribe<TMessage>(Action<TMessage> action, string tag = null)
	        where TMessage : MvxMessage
	    {
	        var token = MvxMessenger.Subscribe<TMessage>(action, MvxReference.Weak, tag);
	        AddDisposable(token);
	        return token;
	    }
	
	    public MvxSubscriptionToken SubscribeOnMainThread<TMessage>(Action<TMessage> action, string tag = null)
	        where TMessage : MvxMessage
	    {
	        var token = MvxMessenger.SubscribeOnMainThread<TMessage>(action, MvxReference.Weak, tag);
	        AddDisposable(token);
	        return token;
	    }
	
	    public void Unsubscribe(MvxSubscriptionToken token)
	    {
	        RemoveDisposable(token);
	        token.Dispose();
	    }
		
		public IMvxViewModel ViewModel
        {
            get { return (IMvxViewModel)DataContext; }
            set { DataContext = value; }
        }
    }
}