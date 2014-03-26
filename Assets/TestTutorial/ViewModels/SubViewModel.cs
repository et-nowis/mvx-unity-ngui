using UnityEngine;
using System.Collections;
using Cirrious.MvvmCross.Binding.Unity.Views;
using Cirrious.MvvmCross.ViewModels;

public class SubViewModel : MvxViewModel {
	
	private string _title;
    public string Title
    {
        get { return _title; }
        set { _title = value; RaisePropertyChanged(() => Title); }
   	}

	public void Init()
    {
       Title = "Hello World";
    }
}
