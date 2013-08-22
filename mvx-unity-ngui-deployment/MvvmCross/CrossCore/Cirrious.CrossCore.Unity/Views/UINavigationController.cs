// UINavigationController.cs
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

using System.Collections.Generic;
using UnityEngine;

namespace Cirrious.CrossCore.Unity.Views
{

    public class UINavigationController : UIViewController
    {
        static public UINavigationController Instance { get; private set; }

        private Stack<UIViewController> _viewControllers;

        public Stack<UIViewController> ViewControllers
        {
            get { return _viewControllers; }
            set { _viewControllers = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
            _viewControllers = new Stack<UIViewController>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Instance = null;
            }
            base.Dispose(disposing);
        }

        public UIViewController TopViewController
        {
            get
            {
                return _viewControllers.Count > 0 ? _viewControllers.Peek() : null;
            }
        }

        public void PushViewController(UIViewController viewController, bool animated)
        {
            var top = TopViewController;
            if (top != null)
            {
                top.DismissViewController(true, null, false);
            }

            _viewControllers.Push(viewController);
            viewController.gameObject.name = _viewControllers.Count + " - " + viewController.gameObject.name;

            this.PresentViewController(viewController, true, null);
        }

        public UIViewController PopViewControllerAnimated(bool animated)
        {
            var pop = _viewControllers.Pop();
            pop.DismissViewController(false, null, true);

            var top = TopViewController;
            if (top != null)
            {
                if (!top.gameObject.activeSelf)
                {
                    top.PresentViewController(top, true, null);
                }
            }
            return pop;
        }

        //Pops view controllers until the specified view controller is at the top of the navigation stack.
        public UIViewController PopToViewController(UIViewController viewController, bool animated)
        {
            UIViewController pop = null;
            var top = TopViewController;
            while (top != viewController)
            {
                pop = PopViewControllerAnimated(animated);
                top = TopViewController;
            }
            return pop;
        }

        //Pops all the view controllers on the stack except the root view controller and updates the display.
        public UIViewController[] PopToRootViewController(bool animated)
        {
            while (_viewControllers.Count > 1)
            {
                PopViewControllerAnimated(animated);
            }

            return _viewControllers.ToArray();
        }

    }

}
