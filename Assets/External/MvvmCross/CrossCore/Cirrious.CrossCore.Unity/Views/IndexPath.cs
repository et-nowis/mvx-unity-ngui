using UnityEngine;
using System.Collections;

namespace Cirrious.CrossCore.Unity.Views
{
	public struct IndexPath {
		
		//public object Item;
		public int Index;
		public int Length;
		public int Row;
		public int Section;
		
		public override string ToString ()
		{
			return string.Format ("[IndexPath] Index={0},Row={1}", Index, Row);
		}
	
	}
}