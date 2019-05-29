using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels
{
    /// <summary>
    /// Viewmodel that is used to register new users, this model is also
    /// stored in Session[Kunde], with rules for validation and userinput.
    /// </summary>
    [DisplayName("Kunde")]
    public class UserVM
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Fornavn:")]
        [Required(ErrorMessage = "Fornavn må oppgis")]
        [RegularExpression(@"^[ÆØÅæøåA-Za-z ,.'-]+$", ErrorMessage = "Fornavnet er ugyldig")]
        public string FirstName { get; set; }

        [DisplayName("Etternavn:")]
        [Required(ErrorMessage = "Etternavn må oppgis")]
        [RegularExpression(@"^[ÆØÅæøåA-Za-z ,.'-]+$", ErrorMessage = "Etternavnet er ugyldig")]
        public string SurName { get; set; }

        [DisplayName("Adresse:")]
        [Required(ErrorMessage = "Adresse må oppgis")]
        [RegularExpression(@"^[ÆØÅæøåA-Za-z0-9 ,.'-]+$", ErrorMessage = "Adressen er ugyldig")]
        public string Address { get; set; }

        [DisplayName("Telefon:")]
        [Required(ErrorMessage = "Telefonnummer må oppgis")]
        [RegularExpression(@"[0-9]{8}", ErrorMessage = "Telefonnummeret er ugyldig")]
        public string PhoneNr { get; set; }

        [DisplayName("Postnummer:")]
        [Required(ErrorMessage = "Postnummer må oppgis")]
        [RegularExpression(@"[0-9]{4}", ErrorMessage = "Postnummeret er ugyldig")]
        public string PostalNr { get; set; }

        [DisplayName("Poststed:")]
        [Required(ErrorMessage = "Poststed må oppgis")]
        [RegularExpression(@"^[ÆØÅæøåA-Za-z ,.'-]+$", ErrorMessage = "Poststedet er ugyldig")]
        public string Postal { get; set; }

        [DisplayName("Epostadresse:")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Epostadressen er ugyldig")]
        [Required(ErrorMessage = "Epostadresse må oppgis")]
        public string Email { get; set; }

        [DisplayName("Velg et passord: ")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Passord må oppgis")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Passordet må inneholde minst 6 tegn")]
        public string PassWord { get; set; }

        [DisplayName("Gjenta passordet: ")]
        [DataType(DataType.Password)]
        [CompareAttribute("PassWord", ErrorMessage = "Passordene er ikke like")]
        public string PassWordRepeat { get; set; }

        public bool Admin { get; set; } = false;
        public bool Active { get; set; } = true;

        [DisplayName("Opprettet: ")]
        public string CreatedDate { get; set; }

        public override string ToString()
        {
            return FirstName + " " + SurName + " " + Email + " " + PhoneNr + " " +
                   Address + " " + Postal + " " + PostalNr;
        }
    }
}