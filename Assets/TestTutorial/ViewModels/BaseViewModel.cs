using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Unity.Views;
using Cirrious.MvvmCross.Plugins.Messenger;
using Cirrious.MvvmCross.ViewModels;
using UnityEngine;

namespace TestTutorial.ViewModels
{
    public abstract class BaseViewModel : MvxViewModel, IDisposable
    {
		protected bool IsClosed { get; set; }
		
		protected bool _updateUIProcess;
		
		private MvxSubscriptionToken _onClosePanelToken;

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
		
		protected void RaiseEvent<T>(EventHandler<MvxValueEventArgs<T>> eventHandler, T value)
		{
			// check for subscription before going multithreaded
			if (eventHandler == null) return;
			
			InvokeOnMainThread(
				() => eventHandler.Raise<T>(this, value)
			);
		}
		
		protected void RaiseRequest<T>(InteractionRequest<T> request, T context, Action<T> callback)
		{
			InvokeOnMainThread(
				() => request.Raise(context, callback)
			);
		}
		
		protected void RaiseRequest<T>(InteractionRequest<T> request, T context)
		{
			InvokeOnMainThread(
				() => request.Raise(context)
			);
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
		
		private IMvxMessenger _mvxMessenger;
		public IMvxMessenger MvxMessenger
		{
			get
			{
				if (_mvxMessenger == null)
				{
					_mvxMessenger = Mvx.Resolve<IMvxMessenger>();
				}
				return _mvxMessenger;
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
		
		private ICommand _closeCommand;
		public ICommand CloseCommand
		{
			get
			{
				return _closeCommand ?? (_closeCommand = new MvxCommand(() =>
				{
					if (!IsClosed && DoClose())
					{
						IsClosed = true;
					}
				}));
			}
		}
		
//		public void SubscribeClosePanelMessage()
//		{
//			_onClosePanelToken = Subscribe<ClosePanelMessage>(OnClosePanelMessage);
//		}
		
//		private void OnClosePanelMessage(ClosePanelMessage message)
//		{
//			DoClose();
//		}
		
		protected virtual bool DoClose()
		{
//			if (_onClosePanelToken != null)
//				Unsubscribe(_onClosePanelToken);
			
			Close(this);
			
			return true;
		}
		
		public class BaseViewModelParameters
		{
			public bool UpdateUIProcess { get; set; }
		}
		
		protected bool ShowViewModel<TViewModel>(BaseViewModelParameters parameterValuesObject, bool updateUIProcess = false)
			where TViewModel : IMvxViewModel
		{
//			_updateUIProcess = updateUIProcess;
//			
//			if (_updateUIProcess)
//			{
//				UpdateUIPanelOpened(true);
//			}
			
			return base.ShowViewModel<TViewModel>(parameterValuesObject);
		}
		
		protected bool ShowViewModel<TViewModel>(bool updateUIProcess = false)
			where TViewModel : IMvxViewModel
		{
//			_updateUIProcess = updateUIProcess;
//			
//			if (_updateUIProcess)
//			{
//				UpdateUIPanelOpened(true);
//			}
//			
			return base.ShowViewModel<TViewModel>();
		}
		
//		protected void UpdateUIPanelOpened(bool flag)
//		{
//			var userModel = Mvx.Resolve<IUserModel>();
//			userModel.UpdateUIPanelOpened(flag);
//			Mvx.Resolve<GameLogService>().Log("{0} UpdateUIPanelOpened {1}", GetType().Name, flag);
//		}
//		
//		protected void UpdateEditMode(bool flag)
//		{
//			var userModel = Mvx.Resolve<IUserModel>();
//			userModel.UpdateEditMode(flag);
//			//Mvx.Resolve<GameLogService>().Log("{0} UpdateEditMode {1}", GetType().Name, flag);
//		}
//		
//		protected void UpdateMapCutsceneStarted(bool flag)
//		{
//			var userModel = Mvx.Resolve<IUserModel>();
//			userModel.UpdateMapCutsceneStarted(flag);
//			Mvx.Resolve<GameLogService>().Log("{0} UpdateMapCutsceneStarted {1}", GetType().Name, flag);
//		}
//		
//		protected void UpdatePanelHideMainMenu(bool flag)
//		{
//			var userModel = Mvx.Resolve<IUserModel>();
//			userModel.UpdatePanelHideMainMenu(flag);
//			Mvx.Resolve<GameLogService>().Log("{0} UpdatePanelHideMainMenu {1}", GetType().Name, flag);
//		}
//		
//		protected void UpdateFactoryPanelOpened(bool flag)
//		{
//			var userModel = Mvx.Resolve<IUserModel>();
//			userModel.UpdateFactoryPanelOpened(flag);
//			Mvx.Resolve<GameLogService>().Log("{0} UpdateFactoryPanelOpened {1}", GetType().Name, flag);
//		}
//		
		//public virtual ICommand CloseAllCommand
		//{
		//    get
		//    {
		//        return new MvxCommand(CloseAll);
		//    }
		//}
		
		//public event EventHandler Closed;
		
		//protected void Close()
		//{
		//    UnityEngine.Debug.Log("Close = " + this);
		//    //if (Closed != null) Closed(this, EventArgs.Empty);
		
		//    MvxClosePresentationHint hint = new MvxClosePresentationHint(this);
		
		//    ChangePresentation(hint);
		//}
		
		//protected void CloseAll()
		//{
		//    UnityEngine.Debug.Log("CloseAll = " + this);
		//    //if (Closed != null) Closed(this, EventArgs.Empty);
		
		//    MvxCloseAllPresentationHint hint = new MvxCloseAllPresentationHint(this);
		
		//    ChangePresentation(hint);
		//}
		
		//protected void ClearBackStack()
		//{
		//    UnityEngine.Debug.Log("ClearBackStack = " + this);
		//    //if (Closed != null) Closed(this, EventArgs.Empty);
		
		//    MvxClearBackStackPresentationHint hint = new MvxClearBackStackPresentationHint();
		
		//    ChangePresentation(hint);
		//}
		
		//protected void Close()
		//{
		//    CloseHint hint = new CloseHint();
		
		//    hint.ViewModelToClose = this;
		
		//    ChangePresentation(hint);
		
		//}

    }
}

