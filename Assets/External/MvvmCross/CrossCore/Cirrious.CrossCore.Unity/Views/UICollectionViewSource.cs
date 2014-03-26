using UnityEngine;
using System.Collections;
using System;

namespace Cirrious.CrossCore.Unity.Views
{
    public class UICollectionViewSource : IDisposable
    {

        public UICollectionViewSource()
        {

        }

        ~UICollectionViewSource()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            // nothing to do in this base class
        }

        public virtual int GetItemsCount(UICollectionView collectionView, int section)
        {

            return 0;
        }

        public virtual GameObject GetCell(UICollectionView collectionView, IndexPath indexPath)
        {
            return new GameObject();
        }

        public virtual void ItemSelected(UICollectionView collectionView, IndexPath indexPath)
        {
            // nothing to do in this base class
        }

        public virtual int NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }
		
    }
}