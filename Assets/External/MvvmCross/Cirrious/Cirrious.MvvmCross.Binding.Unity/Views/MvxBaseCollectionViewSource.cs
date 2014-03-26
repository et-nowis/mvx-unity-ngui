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
using System.Windows.Input;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.Unity.Views;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Unity.Views;
using UnityEngine;

namespace Cirrious.MvvmCross.Binding.Unity.Views
{
    public abstract class MvxBaseCollectionViewSource : UICollectionViewSource
    {
        public static readonly string UnknownCellIdentifier = null;

        private readonly string _cellIdentifier;
        private readonly UICollectionView _collectionView;

        protected virtual string DefaultCellIdentifier
        {
            get { return _cellIdentifier; }
        }

        protected MvxBaseCollectionViewSource(UICollectionView collectionView)
            : this(collectionView, UnknownCellIdentifier)
        {

        }

        protected MvxBaseCollectionViewSource(UICollectionView collectionView,
                                              string cellIdentifier)
        {
            _collectionView = collectionView;
            _cellIdentifier = cellIdentifier;
        }

        public UICollectionView CollectionView
        {
            get { return _collectionView; }
        }

        public ICommand SelectionChangedCommand { get; set; }

        public virtual void ReloadData()
        {
            try
            {
                _collectionView.ReloadData();
            }
            catch (Exception exception)
            {
                Mvx.Warning("Exception masked during CollectionView ReloadData {0}", exception.ToString());
            }
        }

        protected virtual GameObject GetOrCreateCellFor(UICollectionView collectionView, IndexPath indexPath,
                                                                  object item)
        {
            return (GameObject)collectionView.DequeueReusableCell(DefaultCellIdentifier, indexPath);
        }

        protected abstract object GetItemAt(IndexPath indexPath);

        public override void ItemSelected(UICollectionView collectionView, IndexPath indexPath)
        {
            var item = GetItemAt(indexPath);

            var command = SelectionChangedCommand;
            if (command != null)
                command.Execute(item);

            SelectedItem = item;
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                // note that we only expect this to be called from the control/Table
                // we don't have any multi-select or any scroll into view functionality here
                _selectedItem = value;
                var handler = SelectedItemChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler SelectedItemChanged;

        public override GameObject GetCell(UICollectionView collectionView, IndexPath indexPath)
        {
			if ( collectionView == null ) return null;
				
            var item = GetItemAt(indexPath);
            var cell = GetOrCreateCellFor(collectionView, indexPath, item);

            var bindable = cell.GetComponent(typeof(IMvxDataConsumer)) as IMvxDataConsumer;
            if (bindable != null)
            {
                bindable.DataContext = item;
            }
            return cell;
        }

        public override int NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public virtual void InsertItems(IndexPath[] indexPaths)
        {
            if ( !_collectionView.Busy )
			{
				_collectionView.Progress = 0.0f;
				_collectionView.InsertItems(indexPaths);
			}
        }

        public virtual void DeleteItems(IndexPath[] indexPaths)
        {
            _collectionView.DeleteItems(indexPaths);
        }
    }
}