using System.ComponentModel;

namespace Model.ViewModels
{
    /// <summary>
    /// Viewmodel for ShoppingCartTable.
    /// </summary>
    public class ShoppingCartVM
    {
        public int Id { get; set; }

        [DisplayName("Titler:")]
        public string Title { get; set; }

        [DisplayName("Pris:")]
        public int Price { get; set; }

        public string ImgURL { get; set; }
    }
}