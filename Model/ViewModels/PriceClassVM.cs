using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels
{
    public class PriceClassVM
    {
        [Display(Name = "Prisklasse")]
        public int Id { get; set; }

        [Display(Name = "Pris")]
        public int Price { get; set; }
    }
}