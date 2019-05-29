using System.Collections.Generic;

namespace Model.ViewModels
{
    public class GenreListVM
    {
        public string Name { get; set; }
        public List<FilmVM> Films { get; set; }
    }
}