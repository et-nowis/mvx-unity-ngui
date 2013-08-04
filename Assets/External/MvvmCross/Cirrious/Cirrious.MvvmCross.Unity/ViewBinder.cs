using System;
using System.Collections.Generic;
using UnityEngine;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;

namespace AssemblyCSharp
{
	public class ViewBinder : MvxViewController
	{
		public ViewBinding[] Bindings = new ViewBinding[] {};

		protected override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			var bindingMap = new Dictionary<object, string>();
			foreach (var b in Bindings)
			{
				bindingMap.Add(b.ControlContainer, b.BindText);
			}
			
			this.AddBindings(bindingMap);
		}
		
	}
	
	[Serializable]
	public class ViewBinding
	{
		public GameObject ControlContainer;
		public string BindText;
	}
	
}

