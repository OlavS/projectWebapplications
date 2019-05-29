using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels
{
    /// <summary>
    /// Viewmodel that is used to grant login functionality to the user.
    /// </summary>
    [DisplayName("Passord")]
    public class PassWordChangeVM
    {
        public int Id { get; set; }

        [DisplayName("Velg et passord: ")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Passord må oppgis")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Passordet må inneholde minst 6 tegn")]
        public string PassWord { get; set; }

        [DisplayName("Gjenta passordet: ")]
        [DataType(DataType.Password)]
        [CompareAttribute("PassWord", ErrorMessage = "Passordene er ikke like")]
        public string PassWordRepeat { get; set; }

        public override string ToString()
        {
            return Id + " " + PassWord + " " + PassWordRepeat;
        }
    }
}