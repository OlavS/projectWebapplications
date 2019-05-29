using Model;
using Model.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DAL.Interfaces
{
    public interface IFilmRepository
    {
        List<FilmVM> GetFilms(Sort sort = Sort.none);

        List<FilmVM> GetFilms(string genreName, Sort sort = Sort.none);

        List<FilmVM> GetUserFilms(string email);

        List<FilmVM> SortFilmVM(List<FilmVM> filmList, Sort sort);

        List<FilmVM> SearchByTitle(string searchString, Sort sort = Sort.alfa);

        List<SelectListItem> GetPriceClassSelectList();

        List<SelectListItem> GetGenreSelectList();

        string CreateFilm(AddFilmVM newFilm, int priceClassId, int[] genreIds);

        string EditFilm(AddFilmVM newFilm, int priceClassId, int[] genreIds);

        FilmVM GetFilm(int filmId);

        bool ToggleActivate(int id);

        List<AddFilmVM> GetAllAddFilmVMs();

        AddFilmVM GetAddFilmVM();

        AddFilmVM GetEditFilm(int filmId);

        int GetFilmCount();
    }
}