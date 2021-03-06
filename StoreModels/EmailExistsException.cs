namespace StoreModels
{
    /// <summary>
    /// My custom exception when the user enters an email that already exists
    /// </summary>
    public class EmailExistsException : System.Exception
    {
        public EmailExistsException() { }
        public EmailExistsException(string message) : base(message) { }
        public EmailExistsException(string message, System.Exception inner) : base(message, inner) { }
        protected EmailExistsException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}