using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Unity.Views;
using TestTutorial.ViewModels;
using TestTutorial.Converters;
using UnityEngine;
using Cirrious.MvvmCross.Binding.Unity.Views;

[MvxUnityView("TestTutorial_NGUI_3/Views/HUDView")]
public class HUDView : BaseViewController
{
    public UILabel tipValueLabel;
    public UIButton tipView1Button;

    public UIButton showModalDialogButton;
    public UIButton showDialogButton;
    public UIButton buildButton;
	
	public UIButton alertButton;
	
	public UIInput input;
	
	public GameObject SubViewPrefab;

    public new HUDViewModel ViewModel
    {
        get { return (HUDViewModel)base.ViewModel; }
        set { base.ViewModel = value; }
    }

    protected override void ViewDidLoad()
    {
        base.ViewDidLoad();
		
		
		//Create SubView
		GameObject go = GameObject.Instantiate( SubViewPrefab.gameObject, Vector3.zero, Quaternion.identity ) as GameObject;
		go.transform.parent = this.transform;
		go.transform.localScale = Vector3.one;
		go.transform.localPosition = Vector3.zero;
		var currentSubView = go.GetComponent<MvxView>();
		
		
		var bindingSet = this.CreateBindingSet<HUDView, HUDViewModel>();
		
			bindingSet.Bind(tipValueLabel).To(vm => vm.TipValue).WithConversion(Converters.StringFormat, "{0:0.000}");
			bindingSet.Bind(tipView1Button).To(vm => vm.TipView1Command);
			bindingSet.Bind(showDialogButton).To(vm => vm.ShowDialogCommand);
			bindingSet.Bind(showModalDialogButton).To(vm => vm.ShowModalDialogCommand);
			bindingSet.Bind(buildButton).To(vm => vm.BuildCommand);
			bindingSet.Bind(alertButton).To(vm => vm.AlertCommand);
		
			bindingSet.Bind(currentSubView).For(s => s.DataContext).To(vm => vm.Current);
		
		bindingSet.Apply();

    }

}