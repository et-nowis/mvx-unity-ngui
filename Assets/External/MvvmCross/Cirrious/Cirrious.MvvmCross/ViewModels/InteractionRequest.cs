using System;
using System.Windows.Interactivity;

namespace Cirrious.MvvmCross.ViewModels
{
    public class InteractionRequest<T> : IInteractionRequest
    {
        public event EventHandler<InteractionRequestedEventArgs> Raised;

        public void Raise(T context)
        {
            Raise(context, null);
        }

        public void Raise(T context, Action<T> callback)
        {
            var handler = Raised;
            if (handler != null)
            {
                handler(
                    this,
                    new InteractionRequestedEventArgs<T>(
                        context,
                        () => { if (callback != null) callback(context); }));
            }
        }
    }

    public class InteractionRequest : InteractionRequest<object>
    {
    }
}
