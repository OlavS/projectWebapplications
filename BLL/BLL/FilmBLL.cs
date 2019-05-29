using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Repositories;
using Model;
using Model.ViewModels;
using System.Collections.Generic;

namespace BLL.BLL
{
    /// <summary>
    /// Business Logic Layer administrating how View-Model Film-objects are fetched from the
    /// database and redirected to the Views.
    /// </summary>
    public class FilmBLL : IFilmBLL
    {
        private IFilmRepository _filmRepository;
        private IGenreRepository _genreRepository;

        //Constructor for FilmBLL using a repository using the DBcontext
        public FilmBLL()
        {
            this._filmRepository = new FilmRepository();
            this._genreRepository = new GenreRepository();
        }

        //Constructor for FilmBLL using a repository with stubs (for testing)
        public FilmBLL(IFilmRepository FilmStub, IGenreRepository GenreStub)
        {
            this._filmRepository = FilmStub;
            this._genreRepository = GenreStub;
        }

        /// <summary>
        /// Search for films in the database that has a title that starts with the string provided as an argument
        /// to the method. A film which has the searchString as the first part of its title is added to a list
        /// of View-Model films and that list is returned to the caller.
        /// </summary>
        /// <param name="searchString">A string that is compared to film titles</param>
        /// <param name="sorting">An enum that sets the sorting order</param>
        /// <seealso cref="FilmRepository.SearchByTitle(string, Sort)"/>
        /// <seealso cref="Sort"/>
        /// <returns>A list of View-Model Film-objects that have titles that starts with searchString</returns>
        public List<FilmVM> SearchByTitle(string searchString, Sort sorting = Sort.alfa)
        {
            return _filmRepository.SearchByTitle(searchString, sorting);
        }

        /// <summary>
        /// Fetching the films associated with a customer through the customers e-mail address.
        /// The films are sorted alphabetically and a View-Model of a list of Film-objects is returned.
        /// </summary>
        /// <param name="eMailAddress">The e-mail address of the corresponding customer</param>
        /// <returns>A View-Model of a list of Film-objects</returns>
        public FilmListVM GetUserFilmList(string eMailAddress)
        {
            List<FilmVM> userFilms = _filmRepository.GetUserFilms(eMailAddress);
            var filmListViewModel = new FilmListVM
            {
                HeadLine = "Mine filmer",
                SortingText = Props.GetSortingText(Sort.alfa),
                Films = _filmRepository.SortFilmVM(userFilms, Sort.alfa)
            };
            return filmListViewModel;
        }

        /// <summary>
        /// Creates a complete list of the films belonging to a customer identified by the
        /// customer e-mail address.
        /// </summary>
        /// <param name="eMailAddress">The customer e-mail address</param>
        /// <returns>A list of View-Model Film-objects</returns>
        public List<FilmVM> AllOfUsersFilms(string eMailAddress)
        {
            return _filmRepository.GetUserFilms(eMailAddress);
        }

        /// <summary>
        /// Creates a list of GenreListVMs containing all genres with more than the set amount of
        /// films, and a list of that genre's films. Both the order of the genres and the films
        /// can be set via the sort arguments. They both defaults to a random order.
        /// </summary>
        /// <param name="sortGenre">Sorting of the genres</param>
        /// <param name="sortFilms">Sorting of the films per genre</param>
        /// <returns></returns>
        public List<GenreListVM> GetGenreListView(int filmCount, Sort sortGenre = Sort.tilf, Sort sortFilms = Sort.tilf)
        {
            List<GenreListVM> GenreListView = new List<GenreListVM>();
            List<GenreVM> Genres = _genreRepository.GetGenres(filmCount, sortGenre);

            foreach (GenreVM genre in Genres)
            {
                GenreListView.Add(new GenreListVM()
                {
                    Name = genre.Name,
                    Films = _filmRepository.GetFilms(genre.Name, sortFilms)
                });
            }
            return GenreListView;
        }

