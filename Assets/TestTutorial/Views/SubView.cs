using UnityEngine;
using System.Collections;
using Cirrious.MvvmCross.Binding.Unity.Views;
using Cirrious.MvvmCross.Binding.BindingContext;

public class SubView : MvxView {
	
	public UILabel TitleLabel;

	protected override void Awake()
    {
		base.Awake();
		
        this.DelayBind(() =>
        {
            var bindingSet = this.CreateBindingSet<SubView, SubViewModel>();
			
				bindingSet.Bind(TitleLabel).To(vm => vm.Title);
			
            bindingSet.Apply();
        });
    }        
}
