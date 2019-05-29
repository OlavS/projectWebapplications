using System;

namespace Model
{
    public class DatabaseErrorException : Exception
    {
        private new readonly string Message;

        public DatabaseErrorException(string message) : base(message)
        {
            Message = message;
        }

        public string GetMessage()
        {
            return Message;
        }
    }
}