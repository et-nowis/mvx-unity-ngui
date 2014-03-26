using UnityEngine;
using System.Collections;
using System;

namespace Cirrious.CrossCore.Unity.Views
{
	
	public class UICollectionViewCell: MonoBehaviour, IDisposable {
		
		protected virtual void Awake()
	    {
	
	    }

		protected virtual void Start ()
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
	}
	
}
