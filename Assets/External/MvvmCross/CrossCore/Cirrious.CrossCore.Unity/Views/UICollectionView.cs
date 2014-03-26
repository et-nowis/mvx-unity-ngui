using System;
using System.Collections;
using System.Collections.Generic;
using Cirrious.CrossCore.Core;
using Cirrious.MvvmCross.Binding.ExtensionMethods;
using Cirrious.MvvmCross.Unity.Platform;
using UnityEngine;
#if SPAWN_POOL
using PathologicalGames;
#endif

namespace Cirrious.CrossCore.Unity.Views
{
    public class UICollectionView : MonoBehaviour
    {
		private int x = 0;
		private int y = 0;

#if SPAWN_POOL
		public string PoolName;
		public bool usePool = false;
		private SpawnPool _spawnPool;
#endif

        public bool AllowMultipleSelection = false;
        public bool AllowSelection = false;

        public bool AsyncLoad = false;
        public int AsyncPageSize = 3;
        public bool AsyncPaging = false;
		
        public UICollectionViewSource Source { get; set; }

        public Dictionary<string, GameObject> Templates = new Dictionary<string, GameObject>();

        public GameObject Template;

        public UIPanel Panel;

#if NGUI_3
        public UIScrollView scrollView;
#else
		public UIDraggablePanel DraggablePanel;
#endif

        public UIGrid Grid;

        public bool Busy { get; set; }
        public float Progress { get; set; }

        public event EventHandler<MvxValueEventArgs<GameObject>> Progressed;
        public event EventHandler Completed;

        private List<GameObject> _childList = new List<GameObject>();

        public List<GameObject> ChildList
        {
            get { return _childList; }
        }

#if SPAWN_POOL
		public void DespawnChildList()
		{
			if (usePool)
			{
				foreach (var child in _childList)
	            {
					if ( _spawnPool != null )
						_spawnPool.Despawn(child.transform, _spawnPool.transform);
	            }
			}
		}
#endif
		
        private void Reposition(GameObject cell)
        {
            if (Grid != null)
            {
				Transform t = cell.transform;

				if (NGUITools.GetActive(t.gameObject) && Grid.hideInactive)
				{
					float depth = t.localPosition.z;
					Vector3 pos = (Grid.arrangement == UIGrid.Arrangement.Horizontal) ?
						new Vector3(Grid.cellWidth * x, -Grid.cellHeight * y, depth) :
						new Vector3(Grid.cellWidth * y, -Grid.cellHeight * x, depth);
	
#if NGUI_3					
					if (Grid.animateSmoothly && Application.isPlaying)
					{
						SpringPosition.Begin(t.gameObject, pos, 15f);
					}
					else
						t.localPosition = pos;
#else
						t.localPosition = pos;
#endif
	
					if (++x >= Grid.maxPerLine && Grid.maxPerLine > 0)
					{
						x = 0;
						++y;
					}
					
				}
				
            }
        }

        //UICollectionView will discard any visible items and then redisplay them
        public virtual void ReloadData()
        {
            StopAllCoroutines();
			
			x = 0;
			y = 0;
#if SPAWN_POOL
			if ( usePool && _spawnPool != null )
			{
				foreach (var child in _childList)
				{
					_spawnPool.Despawn(child.transform, _spawnPool.transform);
				}
			}
			else
			{
				foreach (var child in _childList)
            	{
               		GameObject.Destroy(child);
				}
			}
#else
            foreach (var child in _childList)
            {
                child.transform.parent = null;
                GameObject.Destroy(child);
            }
#endif
            _childList.Clear();

            if (Source != null)
            {
                int itemCount = Source.GetItemsCount(this, 0);

                if (itemCount > 0)
                {
                    IndexPath[] indexPaths = new IndexPath[itemCount];

                    for (int i = 0; i < indexPaths.Length; ++i)
                    {
                        indexPaths[i] = new IndexPath() { Index = i, Row = i };
                    }

                    Busy = true;
                    Progress = 0.0f;
					
					InsertItems(indexPaths);
                }
            }

        }
		
		public void InsertItems(IndexPath[] indexPaths)
		{
			var iter = GetInsertItemsEnumerator(indexPaths);

			if ( AsyncLoad )
			{
            	StartCoroutine(iter);
			}
			else
			{
				while ( iter.MoveNext() ) ;
			}
		}

        //If the specified elements are visible, the UICollectionView will discard and then redisplay them.
        public virtual void ReloadItems(IndexPath[] indexPaths)
        {
            throw new NotImplementedException();
        }

        public void RegisterTemplateForCell(string templatePath, string reuseIdentifier)
        {
            Templates[reuseIdentifier] = Resources.Load(templatePath) as GameObject;
        }

