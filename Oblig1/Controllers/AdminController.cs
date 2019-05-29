using BLL.BLL;
using BLL.Interfaces;
using Model;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Oblig1.Controllers
{
    public class AdminController : Controller
    {
        private IChangeLogBLL _changeLogLogic;
        private IErrorLogBLL _errorLogLogic;
        private IFilmBLL _filmLogic;
        private IMenuBLL _menyLogic;
        private IOrderBLL _orderLogic;
        private IPriceBLL _priceLogic;
        private IUserBLL _UserLogic;

        public AdminController()
        {
            this._changeLogLogic = new ChangeLogBLL();
            this._errorLogLogic = new ErrorLogBLL();
            this._filmLogic = new FilmBLL();
            this._menyLogic = new MenuBLL();
            this._orderLogic = new OrderBLL();
            this._priceLogic = new PriceBLL();
            this._UserLogic = new UserBLL();
        }

        public AdminController(IChangeLogBLL CS, IErrorLogBLL ES, IFilmBLL FS, IMenuBLL MS, IOrderBLL OS, IPriceBLL PS, IUserBLL US)
        {
            _changeLogLogic = CS;
            _errorLogLogic = ES;
            _filmLogic = FS;
            _menyLogic = MS;
            _orderLogic = OS;
            _priceLogic = PS;
            _UserLogic = US;
        }

        //------------------ADMIN

        #region ADMIN

        /// <summary>
        /// Controller-method for the AdminFrontPage view.
        /// </summary>
        /// <returns>AdminFrontPage view</returns>
        public ActionResult AdminFrontPage()
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                AdminFrontPageVM frontPageVM = _menyLogic.GetFrontPageVM();
                return View(frontPageVM);
            }
            catch (DatabaseErrorException e)
            {
                return RedirectToAction("ShowMessage", "Home", new
                {
                    header = "Noe gikk galt under opprettelsen av kunden!", message = e.GetMessage()
                });
            }
        }

        /// <summary>
        /// Clears the session variable and redirects to the
        /// main front page.
        /// </summary>
        /// <returns>A redirect to the main front page</returns>
        public ActionResult LogOut()
        {
            Session["Kunde"] = null;
            return RedirectToAction("Frontpage", "Film");
        }
        
        #endregion ADMIN

        //------------------USERS

        #region Users

        /// <summary>
        /// Retrieves a list of all users and displays the list in
        /// a view.
        /// </summary>
        /// <returns></returns>
        public ActionResult AllUsers()
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                List<UserVM> UserVMs = _UserLogic.AllUser();
                return View("Users", UserVMs);
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Brukerene kunne ikke hentes fra databasen. Se errorLog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        /// <summary>
        /// Shows selected Users.
        /// </summary>
        /// <param name="Users"></param>
        /// <returns></returns>
        public ActionResult CreateUser()
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }

            return View();
        }

        /// <summary>
        /// Create user.
        /// </summary>
        /// <param name="inUser"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(UserVM inUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var resultat = _UserLogic.CreateUser(inUser);

                    if (resultat == "")
                    {
                        TempData["message"] = "Registrering av ny kunde fullført!";

                        return RedirectToAction("AllUsers");
                    }
                    else
                    {
                        TempData["message"] = "Noe gikk galt under opprettelsen av kunden!";
                        TempData["errormessage"] = resultat;

                        return View();
                    }
                }
                return View();
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        /// <summary>
        /// Gets a wrapper class for editing a specific user that is
        /// retrieved by its primary key.
        /// </summary>
        /// <param name="id">Primary key for the user to edit</param>
        /// <returns>A view with the wrapper class for editing users</returns>
        public ActionResult EditUser(int id)
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                EditUserVM userEdited = _UserLogic.GetEditUser(id);
                return View(userEdited);
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        /// <summary>
        /// Changes the user values through the wrapper class for
        /// editing users.
        /// </summary>
        /// <param name="changedUser">Wrapper class with the edited values</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(EditUserVM changedUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = _UserLogic.EditUser(changedUser.Id, changedUser);

                    if (result)
                    {
                        TempData["message"] = "Bruker med id: " + changedUser.Id + " har nå blitt endret.";
                    }
                    else
                    {
                        TempData["message"] = "Noe gikk galt under endringen.";
                        TempData["errormessage"] = "Feilmelding lagret til logg.";
                    }
                    return RedirectToAction("AllUsers");
                }
                return View(changedUser);
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        /// <summary>
        /// Creates a wrapper class for resetting password for the user specified
        /// by the primary key provided as an argument.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ResetPassWord(int id)
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                var passwordChangeVM = new PassWordChangeVM
                {
                    Id = id
                };
                return View(passwordChangeVM);
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        /// <summary>
        /// Resets the password for a specified user contained in the
        /// wrapper class by its primary key.
        /// </summary>
        /// <param name="passwordChangeVM"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassWord(PassWordChangeVM passwordChangeVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool result = _UserLogic.ResetPassword(passwordChangeVM);

                    if (result)
                    {
                        TempData["message"] = "Endring av passord for bruker med id: " + passwordChangeVM.Id +
                                              " velykket.";
                        return View("EditUser", _UserLogic.GetEditUser(passwordChangeVM.Id));
                    }
                    else
                    {
                        TempData["message"] = "Noe gikk galt under passordendringen av kunden!";
                        TempData["errormessage"] = result.ToString();
                        return RedirectToAction("AllUsers");
                    }
                }
                return View();
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        /// <summary>
        /// Search for User/s by the UserVM attributes strings
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public ActionResult SearchUsers(string searchString)
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                IGenericBLL<UserVM> _genericLogic = new GenericBLL<UserVM>();
                return View("Users", _genericLogic.Search(searchString, _UserLogic.AllUser()));
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        /// <summary>
        /// Activate/Inactivates user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ToggleActivateUser(int id)
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                if (!_UserLogic.ToggleActivate(id))
                {
                    TempData["message"] = "Noe gikk galt under endringen.";
                    TempData["errormessage"] = "Feilmelding lagret til logg.";
                }
                else
                {
                    TempData["message"] = "Brukeren er nå deaktivert/aktivert.";
                    return View("Users", _UserLogic.ToggleActivate(id, (List<UserVM>)TempData["UserVMs"]));
                }
                return View("Users", (List<UserVM>)TempData["UserVMs"]);
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        #endregion Users

        //------------------FILMS

        #region Films

        /// <summary>
        /// Gets a new AddFilmVM object that is used to create a
        /// new film object based on existing price classes and
        /// genres.
        /// Films handles exceptions in the filmrepository with a false return statement.
        /// </summary>
        /// <returns>ActionResult with the wrapper class for adding a film</returns>
        [HttpGet]
        public ActionResult AddFilm()
        {
            #region IsAdminCheck

            if (Session["Kunde"] == null)
                return RedirectToAction("Frontpage", "Film");

            var user = (UserVM)Session["Kunde"];
            int userId = user.Id;
            if (!_UserLogic.IsAdmin(userId))
                return RedirectToAction("Frontpage", "Film");

            #endregion IsAdminCheck

            var addFilmVM = _filmLogic.GetAddFilmVM();
            if (addFilmVM != null)
            {
                return View(addFilmVM);
            }
            else
            {
                TempData["message"] = "Feil ved lasting av skjema for film!";
                TempData["errormessage"] = "Feilmelding lagret til logg.";

                return RedirectToAction("ListAllFilms");
            }
        }

        /// <summary>
        /// Gets the wrapper class for creating new films and adds the new film
        /// to the database through the BLL.
        /// </summary>
        /// <param name="newFilm">Wrapper class for the new film</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFilm(AddFilmVM newFilm)
        {
            if (ModelState.IsValid)
            {
                var result = _filmLogic.CreateFilm(newFilm);
                if (result == "")
                {
                    TempData["message"] = "Registrering av ny film fullført!";
                    return RedirectToAction("ListAllFilms");
                }
                else
                {
                    TempData["message"] = "Feil ved registrering av ny film!";
                    TempData["errormessage"] = "Feilmelding lagret til logg.";
                    return RedirectToAction("AddFilm");
                }
            }

            var addFilmVM = _filmLogic.GetAddFilmVM();
            if (addFilmVM != null)
            {
                TempData["message"] = "Feil ved registrering av ny film!";
                TempData["errormessage"] = "Vennligst sjekk at alle felter er utfylt";
                newFilm.PriceSelectList = addFilmVM.PriceSelectList;
                newFilm.GenreSelectList = addFilmVM.GenreSelectList;
                return View(newFilm);
            }
            else
            {
                TempData["message"] = "Feil ved lasting av skjema for film!";
                TempData["errormessage"] = "Feilmelding lagret til logg.";
                return RedirectToAction("ListAllFilms");
            }
        }

        /// <summary>
        /// Creates the wrapper class with the appropriate information from
        /// the film given by the primary key as parameter to the method.
        /// </summary>
        /// <param name="film">Primary key of the film to edit</param>
        /// <returns></returns>
        public ActionResult EditFilm(int film)
        {
            #region IsAdminCheck

            if (Session["Kunde"] == null)
                return RedirectToAction("Frontpage", "Film");

            var user = (UserVM)Session["Kunde"];
            int userId = user.Id;
            if (!_UserLogic.IsAdmin(userId))
                return RedirectToAction("Frontpage", "Film");

            #endregion IsAdminCheck

            AddFilmVM addFilmVM = _filmLogic.GetEditFilm(film);
            if (addFilmVM != null)
            {
                return View("EditFilm", addFilmVM);
            }
            else
            {
                TempData["message"] = "Feil ved endring av film!";
                TempData["errormessage"] = "Feilmelding lagret til logg.";
                return RedirectToAction("AdminFrontPage");
            }
        }

        /// <summary>
        /// Receives the edited film inside the wrapper class. If changing the
        /// film was successful, a redirect to the list of films occur.
        /// </summary>
        /// <param name="filmVM">The wrapper class for editing films</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFilm(AddFilmVM filmVM)
        {
            if (ModelState.IsValid)
            {
                var result = _filmLogic.EditFilm(filmVM);
                if (result == "")
                {
                    TempData["message"] = "Endringer gjennomført";
                    return RedirectToAction("ListAllFilms");
                }
                else
                {
                    TempData["message"] = "Feil ved endring av film!";
                    TempData["errormessage"] = "Feilmelding lagret til logg.";
                    return RedirectToAction("ListAllFilms");
                }
            }
            TempData["message"] = "Endringer kunne ikke gjennomføres.";
            TempData["errormessage"] = "Vennligst sjekk at alle felter er utfylt.";
            return RedirectToAction("ListAllFilms");
        }

        /// <summary>
        /// Lists all the films in the database through a View-Model and
        /// returns the View-Model representation to the caller.
        /// </summary>
        /// <returns>A View-Model representation of all the films</returns>
        public ActionResult ListAllFilms()
        {
            #region IsAdminCheck

            if (Session["Kunde"] == null)
                return RedirectToAction("Frontpage", "Film");

            var user = (UserVM)Session["Kunde"];
            int userId = user.Id;
            if (!_UserLogic.IsAdmin(userId))
                return RedirectToAction("Frontpage", "Film");

            #endregion IsAdminCheck

            List<AddFilmVM> addFilmVMs = _filmLogic.GetAllAddFilmVMs();
            return View("AllFilms", addFilmVMs);
        }

        /// <summary>
        /// Searches the database for films associated with the provided argument and
        /// thereafter returns the a view with the resulting list of films.
        /// </summary>
        /// <param name="searchString">The string to be associated with films</param>
        /// <returns></returns>
        public ActionResult SearchFilm(string searchString)
        {
            #region IsAdminCheck

            if (Session["Kunde"] == null)
                return RedirectToAction("Frontpage", "Film");

            var user = (UserVM)Session["Kunde"];
            int userId = user.Id;
            if (!_UserLogic.IsAdmin(userId))
                return RedirectToAction("Frontpage", "Film");

            #endregion IsAdminCheck

            IGenericBLL<AddFilmVM> _genericLogic = new GenericBLL<AddFilmVM>();
            return View("AllFilms", _genericLogic.Search(searchString, _filmLogic.GetAllAddFilmVMs()));
        }

        /// <summary>
        /// Toggles the Active variable of the film specified by the parameter to the
        /// method.
        /// </summary>
        /// <param name="filmId">Primary key of the film to activate/deactivate</param>
        /// <returns></returns>
        public ActionResult ToggleActivateFilm(int filmId)
        {
            #region IsAdminCheck

            if (Session["Kunde"] == null)
                return RedirectToAction("Frontpage", "Film");

            var user = (UserVM)Session["Kunde"];
            int userId = user.Id;
            if (!_UserLogic.IsAdmin(userId))
                return RedirectToAction("Frontpage", "Film");

            #endregion IsAdminCheck

            if (!_filmLogic.ToggleActivate(filmId))
            {
                TempData["message"] = "Noe gikk galt under aktivering/deaktivering av film";
                return View("AllFilms", (List<AddFilmVM>)TempData["AddFilmVMs"]);
            }
            TempData["message"] = "Aktivering/deaktivering gjennomført";
            return View("AllFilms", _filmLogic.ToggleActivate(filmId, (List<AddFilmVM>)TempData["AddFilmVMs"]));
        }

        #endregion Films

        //------------------ORDERS

        #region Orders

        /// <summary>
        /// View with all orders.
        /// </summary>
        /// <returns></returns>
        public ActionResult AllOrders()
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                List<ExpandedOrderVM> allOrders = _orderLogic.AllOrders();
                return View("Orders", allOrders);
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        /// <summary>
        /// Refund film from order.
        /// </summary>
        /// <param name="ordernr">Ordernr to remove from</param>
        /// <param name="title">Film title to remove</param>
        /// <returns>Orders view</returns>
        public ActionResult RefundFilm(int ordernr, string title)
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                if (_orderLogic.RefundFilm(ordernr, title))
                {
                    var orders =
                        _orderLogic.RemoveFilmFromOrder(ordernr, title,
                            (List<ExpandedOrderVM>)TempData["ExpandedOrderVM"]);
                    TempData["message"] = title + " ble refundert fra ordre " + ordernr;
                    return View("Orders", orders);
                }

                TempData["errormessage"] = "Noe gikk galt under fjerningen av film " + title + " fra ordre " + ordernr;
                return View("Orders", (List<ExpandedOrderVM>)TempData["ExpandedOrderVM"]);
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        /// <summary>
        /// Refund order, based on order nr.
        /// </summary>
        /// <param name="ordernr">Order nr to remove</param>
        /// <returns>Orders</returns>
        public ActionResult RefundOrder(int ordernr)
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                if (_orderLogic.RefundOrder(ordernr))
                {
                    var orders =
                        _orderLogic.RemoveOrderNr(ordernr, (List<ExpandedOrderVM>)TempData["ExpandedOrderVM"]);
                    TempData["message"] = "Ordre " + ordernr + " har blitt refundert";
                    return View("Orders", orders);
                }

                TempData["errormessage"] = "Noe gikk galt under refunderingen av ordrenr: " + ordernr;
                return View("Orders", (List<ExpandedOrderVM>)TempData["ExpandedOrderVM"]);
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        /// <summary>
        /// Search orders, based on searchstring passed from the layout page.
        /// </summary>
        /// <param name="searchString">To base the search on.</param>
        /// <returns>Orders view.</returns>
        public ActionResult SearchOrders(string searchString)
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                IGenericBLL<ExpandedOrderVM> _genericLogic = new GenericBLL<ExpandedOrderVM>();
                if (!string.IsNullOrEmpty(searchString))
                {
                    TempData["message"] = "Søkeresultat for: " + searchString;
                }

                return View("Orders", _genericLogic.Search(searchString, _orderLogic.AllOrders()));
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        #endregion Orders

        //------------------CHANGELOG

        #region ChangeLog

        /// <summary>
        /// Get view of all changes
        /// </summary>
        /// <returns></returns>
        public ActionResult AllChangeLogs()
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                List<ChangeLogVM> changes = _changeLogLogic.AllChanges();
                return View("ChangeLog", changes);
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        /// <summary>
        /// Return View with a searched changelog-list
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public ActionResult SearchChangeLog(string searchString)
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                IGenericBLL<ChangeLogVM> _genericLogic = new GenericBLL<ChangeLogVM>();
                return View("ChangeLog", _genericLogic.Search(searchString, _changeLogLogic.AllChanges()));
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        #endregion ChangeLog

        //------------------ERRORLOG

        #region ErrorLoG

        /// <summary>
        /// Get view of all ErrorVMs
        /// </summary>
        /// <returns></returns>
        public ActionResult AllErrorLogs()
        {
            List<ErrorLogVM> errorLog = null;
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                errorLog = _errorLogLogic.AllErrors();
                //dummy error.
                _errorLogLogic.CreateError("public ActionResult AllErrorLogs()", "dummy_error", new Exception());
                return View("ErrorLog", errorLog);
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Feilmeldingshåndtering er operativ.";
                TempData["errormessage"] = e.GetMessage();
                return View("ErrorLog", errorLog);
            }
        }

        /// <summary>
        /// Get view with selected errorlogList by searchstring.
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public ActionResult SearchErrorLog(string searchString)
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                IGenericBLL<ErrorLogVM> _genericLogic = new GenericBLL<ErrorLogVM>();
                return View("ErrorLog", _genericLogic.Search(searchString, _errorLogLogic.AllErrors()));
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        #endregion ErrorLoG

        //------------------PRICECLASS

        #region PriceClass

        /// <summary>
        /// Gets a list of the View-Model representation of a price class.
        /// This View-Model is then returned to the view.
        /// </summary>
        /// <returns>A list of View-Model representations</returns>
        public ActionResult PriceClassesChange()
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                List<PriceClassChangeVM> PriceList = _priceLogic.GetPriceChangeVMs();

                return View(PriceList);
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        /// <summary>
        /// Receives an editet list of price classes and a command in the form of a string
        /// that is evaluated and decides if the changes should be completed. A message in
        /// a TempData variable lets the user know if the change was successful.
        /// </summary>
        /// <param name="changeList">The list of edited price classes</param>
        /// <param name="command">The string command to decide if the changes should be completed</param>
        /// <returns>A redirect if the ModelState is valid, the same view if not</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PriceClassesChange(List<PriceClassChangeVM> changeList, string command)
        {
            if (command == "Avbryt")
            {
                return RedirectToAction("PriceClassesList");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    if (_priceLogic.ChangePrices(changeList))
                    {
                        TempData["message"] = "Prisendring gjennomført!";
                    }
                    else
                    {
                        TempData["message"] = "Noe gikk galt under endring av priser!";
                    }

                    return RedirectToAction("PriceClassesList", "Admin");
                }
            }

            return View(changeList);
        }

        /// <summary>
        /// Gets a list of the current price classes stored in the database and
        /// returns the list to the appropriate view.
        /// </summary>
        /// <returns>A view with the current price classes</returns>
        public ActionResult PriceClassesList()
        {
            try
            {
                #region IsAdminCheck

                if (Session["Kunde"] == null)
                    return RedirectToAction("Frontpage", "Film");

                var user = (UserVM)Session["Kunde"];
                int userId = user.Id;
                if (!_UserLogic.IsAdmin(userId))
                    return RedirectToAction("Frontpage", "Film");

                #endregion IsAdminCheck

                List<PriceClassVM> PriceList = _priceLogic.GetPriceClasses();
                return View(PriceList);
            }
            catch (DatabaseErrorException e)
            {
                TempData["message"] = "Databasefeil. Se errorlog.";
                TempData["errormessage"] = e.GetMessage();
                return RedirectToAction("AdminFrontPage");
            }
        }

        #endregion PriceClass
    }
}