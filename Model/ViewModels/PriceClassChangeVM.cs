using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels
{
    public class PriceClassChangeVM
    {
        [Display(Name = "Prisklasse")]
        public int Id { get; set; }

        [Display(Name = "Pris")]
        public int Price { get; set; }

        [Display(Name = "Ny pris")]
        [RegularExpression(@"^\d{0,4}$", ErrorMessage = "Ny pris ugyldig")]
        public int NewPrice { get; set; }
    }
}