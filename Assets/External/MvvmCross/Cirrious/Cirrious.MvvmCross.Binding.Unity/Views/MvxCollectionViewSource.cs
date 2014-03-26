// MvxCollectionViewSource.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Unity.Views;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using Cirrious.MvvmCross.Unity.Views;
using UnityEngine;
using Cirrious.CrossCore;

namespace Cirrious.MvvmCross.Binding.Unity.Views
{
    public class MvxCollectionViewSource : MvxBaseCollectionViewSource
    {
        private IEnumerable _itemsSource;
        private IDisposable _subscription;

        public MvxCollectionViewSource(UICollectionView collectionView)
            : base(collectionView)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }
            }

            base.Dispose(disposing);
        }

        public MvxCollectionViewSource(UICollectionView collectionView,
                                       string defaultCellIdentifier)
            : base(collectionView, defaultCellIdentifier)
        {
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                if (_itemsSource == value)
                    return;

                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }
                _itemsSource = value;
                var collectionChanged = _itemsSource as INotifyCollectionChanged;
                if (collectionChanged != null)
                {
                    _subscription = collectionChanged.WeakSubscribe(CollectionChangedOnCollectionChanged);
                }

                if (_itemsSource != null)
                {
                    Mvx.TaggedTrace(GetType().Name, "ItemsSource");
                    ReloadData();
                }
            }
        }

        protected override object GetItemAt(IndexPath indexPath)
        {
            if (ItemsSource == null)
                return null;
#if MVX_DBG
            UnityEngine.Debug.LogError("MvxCollection view getting element!!!:" + indexPath.Index + ":" + indexPath.Row);
            UnityEngine.Debug.LogError("Item source type:"+ItemsSource);
#endif
            return ItemsSource.ElementAt(indexPath.Row);
        }

        private void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Debug.Log( "OnCollectionChanged " + e.Action );
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Reset:
                    Mvx.TaggedTrace(GetType().Name, "CollectionChangedOnCollectionChanged(Reset)");
                    ReloadData();
                    break;
                case NotifyCollectionChangedAction.Add:
                    {
                        Mvx.TaggedTrace(GetType().Name, "CollectionChangedOnCollectionChanged(Add)");
                        int startingIndex = e.NewStartingIndex;
                        int count = e.NewItems.Count;

                        IndexPath[] indexPaths = new IndexPath[count];

                        for (int i = 0, k = startingIndex; i < count; ++i, ++k)
                        {
                            indexPaths[i] = new IndexPath() { Index = k, Row = k };
                        }

                        InsertItems(indexPaths);

                        break;
                    }
                case NotifyCollectionChangedAction.Remove:
                    {
                        Mvx.TaggedTrace(GetType().Name, "CollectionChangedOnCollectionChanged(Remove)");
                        int startingIndex = e.OldStartingIndex;
                        int count = e.OldItems.Count;

                        IndexPath[] indexPaths = new IndexPath[count];

                        for (int i = 0, k = startingIndex; i < count; ++i, ++k)
                        {
                            indexPaths[i] = new IndexPath() { Index = k, Row = k };
                        }
                        DeleteItems(indexPaths);

                        break;
                    }
                case NotifyCollectionChangedAction.Replace:
                    {
                        throw new NotSupportedException("Replace is not supported");
                        //int startingIndex = e.NewStartingIndex;
                        //int count = e.NewItems.Count;
                        //for (int i = 0, k = startingIndex; i < count; ++i, ++k)
                        //{
                        //    GameObject.Destroy(_childList[k]);
                        //    var cell = GetOrCreateCellFor(_collectionView, "", e.NewItems[i]);
                        //    _childList[k] = cell;
                        //}
                        //RenameCell(startingIndex, startingIndex + count);
                        //Reposition();
                        //break;
                    }

                case NotifyCollectionChangedAction.Move:
                    {
                        throw new NotSupportedException("Move is not supported");
                        //int newStartingIndex = e.NewStartingIndex;
                        //int oldStartingIndex = e.OldStartingIndex;
                        //int count = e.OldItems.Count;
                        //var children = _childList.GetRange(oldStartingIndex, count);
                        //_childList.RemoveRange(oldStartingIndex, count);
                        //_childList.InsertRange(newStartingIndex, children);
                        //RenameCell(Math.Min(oldStartingIndex, newStartingIndex), Math.Max(oldStartingIndex, newStartingIndex) + count);
                        //Reposition();
                        //break;
                    }

            }

        }

        public override int GetItemsCount(UICollectionView collectionView, int section)
        {
            if (ItemsSource == null)
                return 0;

            return ItemsSource.Count();
        }

    }
}