        /// <summary>
        /// Fetches all View-Model representations of Film-objects in a single View-Model
        /// representation of a list of films.
        /// </summary>
        /// <param name="sorting">The sorting of the list</param>
        /// <seealso cref="FilmRepository.GetFilms(Sort)"/>
        /// <seealso cref="Props.GetSortingText(Sort)"/>
        /// <returns></returns>
        public FilmListVM GetFilmViewList(Sort sort = Sort.none)
        {
            FilmListVM FilmListViewModel = new FilmListVM
            {
                HeadLine = "Alle filmer",
                SortingText = Props.GetSortingText(sort),
                Films = _filmRepository.GetFilms(sort)
            };

            return FilmListViewModel;
        }

        /// <summary>
        /// Fetches all View-Model representations of Film-objects that are within a genre. The list of
        /// Film-objects are stored in a View-Model representation of a list of Film-objects, sorted by the
        /// provided sorting-parameter and returned.
        /// </summary>
        /// <param name="name">Name of genre</param>
        /// <param name="sorting">The sorting of the list</param>
        /// <seealso cref="GetFilmViewList(Sort)"/>
        /// <seealso cref="GetFilmViewList(FilmListVM, Sort)"/>
        /// <returns></returns>
        public FilmListVM GetFilmViewList(string genreName, Sort sort = Sort.none)
        {
            FilmListVM FilmListViewModel = new FilmListVM
            {
                HeadLine = genreName,
                SortingText = Props.GetSortingText(sort),
                Films = _filmRepository.GetFilms(genreName, sort)
            };

            return FilmListViewModel;
        }

        /// <summary>
        /// Sorts the View-Model representation of a list of Film-objects by the sorting that
        /// is provided as an argument and then returns a sorted View-Model representation of a list of films.
        /// </summary>
        /// <param name="listeVisning"></param>
        /// <param name="sorting">The sorting of the desired View-Model of the list</param>
        /// <seealso cref="GetFilmViewList(Sort)"/>
        /// <seealso cref="GetFilmViewList(string, Sort)"/>
        /// <returns></returns>
        public FilmListVM GetFilmViewList(FilmListVM listview, Sort sort = Sort.none)
        {
            FilmListVM FilmListViewModel = new FilmListVM
            {
                HeadLine = listview.HeadLine,
                SortingText = Props.GetSortingText(sort),
                Films = _filmRepository.SortFilmVM(listview.Films, sort)
            };

            return FilmListViewModel;
        }

        /// <summary>
        /// Returns a wrapper class for creating a new film with
        /// lists of SelectListItems to populate dropdown lists
        /// in the respective view.
        /// </summary>
        /// <returns>Wrapper class for creating a new film</returns>
        public AddFilmVM GetAddFilmVM()
        {
            return _filmRepository.GetAddFilmVM();
        }

        public string CreateFilm(AddFilmVM newFilm)
        {
            string result = _filmRepository.CreateFilm(newFilm, newFilm.PriceId, newFilm.GenreIDs);
            return result;
        }

        public FilmVM GetFilmVM(int filmId)
        {
            return _filmRepository.GetFilm(filmId);
        }

        public AddFilmVM GetEditFilm(int filmId)
        {
            AddFilmVM addFilmVM = _filmRepository.GetEditFilm(filmId);
            return addFilmVM;
        }

        public string EditFilm(AddFilmVM editFilm)
        {
            string result = _filmRepository.EditFilm(editFilm, editFilm.PriceId, editFilm.GenreIDs);
            return result;
        }

        public List<AddFilmVM> GetAllAddFilmVMs()
        {
            return _filmRepository.GetAllAddFilmVMs();
        }

        public bool ToggleActivate(int id)
        {
            return _filmRepository.ToggleActivate(id);
        }

        /// <summary>
        /// Activates or deactivates a film in a AddFilmVM list.
        /// </summary>
        /// <param name="id">Film id</param>
        /// <param name="filmList">List of films.</param>
        /// <returns></returns>
        public List<AddFilmVM> ToggleActivate(int id, List<AddFilmVM> filmList)
        {
            foreach (var film in filmList)
            {
                if (film.FilmId == id)
                {
                    film.Active = !film.Active;
                    return filmList;
                }
            }

            return filmList;
        }
    }
}