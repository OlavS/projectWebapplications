using BLL.BLL;
using BLL.Interfaces;
using Model;
using Model.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Oblig1.Controllers
{
    /// <summary>
    /// Controller that coordinates how data is sent/received to/from the views.
    /// </summary>
    public class FilmController : Controller
    {
        private IFilmBLL _filmLogic;

        public FilmController()
        {
            this._filmLogic = new FilmBLL();
        }

        public FilmController(IFilmBLL filmStub)
        {
            this._filmLogic = filmStub;
        }

        /// <summary>
        /// Loads the front page view.
        /// </summary>
        /// <returns>A list of a list of FilmVMs</returns>
        public ActionResult Frontpage()
        {
            List<GenreListVM> vm = _filmLogic.GetGenreListView(4);
            return View(vm);
        }

        /// <summary>
        /// Makes a list of films in a genre and returns a view with the corresponding list
        /// to the view.
        /// </summary>
        /// <param name="genreName">Genre to get films from</param>
        /// <param name="sorting">The selected sorting of the films</param>
        /// <returns>View with FilmListVM for a genre</returns>
        public ActionResult GenreView(string genreName, Sort sorting)
        {
            FilmListVM list = _filmLogic.GetFilmViewList(genreName, sorting);
            return View("FilmListView", list);
        }

        /// <summary>
        /// Makes a list of all films with a specific sorting and returns the list
        /// in the specified view.
        /// </summary>
        /// <param name="sorting"></param>
        /// <returns>View with all the films</returns>
        public ActionResult AllFilmsView(Sort sorting)
        {
            FilmListVM list = _filmLogic.GetFilmViewList(sorting);
            return View("FilmListView", list);
        }

        /// <summary>
        /// Creates a view from the Session["filmListe"]-variable, the list of
        /// films in this variable is then sorted and returned.
        /// </summary>
        /// <param name="sorting">The selected sorting of the films</param>
        /// <returns>View with a sorted list of films</returns>
        public ActionResult FilmListView(Sort sorting)
        {
            FilmListVM list = (FilmListVM)Session["filmListe"];
            return View("FilmListView", _filmLogic.GetFilmViewList(list, sorting));
        }

        /// <summary>
        /// Receives a string that is used to filter films by title. A list of the
        /// corresponding films is created and returned to the caller.
        /// </summary>
        /// <param name="searchString">The string to search for</param>
        /// <seealso cref="FilmVM"/>
        /// <returns>Returns a partial view with a list of films</returns>
        public ActionResult Search(string searchString)
        {
            List<FilmVM> list = _filmLogic.SearchByTitle(searchString);
            return PartialView("Search", list);
        }

        /// <summary>
        /// Used to stop the search from returning to a partial view when the
        /// user clicks the "enter" button in the search field.
        /// </summary>
        /// <param name="searchString">The string to search for</param>
        /// <returns>Returns a partial view with a list of films</returns>
        [HttpPost]
        public ActionResult SearchSubmit(string searchString)
        {
            List<FilmVM> list = _filmLogic.SearchByTitle(searchString);
            return View("Search", list);
        }

        /// <summary>
        /// Creates a View-Model of the films that the customer owns and returns the
        /// list to a view.
        /// </summary>
        /// <returns>View with the customer films</returns>
        public ActionResult AllOfCustomersFilms()
        {
            var customer = (UserVM)Session["Kunde"];
            if (customer != null)
            {
                FilmListVM filmListeVisning = _filmLogic.GetUserFilmList(customer.Email);
                return View("AllOfCustomersFilms", filmListeVisning);
            }
            return RedirectToAction("Frontpage", "Film");
        }

        /// <summary>
        /// Sorts the films that are contained within the session variable
        /// and returns the sorted list to a view.
        /// </summary>
        /// <param name="sorting">The selected sorting</param>
        /// <returns>A list of the sorted films to a view</returns>
        public ActionResult AllOfCustomersFilmsSorted(Sort sorting)
        {
            var list = (FilmListVM)Session["filmListe"];
            return View("AllOfCustomersFilms", _filmLogic.GetFilmViewList(list, sorting));
        }
    }
}