        public virtual GameObject DequeueReusableCell(string reuseIdentifier, IndexPath indexPath)
        {
            if (reuseIdentifier != null)
            {
                this.Template = Templates[reuseIdentifier];
            }
            //Debug.Log( "DequeueReusableCell " + reuseIdentifier + "," + indexPath );
            GameObject parent = Grid != null ? Grid.gameObject : this.gameObject;

            GameObject cell = null;
#if !MVX_DBG
            //if (_childList.Count > indexPath.Index && _childList[indexPath.Index] != null)
            //{
            //    cell = _childList[indexPath.Index] as GameObject;
            //}
#endif
            if (cell == null)
            {
#if SPAWN_POOL			
				if ( usePool && _spawnPool != null )
					cell = _spawnPool.Spawn(this.Template.name).gameObject;
				else
#endif
                cell = GameObject.Instantiate(this.Template) as GameObject;

                _childList.Add(cell);

                if (cell != null && parent != null)
                {
                    Transform t = cell.transform;
                    t.parent = parent.transform;
                    t.localPosition = Vector3.zero;
                    t.localRotation = Quaternion.identity;
                    t.localScale = Vector3.one;
                }

            }

            return cell;
        }

        protected virtual void ItemSelected(GameObject go)
        {
            Source.ItemSelected(this, IndexPathForCell(go));
            //Debug.Log( "ItemSelected " + go + "," + _childList.IndexOf(go) ) ;
        }
		
        protected virtual IEnumerator GetInsertItemsEnumerator(IndexPath[] indexPaths)
        {
            for (int i = 0; i < indexPaths.Length; ++i)
            {
//				Debug.Log( string.Format("{0}%{1}={2},{0}/{1}={3}", i, AsyncPageSize, i % AsyncPageSize, i / AsyncPageSize ));
				
                if (!AsyncPaging || (AsyncPaging && ( i % AsyncPageSize == 0 && i / AsyncPageSize > 0 )) )
                {
//					yield return new ContinueAfterSeconds(1f);
//                  yield return new WaitForEndOfFrame();
//                  yield return new WaitForSeconds(1f);
	                yield return null; //Wait one Frame
                }

                GameObject cell = Source.GetCell(this, indexPaths[i]);

                if (cell != null)
                {
					Reposition(cell);
					
                    cell.name = string.Format("{0:000}", indexPaths[i].Index);

                    Progress = (float)i / (float)(indexPaths.Length - 1);
                    if (Progressed != null)
                    {
                        Progressed(this, new MvxValueEventArgs<GameObject>(cell));
                    }

                    if (AllowSelection)
                    {
                        UIEventListener.Get(cell).onClick += ItemSelected;
                    }
                }
            }
			
			Progress = 1f;
            Busy = false;
            
            if (Completed != null)
            {
                Completed(this, EventArgs.Empty);
            }
        }

        public virtual void DeleteItems(IndexPath[] indexPaths)
        {
            if (Busy)
            {
                string errorMessage = "Async loading not finished yet";
                if (Debug.isDebugBuild)
                {
                    throw new InvalidOperationException(errorMessage);
                }
                else
                {
                    Mvx.Error(errorMessage);
                    return;
                }
            }

            for (int i = 0; i < indexPaths.Length; ++i)
            {
                if (AllowSelection)
                {
                    if (_childList[indexPaths[i].Index] != null)
                    {
                        UIEventListener.Get(_childList[indexPaths[i].Index]).onClick -= ItemSelected;
                    }
                }

#if SPAWN_POOL		
				if ( usePool && _spawnPool != null )
					_spawnPool.Despawn(_childList[indexPaths[i].Index].transform, _spawnPool.transform);
				else
#endif
                _childList[indexPaths[i].Index].transform.parent = null;
                GameObject.Destroy(_childList[indexPaths[i].Index]);

                _childList.RemoveRange(indexPaths[i].Index, 1);
            }
			
			if ( Grid != null )
			{
				Grid.Reposition();	
			}
        }

        public virtual void MoveItem(IndexPath indexPath, IndexPath newIndexPath)
        {
            throw new NotImplementedException();
        }

        public virtual GameObject CellForItem(IndexPath indexPath)
        {
            return _childList.ElementAt(indexPath.Index) as GameObject;
        }

        public virtual IndexPath IndexPathForCell(GameObject cell)
        {
            int index = _childList.IndexOf(cell);

            return new IndexPath() { Index = index, Row = index };
        }

        public virtual void ScrollToItem(IndexPath indexPath, Vector2 scrollPosition, bool animated)
        {
            throw new NotImplementedException();
        }

        public virtual void SelectItem(IndexPath indexPath, bool animated, Vector2 scrollPosition)
        {
            throw new NotImplementedException();
        }

        public virtual void DeselectItem(IndexPath indexPath, bool animated)
        {
            throw new NotImplementedException();
        }


    }
}