using System;
using System.ComponentModel;
using System.Reflection;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.ViewModels;

namespace System.Windows.Interactivity
{
    public class InteractionRequestTrigger : IDisposable
    {
        private IDisposable _subscription;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public event EventHandler<InteractionRequestedEventArgs> Actions;

        private EventInfo _eventInfo;
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EventInfo GetEvent()
        {
            return _eventInfo ?? (_eventInfo = GetType().GetEvent("Actions"));
        }

        public IInteractionRequest AssociatedObject { get; private set; }

        public void Attach(IInteractionRequest request)
        {
            AssociatedObject = request;
            OnAttached();
        }

        protected virtual void OnAttached()
        {
            _subscription = new MvxWeakEventSubscription<IInteractionRequest, InteractionRequestedEventArgs>(
                AssociatedObject,
                AssociatedObject.GetType().GetEvent("Raised"),
                InvokeActions);
        }

        public void Detach()
        {
            OnDetaching();
            AssociatedObject = null;
        }

        protected virtual void OnDetaching()
        {
            Dispose();
        }

        protected void InvokeActions(object sender, InteractionRequestedEventArgs e)
        {
            var handler = Actions;
            if (handler != null) handler(sender, e);
        }

        ~InteractionRequestTrigger()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }
            }
        }
    }

    public static class InteractionRequestTriggerExtensionMethods
    {
        public static MvxWeakEventSubscription<InteractionRequestTrigger, InteractionRequestedEventArgs> WeakSubscribe(
            this InteractionRequestTrigger source,
            EventHandler<InteractionRequestedEventArgs> eventHandler)
        {
            return new MvxWeakEventSubscription<InteractionRequestTrigger, InteractionRequestedEventArgs>(
                source, source.GetEvent(), eventHandler);
        }
    }
}
