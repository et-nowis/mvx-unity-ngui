// MvxUnityViewPresenter.cs
//
// Copyright (c) 2013 Frenzoo Ltd.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using Cirrious.CrossCore.Exceptions;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore.Unity.Views;
using Cirrious.MvvmCross.Unity.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using UnityEngine;

namespace Cirrious.MvvmCross.Unity.Views.Presenters
{
    public class MvxUnityViewPresenter
        : MvxBaseUnityViewPresenter
    {
        private readonly MvxApplicationDelegate _applicationDelegate;

        private UINavigationController _masterNavigationController;

        protected virtual UINavigationController MasterNavigationController
        {
            get { return _masterNavigationController; }
        }

        public MvxUnityViewPresenter(MvxApplicationDelegate applicationDelegate)
        {
            _applicationDelegate = applicationDelegate;
        }

        public override void Show(MvxViewModelRequest request)
        {
            var view = this.CreateViewControllerFor(request);

            Show(view);
        }

        public virtual void Show(IMvxUnityView view)
        {
            var viewController = view as UIViewController;
            if (viewController == null)
                throw new MvxException("Passed in IMvxUnityView is not a UIViewController");

            int layerMask = 1 << viewController.gameObject.layer;
            if ((_applicationDelegate.uiCamera.cachedCamera.cullingMask & layerMask) == 0)
            {
                // ignore if not visible by ui camera
                return;
            }

            if (_masterNavigationController == null)
                ShowFirstView(viewController);
            else
                _masterNavigationController.PushViewController(viewController, true);
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is MvxClosePresentationHint)
            {
                Close((hint as MvxClosePresentationHint).ViewModelToClose);
                return;

            }
			
			if (hint is MvxClearStackPresentationHint)
			{
				ClearStack();
				return;
			}

            base.ChangePresentation(hint);
        }
		
		public void ClearStack()
		{
			this._masterNavigationController.PopToRootViewController(false);
		}

        public virtual void Close(IMvxViewModel toClose)
        {
            var topViewController = _masterNavigationController.TopViewController;

            if (topViewController == null)
            {
                MvxTrace.Warning(string.Format( "Don't know how to close this viewmodel {0} - no topmost", toClose));
                return;
            }

            var topView = topViewController as IMvxUnityView;
            if (topView == null)
            {
                MvxTrace.Warning(string.Format( "Don't know how to close this viewmodel {0} - topmost {1} is not a IMvxUnityView", toClose, topViewController));
                return;
            }

            var viewModel = topView.ReflectionGetViewModel();
			
            if (viewModel != toClose)
            {
                MvxTrace.Warning(string.Format( "Don't know how to close this viewmodel {0} - topmost {1} does not present this viewmodel", toClose, topViewController));
                return;
            }

            _masterNavigationController.PopViewControllerAnimated(true);
        }

        protected virtual void ShowFirstView(UIViewController viewController)
        {
            _masterNavigationController = CreateNavigationController(viewController);

            OnMasterNavigationControllerCreated();
        }

        protected virtual void OnMasterNavigationControllerCreated()
        {
        }

        protected virtual UINavigationController CreateNavigationController(UIViewController viewController)
        {
            GameObject go = NGUITools.AddChild(_applicationDelegate.uiCamera.gameObject);
            go.name = "UINavigationController";
            UINavigationController _uiNavigationController = go.AddComponent<UINavigationController>();
            _uiNavigationController.PushViewController(viewController, false);
            return _uiNavigationController;
        }

        protected virtual UIViewController CurrentTopViewController
        {
            get { return _masterNavigationController.TopViewController; }
        }
    }
}