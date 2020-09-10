using System;

namespace TodoList2.Desktop.Model
{
    public class NetworkException : Exception
    {
        public NetworkException(String message) : base(message)
        {
        }

        public NetworkException(Exception innerException) : base("Exception occurred.", innerException)
        {
        }
    }
}