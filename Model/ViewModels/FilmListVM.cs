using System.Collections.Generic;

namespace Model.ViewModels
{
    public class FilmListVM
    {
        public string HeadLine { get; set; }
        public string SortingText { get; set; }
        public List<FilmVM> Films { get; set; }
    }
}