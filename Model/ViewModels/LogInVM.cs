using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels
{
    /// <summary>
    /// Viewmodel for LogOnModal.
    /// </summary>
    public class LogInVM
    {
        [DisplayName("Epostadresse:")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Epostadressen er ugyldig")]
        [Required(ErrorMessage = "Epostadresse må oppgis")]
        public string Email { get; set; }

        [DisplayName("Passord: ")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Passord må oppgis")]
        public string PassWord { get; set; }

        /// <summary>
        /// Testing if the user has typed in credentials in the form fields.
        /// </summary>
        /// <param name="inUser">Credentials from user.</param>
        /// <returns>Null if test passes, else a string with errormessages.</returns>
        public string FindEmptyFields(LogInVM inUser)
        {
            if (inUser.Email == null && inUser.PassWord == null)
            {
                return "Oppgi epostadresse og passord";
            }
            else if (inUser.PassWord == null)
            {
                return "Oppgi passord";
            }
            else if (inUser.Email == null)
            {
                return "Oppgi epostadresse";
            }
            return null;
        }
    }
}