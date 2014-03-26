namespace TestTutorial.Converters
{
	public static class Converters
    {
        public static readonly StringLengthValueConverter StringLength = new StringLengthValueConverter(); //Converter=StringLength
        public static readonly StringReverseValueConverter StringReverse = new StringReverseValueConverter();  //Converter=StringReverse
        public static readonly FloatConverter Float = new FloatConverter();  //Converter=Float
        public static readonly IntConverter Int = new IntConverter(); //Converter=Int
        public static readonly IntToFloatConverter IntToFloat = new IntToFloatConverter(); //Converter=IntToFloat
		public static readonly StringFormatValueConverter StringFormat = new StringFormatValueConverter(); //Converter=StringFormat
		public static readonly InverseBooleanValueConverter InverseBoolean = new InverseBooleanValueConverter(); //Converter=InverseBoolean
    }
}
