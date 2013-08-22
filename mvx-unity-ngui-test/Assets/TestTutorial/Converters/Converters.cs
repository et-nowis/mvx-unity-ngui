namespace TestTutorial.Converters
{
	public class Converters
    {
        public readonly StringLengthValueConverter StringLength = new StringLengthValueConverter(); //Converter=StringLength
        public readonly StringReverseValueConverter StringReverse = new StringReverseValueConverter();  //Converter=StringReverse
        public readonly FloatConverter Float = new FloatConverter();  //Converter=Float
        public readonly IntConverter Int = new IntConverter(); //Converter=Int
        public readonly IntToFloatConverter IntToFloat = new IntToFloatConverter(); //Converter=IntToFloat
		public readonly StringFormatValueConverter StringFormat = new StringFormatValueConverter(); //Converter=StringFormat
		public readonly InverseBooleanValueConverter InverseBoolean = new InverseBooleanValueConverter(); //Converter=InverseBoolean
    }
}
