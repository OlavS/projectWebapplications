using System.ComponentModel;

namespace Model.ViewModels
{
    public class GenreVM
    {
        public int Id { get; set; }

        [DisplayName("Sjangernavn: ")]
        public string Name { get; set; }
    }
}