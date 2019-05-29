using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Model.ViewModels
{
    public class FilmVM
    {
        public int Id { get; set; }

        [DisplayName("Tittel: ")]
        [Required(ErrorMessage = "Tittel må fylles ut")]
        public string Title { get; set; }

        [DisplayName("Beskrivelse: ")]
        [Required(ErrorMessage = "Beskrivelse må fylles ut")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DisplayName("URL til bilde")]
        [Required(ErrorMessage = "URL må fylles ut")]
        public string ImgURL { get; set; }

        [DisplayName("Sjanger: ")]
        [Required(ErrorMessage = "Sjanger(e) må velges")]
        public List<GenreVM> Genres { get; set; }

        [DisplayName("Pris: ")]
        [Required(ErrorMessage = "Pris må velges")]
        public int Price { get; set; }

        [DisplayName("Opprettet: ")]
        public string CreatedDate { get; set; }

        public string JsonSerialize { get; set; }

        public bool Active { get; set; } = true;

        public int PriceClassId { get; set; }
    }
}