using Cirrious.MvvmCross.Plugins.Messenger;

namespace TestTutorial.Messages
{
	public class TestChangedMessage : MvxMessage
	{
	    public TestChangedMessage(object sender)
	        : base(sender)
	    {
	    }
	}
}