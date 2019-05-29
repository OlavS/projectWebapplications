using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Model.ViewModels
{
    public class AddFilmVM
    {
        public int FilmId { get; set; }

        [DisplayName("Tittel: ")]
        [Required(ErrorMessage = "Tittel må fylles ut")]
        public string Title { get; set; }

        [DisplayName("Beskrivelse: ")]
        [Required(ErrorMessage = "Beskrivelse må fylles ut")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("URL til bilde")]
        [Required(ErrorMessage = "URL må fylles ut")]
        [RegularExpression(".+[.jpg|.jpeg|.png|.tiff|.bmp]$", ErrorMessage = "URL må være til et bilde")]
        public string ImgURL { get; set; }

        public int FilmPriceClassId { get; set; }

        public List<int> CurrFilmGenreIds { get; set; }

        public List<GenreVM> CurrentGenres { get; set; }

        [Display(Name = "Prisklasse")]
        [Required(ErrorMessage = "Prisklasse må fylles ut")]
        public int PriceId { get; set; }

        public List<SelectListItem> PriceSelectList { get; set; }

        [Display(Name = "Sjanger(e)")]
        [Required(ErrorMessage = "Sjanger(e) må fylles ut")]
        public int[] GenreIDs { get; set; }

        public List<SelectListItem> GenreSelectList { get; set; }

        public string JsonSerialize { get; set; }

        public bool Active { get; set; } = true;

        [DisplayName("Opprettet: ")]
        public string CreatedDate { get; set; }

        public override string ToString()
        {
            string genres = "";
            foreach (GenreVM genre in CurrentGenres)
            {
                genres += " " + genre.Name;
            }
            return Title +" "+CreatedDate + genres;
        }
    }
}
