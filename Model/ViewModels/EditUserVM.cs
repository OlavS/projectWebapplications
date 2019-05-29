using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels
{
    /// <summary>
    /// Viewmodel that is used to validate new users information, and its used in
    /// Session["Customer"].
    /// </summary>
    [DisplayName("Kunde")]
    public class EditUserVM
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
        public string Email { get; set; }

        public override string ToString()
        {
            return "EditCustomerVM: " + FirstName + " " + SurName + " " + Email + " " + PhoneNr + " " +
                   Address + " " + Postal + " " + PostalNr;
        }
    }
}