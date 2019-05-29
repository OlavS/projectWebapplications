using Model;
using Model.ViewModels;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IFilmBLL
    {
        List<FilmVM> SearchByTitle(string searchString, Sort sorting = Sort.alfa);

        FilmListVM GetUserFilmList(string eMailAddress);

        List<FilmVM> AllOfUsersFilms(string eMailAddress);

        List<GenreListVM> GetGenreListView(int filmCount, Sort sortGenre = Sort.tilf, Sort sortFilms = Sort.tilf);

        FilmListVM GetFilmViewList(Sort sort = Sort.none);

        FilmListVM GetFilmViewList(string genreName, Sort sort = Sort.none);

        FilmListVM GetFilmViewList(FilmListVM listview, Sort sort = Sort.none);

        AddFilmVM GetAddFilmVM();

        string CreateFilm(AddFilmVM newFilm);

        FilmVM GetFilmVM(int filmId);

        string EditFilm(AddFilmVM newFilm);

        List<AddFilmVM> GetAllAddFilmVMs();

        bool ToggleActivate(int id);

        List<AddFilmVM> ToggleActivate(int id, List<AddFilmVM> filmList);

        AddFilmVM GetEditFilm(int filmId);
    }
}