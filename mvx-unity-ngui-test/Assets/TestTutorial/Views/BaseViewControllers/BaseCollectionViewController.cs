using System;
using System.Collections.Generic;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.Unity.Views;

public abstract class BaseCollectionViewController
    : MvxCollectionViewController
{

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
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

    public MvxSubscriptionToken Subscribe<TMessage>(Action<TMessage> action)
        where TMessage : MvxMessage
    {
        return MvxMessenger.Subscribe<TMessage>(action, MvxReference.Weak);
    }

    public MvxSubscriptionToken SubscribeOnMainThread<TMessage>(Action<TMessage> action)
        where TMessage : MvxMessage
    {
        return MvxMessenger.SubscribeOnMainThread<TMessage>(action, MvxReference.Weak);
    }

    public void Unsubscribe<TMessage>(MvxSubscriptionToken id)
        where TMessage : MvxMessage
    {
        MvxMessenger.Unsubscribe<TMessage>(id);
    }

}
