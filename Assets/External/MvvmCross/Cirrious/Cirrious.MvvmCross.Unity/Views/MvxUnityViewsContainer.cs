// MvxUnityViewsContainer.cs
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

using System.Linq;
using Cirrious.CrossCore.Exceptions;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.Views;
using UnityEngine;

namespace Cirrious.MvvmCross.Unity.Views
{
    public class MvxUnityViewsContainer
        : MvxViewsContainer,
          IMvxUnityViewCreator,
          IMvxCurrentRequest
    {
        public MvxViewModelRequest CurrentRequest { get; private set; }

        public virtual IMvxUnityView CreateView(MvxViewModelRequest request)
        {
            try
            {
                CurrentRequest = request;

                IMvxUnityView view;
                var viewType = GetViewType(request.ViewModelType);
                if (viewType == null)
                    throw new MvxException("View Type not found for " + request.ViewModelType);

                var customAttribute = (MvxUnityViewAttribute)viewType.GetCustomAttributes(typeof(MvxUnityViewAttribute), false).FirstOrDefault();

                if (customAttribute != null)
                {
                    string viewUrl = customAttribute.Url;

                    GameObject prefab = (GameObject)UnityEngine.Resources.Load(viewUrl);

                    Vector3 newPosition;
                    if (prefab.layer == LayerMask.NameToLayer("UI"))
                    {
						Camera uiCam = UICamera.FindCameraForLayer(prefab.layer).camera;
                        newPosition = new Vector3(0, 0, uiCam.farClipPlane + 100f);
                    }
                    else
                    {
                        newPosition = Vector3.zero;
                    }

                    GameObject go = GameObject.Instantiate(prefab, newPosition, Quaternion.identity) as GameObject;

                    view = go.GetComponent(typeof(IMvxUnityView)) as IMvxUnityView;
                }
                else
                {
                    GameObject go = new GameObject();
                    go.name = viewType.Name + "(Clone)";
                    go = go.AddComponent(viewType).gameObject;
                    view = go.GetComponent(typeof(IMvxUnityView)) as IMvxUnityView;
                }

                if (view == null)
                    throw new MvxException("View not loaded for " + viewType);
                view.Request = request;
                return view;
            }
            finally
            {
                CurrentRequest = null;
            }
        }

        public virtual IMvxUnityView CreateView(IMvxViewModel viewModel)
        {
            var request = new MvxViewModelInstanceRequest(viewModel);
            var view = CreateView(request);
            return view;
        }
    }
}