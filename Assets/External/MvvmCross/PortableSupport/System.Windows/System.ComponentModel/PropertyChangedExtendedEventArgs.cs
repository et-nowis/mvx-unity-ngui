using System.ComponentModel;

namespace System.ComponentModel
{
	public class PropertyChangedExtendedEventArgs<T> : PropertyChangedEventArgs
	{
	    public T OldValue { get; private set; }
	    public T NewValue { get; private set; }
	
	    public PropertyChangedExtendedEventArgs(string propertyName)
	        : base(propertyName)
	    {
	    }
	
	    public PropertyChangedExtendedEventArgs(string propertyName, T oldValue, T newValue)
	        : base(propertyName)
	    {
	        this.OldValue = oldValue;
	        this.NewValue = newValue;
	    }
	}
}
