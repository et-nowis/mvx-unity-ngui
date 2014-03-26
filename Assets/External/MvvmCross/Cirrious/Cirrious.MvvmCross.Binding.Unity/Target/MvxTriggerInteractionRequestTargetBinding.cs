using System;
using System.Windows.Interactivity;
using Cirrious.MvvmCross.Binding;
using Cirrious.MvvmCross.Binding.Bindings.Target;
using Cirrious.MvvmCross.ViewModels;

namespace Cirrious.MvvmCross.Binding.Unity.Target
{
	public class MvxTriggerInteractionRequestTargetBinding : MvxTargetBinding
	{
	    public MvxTriggerInteractionRequestTargetBinding(object target)
	        : base(target)
	    {
	    }
	
	    public override MvxBindingMode DefaultMode
	    {
	        get { return MvxBindingMode.OneWay; }
	    }
		
		public override Type TargetType
	    {
	        get { return typeof(IInteractionRequest); }
	    }
		
		public override void SetValue(object value)
	    {
	        InteractionRequestTrigger trigger = (InteractionRequestTrigger)Target;
	        IInteractionRequest request = (IInteractionRequest)value;
	        trigger.Attach(request);
	    }
	
	    protected override void Dispose(bool isDisposing)
	    {
	        if (isDisposing)
	        {
	            InteractionRequestTrigger trigger = (InteractionRequestTrigger)Target;
	            trigger.Detach();
	        }
	        base.Dispose(isDisposing);
	    }
	}
}
