using System;
using System.Windows.Interactivity;

namespace Cirrious.MvvmCross.ViewModels
{
	public interface IInteractionRequest
	{
        event EventHandler<InteractionRequestedEventArgs> Raised;
	}
}
