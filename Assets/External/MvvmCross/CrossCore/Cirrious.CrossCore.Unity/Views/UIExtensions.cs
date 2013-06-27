// UIExtensions.cs
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
using System.Linq;
using UnityEngine;

namespace Cirrious.CrossCore.Unity.Views
{

    public static class UIExtensions
    {
        public static void SetLayer(this Transform transform, int layer, IEnumerable<string> excludeLayers = null)
        {
            IEnumerable<int> exclude = excludeLayers == null ? null : excludeLayers.Select(x => LayerMask.NameToLayer(x));
            SetLayer(transform, layer, exclude);
        }

        public static void SetLayer(this Transform transform, int layer, IEnumerable<int> excludeLayers = null)
        {
            if (excludeLayers == null || !excludeLayers.Contains(transform.gameObject.layer))
                transform.gameObject.layer = layer;

            for (int i = 0; i < transform.GetChildCount(); ++i)
            {
                Transform child = transform.GetChild(i);

                if (excludeLayers == null || !excludeLayers.Contains(child.gameObject.layer))
                    child.gameObject.layer = layer;

                SetLayer(child, layer, excludeLayers);
            }
        }
    }

}