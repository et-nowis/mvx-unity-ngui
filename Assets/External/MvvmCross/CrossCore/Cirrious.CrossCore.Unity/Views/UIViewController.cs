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

        protected virtual void Start()
        {
            this.ViewDidLoad();
        }
		
		public virtual void ViewWillDisappear(bool animated)
        {
        }

        public virtual void ViewDidAppear(bool animated)
        {
        }

        public virtual void ViewWillAppear(bool animated)
        {
        }

        public virtual void ViewDidDisappear(bool animated)
        {
		}
		
		protected virtual void ViewDidLoad()
        {
        }

        protected virtual void OnDestroy()
        {
            this.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
        }
		
		public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void PresentViewController(UIViewController viewController, bool animated, Action action)
        {
            GameObject go = viewController.gameObject;
            GameObject parent = this.gameObject;

            if (parent != null)
            {
                Transform transform = go.transform;
                transform.parent = parent.transform;
				
#if NGUI_3			
				foreach( UIPanel panel in go.GetComponentsInChildren<UIPanel>())
				{
					panel.depth = panel.depth + UINavigationController.Instance.ViewControllers.Count;
				}
				transform.localPosition = new Vector3(0, 0, 0 );
#else
                transform.localPosition = new Vector3(0, 0, UINavigationController.Instance.ViewControllers.Count * -10);
#endif
				
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
                go.layer = parent.layer;
            }

            if (viewController is IMvxModalUnityView)
            {
				GameObject closeCollider = new GameObject("CloseCollider");
                Transform transform = closeCollider.transform;
                transform.parent = go.transform;
#if NGUI_3	
				transform.localPosition = new Vector3(0, 0, 0);
				transform.localScale = new Vector3(1, 1, 1);
#else
				transform.localPosition = new Vector3(0, 0, 1);
#endif
                transform.localRotation = Quaternion.identity;
                closeCollider.layer = LayerMask.NameToLayer("UI");

                BoxCollider collider = closeCollider.AddComponent<BoxCollider>();
				collider.isTrigger = true;
				collider.size = new Vector3(Screen.width, Screen.height, 0f);
#if NGUI_3		
				UIWidget uiWidget = closeCollider.AddComponent<UIWidget>();
				uiWidget.width = Screen.width;
				uiWidget.height = Screen.height;
#endif
            }

            Animate(go, true, action);
        }

        public virtual void DismissViewController(bool animated, Action action, bool destroy)
        {
            Animate(this.gameObject, false, () =>
            {
                if (action != null) action();
                if (destroy) 
				{ 
#if SPAWN_POOL
					foreach( UICollectionView view in this.gameObject.GetComponentsInChildren<UICollectionView>())
					{
						view.DespawnChildList();	
					}
#endif
					Destroy(this.gameObject);
				}
            });
        }

        private void Animate(GameObject gameObject, bool forward, Action callback)
        {
            UITweener tween = gameObject.GetComponent<UITweener>();
            if (tween != null)
            {
#if NGUI_3				
				EventDelegate.Set( tween.onFinished, 
	                () =>
	                {
	                    if (callback != null) callback();
	                }
				);
#else
				tween.onFinished = tweener =>
                {
                    if (callback != null) callback();
                };
#endif				
				
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
