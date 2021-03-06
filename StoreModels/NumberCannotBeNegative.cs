namespace StoreModels
{
    /// <summary>
    /// My custom exception when the user enters in a negative number where it shouldnt be
    /// </summary>
    public class NumberCannotBeNegative : System.Exception
    {
        public NumberCannotBeNegative() { }
        public NumberCannotBeNegative(string message) : base(message) { }
        public NumberCannotBeNegative(string message, System.Exception inner) : base(message, inner) { }
        protected NumberCannotBeNegative(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}