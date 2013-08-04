using System;
using System.Collections.Generic;
using UnityEngine;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;

namespace AssemblyCSharp
{
    /// <summary>
    /// Attach this script to the View Prefab in the Unity Editor.
    /// </summary>
	public class ViewBinder : MvxViewController
	{
        /// <summary>
        /// Drag the GameObject that contains the NGUI control you want to bind.
        /// Then set the binding text to one or more bindings (delimited by semicolons if more than one).
        /// Ex: text PlayerName
        /// </summary>
		public ViewBinding[] Bindings = new ViewBinding[] {};

		protected override void ViewDidLoad ()
		{
			base.ViewDidLoad();
			
			var bindingMap = new Dictionary<object, string>();
			foreach (var b in Bindings)
			{
				bindingMap.Add(b.UIControl, b.BindText);
			}
			
			this.AddBindings(bindingMap);
		}
	}
	
    /// <summary>
    /// Bindings class to populated via Unity Editor.
    /// </summary>
	[Serializable]
	public class ViewBinding
	{
		public MonoBehaviour UIControl;
		public string BindText;
	}
}

