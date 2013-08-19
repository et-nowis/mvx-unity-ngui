using Cirrious.MvvmCross.Plugins.Messenger;

namespace TestTutorial.Messages
{
	public class ButtonClickMessage : MvxMessage
	{
	    public ButtonClickMessage(object sender)
	        : base(sender)
	    {
	    }
	
	    public int ButtonIndex { get; set; }
	    public string ButtonAction { get; set; }
	}
}