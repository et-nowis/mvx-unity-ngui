using System;

namespace System.Windows.Interactivity
{
    public class InteractionRequestedEventArgs : EventArgs
    {
        public object Context { get; set; }
        public Action Callback { get; set; }

        public InteractionRequestedEventArgs(object context, Action callback)
        {
            Context = context;
            Callback = callback;
        }
    }

    public class InteractionRequestedEventArgs<T> : InteractionRequestedEventArgs
    {
        public new T Context { get { return (T)base.Context; } set { base.Context = value; } }

        public InteractionRequestedEventArgs(T context, Action callback)
            : base(context, callback)
        {
        }
    }

}
