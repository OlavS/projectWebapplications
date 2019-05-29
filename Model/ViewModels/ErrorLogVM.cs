using System.ComponentModel;

namespace Model.ViewModels
{
    public class ErrorLogVM
    {
        [DisplayName("Id:")]
        public int Id { get; set; }

        [DisplayName("Feilmelding:")]
        public string Message { get; set; }

        [DisplayName("Tidspunkt:")]
        public string Time { get; set; }

        [DisplayName("Parameter:")]
        public string Parameter { get; set; }

        [DisplayName("Stacktrace:")]
        public string StackTrace { get; set; }

        public override string ToString()
        {
            return Id + " " + Message + " " + Parameter + " " + StackTrace + " " +
                   Time;
        }
    }
}