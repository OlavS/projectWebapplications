using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels
{
    /// <summary>
    /// Viewmodel for the form the form Payment.cshtml,
    /// with validation rules.
    /// </summary>
    public class PayVM : IValidatableObject
    {
        [DisplayName("Kortnummer:")]
        [Required(ErrorMessage = "Kortnummer må oppgis")]
        [RegularExpression(@"[0-9]{16}", ErrorMessage = "Kortnummeret er ugyldig")]
        public string CardNumber { get; set; }

        [DisplayName("CVC:")]
        [Required(ErrorMessage = "CVC må oppgis")]
        [RegularExpression(@"[0-9]{3}", ErrorMessage = "CVC er ugyldig")]
        public string CVC { get; set; }

        [DisplayName("Korthavers navn:")]
        [Required(ErrorMessage = "Navn må oppgis")]
        [RegularExpression(@"^[ÆØÅæøåA-Za-z ,.'-]+$", ErrorMessage = "Navnet er ugyldig")]
        public string CardHoldersName { get; set; }

        [DisplayName("Måned:")]
        [Required(ErrorMessage = "Måned må oppgis")]
        public string Month { get; set; }

        [DisplayName("År:")]
        [Required(ErrorMessage = "År må oppgis")]
        public string Year { get; set; }

        /// <summary>
        /// Validating if the date given is valid compared to current date.
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var thisDate = DateTime.Now.Date;
            int month = Convert.ToInt32(Month);
            int year = Convert.ToInt32(Year);
            if (thisDate.Month >= month && thisDate.Year == year)
            {
                yield return new ValidationResult(errorMessage: "Kortet har gått ut, eller ugyldig dato er oppgitt!", memberNames: new[] { "Month" });
            }
        }
    }
}