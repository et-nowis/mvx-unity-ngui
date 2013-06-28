// UIViewController.cs
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

using System;
using System.Reflection;
using Cirrious.MvvmCross.Unity.Views;
using UnityEngine;

namespace Cirrious.CrossCore.Unity.Views
{

    public class UIViewController : MonoBehaviour, IDisposable
    {

        protected virtual void Awake()
        {

        }

        protected void Start()
        {
            this.ViewDidLoad();
        }

        protected void OnDestroy()
        {
            this.Dispose();
        }

        protected virtual void ViewDidLoad()
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        public virtual void PresentViewController(UIViewController viewController, bool animated, Action action)
        {
            GameObject gameObject = viewController.gameObject;
            GameObject parent = this.gameObject;

            if (parent != null)
            {
                Transform transform = gameObject.transform;
                transform.parent = parent.transform;
                transform.localPosition = new Vector3(0, 0, UINavigationController.Instance.ViewControllers.Count * -10);
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
                gameObject.layer = parent.layer;
            }

            if (viewController is IMvxModalUnityView)
            {
                GameObject closeCollider = new GameObject("CloseCollider");
                Transform transform = closeCollider.transform;
                transform.parent = gameObject.transform;
                transform.localPosition = new Vector3(0, 0, 1);
                transform.localRotation = Quaternion.identity;
                closeCollider.layer = LayerMask.NameToLayer("UI");

                UIStretch uiStretch = closeCollider.AddComponent<UIStretch>();
                uiStretch.style = UIStretch.Style.Both;

                BoxCollider collider = closeCollider.AddComponent<BoxCollider>();
                collider.size = new Vector3(1f, 1f, 0f);
                collider.isTrigger = true;
            }

            Animate(gameObject, true, action);
        }

        public virtual void DismissViewController(bool animated, Action action, bool destroy)
        {
            Animate(this.gameObject, false, () =>
            {
                if (action != null) action();
                if (destroy) Destroy(this.gameObject);
            });
        }

        private void Animate(GameObject gameObject, bool forward, Action callback)
        {
            UITweener tween = gameObject.GetComponent<UITweener>();
            if (tween != null)
            {
                FieldInfo _onFinishedFieldInfo;
                if ((_onFinishedFieldInfo = typeof(UITweener).GetField("onFinished")) != null)
                {
                    //tween.onFinished = onFinished;
                    Action<UITweener> onFinished = tweener =>
                    {
                        if (callback != null) callback();
                    };
                    _onFinishedFieldInfo.SetValue(tween, onFinished.Cast(_onFinishedFieldInfo.FieldType));
                }
                else
                {
                    tween.eventReceiver = tween.gameObject;
                    tween.callWhenFinished = "OnTweenFinished";
                    MvxUITweenerOnFinishedEventHandler handler = tween.GetComponent<MvxUITweenerOnFinishedEventHandler>();
                    if (handler == null)
                        handler = tween.gameObject.AddComponent<MvxUITweenerOnFinishedEventHandler>();
                    handler.onFinished = tweener =>
                    {
                        if (callback != null) callback();
                    };
                }
                tween.Play(forward);
            }
            else
            {
                if (callback != null) callback();
            }
        }

        public UIViewController parentViewController
        {
            get { return this.gameObject.transform.parent.GetComponent<UIViewController>(); }
        }

    }

}
