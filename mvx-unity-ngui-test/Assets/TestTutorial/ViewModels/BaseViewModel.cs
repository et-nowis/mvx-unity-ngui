using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using TestTutorial.Models;
using Cirrious.MvvmCross.Unity.Views;

namespace TestTutorial.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel, IDisposable
    {

        ~BaseViewModel()
        {
            Dispose(false);
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
                foreach (IDisposable disposable in _disposables)
                {
                    disposable.Dispose();
                }
                _disposables.Clear();
            }
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> property, T oldValue, T newValue)
        {
            var name = this.GetPropertyNameFromExpression(property);
            RaisePropertyChanged(name, oldValue, newValue);
        }

        protected void RaisePropertyChanged<T>(string whichProperty, T oldValue, T newValue)
        {
            RaisePropertyChanged(new PropertyChangedExtendedEventArgs<T>(whichProperty, oldValue, newValue));
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

        private IMvxMessenger MvxMessenger
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

        public virtual ICommand CloseCommand
        {
            get
            {
                return new MvxCommand(() => Close(this));
            }
        }

    }
}
