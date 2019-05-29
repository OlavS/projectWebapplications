using System;

namespace DAL.DALModels
{
    /// <summary>
    /// The database's errormessage model.
    /// A model that is used to track exception events.
    /// </summary>
    public class Error
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Parameter { get; set; }
        public string StackTrace { get; set; }
        public DateTime Time { get; internal set; } = DateTime.Now;

        override
        public string ToString()
        {
            return
                "Message: " + Message + "\r\n" +
                "Parameter: " + Parameter + "\r\n" +
                "StackTrace: " + StackTrace + "\r\n" +
                "DateTime: " + Time.ToString();
        }
    }
}