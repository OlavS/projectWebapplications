using Model;
using Model.ViewModels;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IGenreRepository
    {
        GenreVM GetGenre(int id);

        GenreVM GetGenre(string name);

        List<GenreVM> GetGenres(Sort sort = Sort.alfa);

        List<GenreVM> GetGenres(int filmCount, Sort sort = Sort.alfa);
    }
}