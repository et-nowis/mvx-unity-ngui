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
using Cirrious.CrossCore.Core;
using Cirrious.CrossCore.WeakSubscription;
using Cirrious.MvvmCross.Binding.Attributes;
using Cirrious.MvvmCross.Unity.Views;
using UnityEngine;

[AddComponentMenu("NGUIExtensions/UI/CollectionViewSource")]
public class MvxCollectionViewSource : MonoBehaviour
{

    public GameObject Template;

    public UIPanel Panel;
    public UIDraggablePanel DraggablePanel;
    public UIGrid Grid;

    private List<GameObject> _childList = new List<GameObject>();

    private IEnumerable _itemsSource;
    private IDisposable _subscription;

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
                _subscription = collectionChanged.WeakSubscribe(this.OnCollectionChanged);
            }
            ReloadData();
        }
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Reset:
                ReloadData();
                break;
            case NotifyCollectionChangedAction.Add:
                {
                    int startingIndex = e.NewStartingIndex;
                    int count = e.NewItems.Count;
                    for (int i = 0, k = startingIndex; i < count; ++i, ++k)
                    {
                        var cell = CreateCellFor(Template, e.NewItems[i]);
                        _childList.Insert(k, cell);
                    }
                    RenameCell(startingIndex, _childList.Count);
                    Reposition();
                }
                break;
            case NotifyCollectionChangedAction.Remove:
                {
                    int startingIndex = e.OldStartingIndex;
                    int count = e.OldItems.Count;
                    for (int i = 0, k = startingIndex; i < count; ++i, ++k)
                    {
                        UIEventListener.Get(_childList[k]).onClick -= ItemSelected;
                        GameObject.DestroyImmediate(_childList[k]);
                    }
                    _childList.RemoveRange(startingIndex, count);
                    RenameCell(startingIndex, _childList.Count);
                    Reposition();
                }
                break;
            case NotifyCollectionChangedAction.Replace:
                {
                    int startingIndex = e.NewStartingIndex;
                    int count = e.NewItems.Count;
                    for (int i = 0, k = startingIndex; i < count; ++i, ++k)
                    {
                        UIEventListener.Get(_childList[k]).onClick -= ItemSelected;
                        GameObject.DestroyImmediate(_childList[k]);
                        var cell = CreateCellFor(Template, e.NewItems[i]);
                        _childList[k] = cell;
                    }
                    RenameCell(startingIndex, startingIndex + count);
                    Reposition();
                }
                break;
            case NotifyCollectionChangedAction.Move:
                {
                    int newStartingIndex = e.NewStartingIndex;
                    int oldStartingIndex = e.OldStartingIndex;
                    int count = e.OldItems.Count;
                    var children = _childList.GetRange(oldStartingIndex, count);
                    _childList.RemoveRange(oldStartingIndex, count);
                    _childList.InsertRange(newStartingIndex, children);
                    RenameCell(Math.Min(oldStartingIndex, newStartingIndex), Math.Max(oldStartingIndex, newStartingIndex) + count);
                    Reposition();
                }
                break;
        }
    }

    private void Awake()
    {
        if (Panel == null)
        {
            Panel = this.GetComponent<UIPanel>();
        }

        if (DraggablePanel == null)
        {
            DraggablePanel = this.GetComponent<UIDraggablePanel>();
        }
    }

    private void OnDestroy()
    {
        if (_subscription != null)
        {
            _subscription.Dispose();
            _subscription = null;
        }
    }

    private void LateUpdate()
    {
        if (!Application.isPlaying) return;

        if (Panel != null)
        {
            Panel.Refresh();
        }
    }

    private void ReloadData()
    {
        foreach (var child in _childList)
        {
            GameObject.DestroyImmediate(child);
        }
        _childList.Clear();

        if (ItemsSource != null)
        {
            int index = 0;
            foreach (object item in ItemsSource)
            {
                var cell = CreateCellFor(Template, item);
                _childList.Add(cell);
                ++index;
            }

            RenameCell(0, _childList.Count);
        }

        Reposition();
    }

    private GameObject CreateCellFor(GameObject template, object item)
    {
        GameObject parent = Grid != null ? Grid.gameObject : this.gameObject;
        //GameObject cell = NGUITools.AddChild(parent, template);
		
		GameObject cell = GameObject.Instantiate(template) as GameObject;

		if (cell != null && parent != null)
		{
			Transform t = cell.transform;
			t.parent = parent.transform;
			t.localPosition = Vector3.zero;
			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;
		}

        UIEventListener.Get(cell).onClick += ItemSelected;

        //NGUITools.SetLayer(cell, parent.layer);
        IMvxDataConsumer bindable = cell.GetComponent(typeof(IMvxDataConsumer)) as IMvxDataConsumer;
        if (bindable != null)
        {
            bindable.DataContext = item;
        }
        return cell;
    }

    private void RenameCell(int from, int to)
    {
        for (int i = from; i < to; ++i)
        {
            _childList[i].name = string.Format("{0:000}", i);
        }
    }

    private void Reposition()
    {
        if (Grid != null)
        {
            Grid.Reposition();
        }
        //if (DraggablePanel != null)
        // {
        //    DraggablePanel.ResetPosition();
        // }
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

    public ICommand SelectionChangedCommand { get; set; }

    void ItemSelected(GameObject go)
    {
        var item = go.GetComponent<MvxViewController>().DataContext;
		
		var command = SelectionChangedCommand;
		
		if ( command != null )
            command.Execute(item);
		
		SelectedItem = item;
    }

}
