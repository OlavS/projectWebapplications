using DAL.DALModels;
using Model;
using Model.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DAL.Repositories
{
    public class FilmRepository : Interfaces.IFilmRepository
    {
        //Finding Norwegian timezones:
        private readonly TimeZoneInfo tZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

        /// <summary>
        /// Gets all films in the database and returns them sorted if specified, or in the order
        /// they have in the database (no sorting/by Id) if not.
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public List<FilmVM> GetFilms(Sort sort = Sort.none)
        {
            try
            {
                using (var db = new VideoDB())
                {
                    List<Film> AllFilms = SortFilms(db.Films.ToList(), sort);
                    List<FilmVM> AllFilmVMs = ConvertFilms(AllFilms);

                    return AllFilmVMs;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.FilmDAL.GetFilms(Sorter sort = Sorter.none)", "null", e);
                return null;
            }
        }

        /// <summary>
        /// Gets all films in a genre and sorts it after the chosen sorting.
        /// </summary>
        /// <param name="genreName">The selected genre</param>
        /// <param name="sort">The sorting of the films</param>
        /// <seealso cref="GetFilms(Sorter)"/>
        /// <returns>A list of the appropriate films</returns>
        public List<FilmVM> GetFilms(string genreName, Sort sort = Sort.none)
        {
            try
            {
                using (var db = new VideoDB())
                {
                    Genre Genre = db.Genres.FirstOrDefault(g => g.Name == genreName);
                    List<Film> GenresFilms = SortFilms(Genre.Films.ToList(), sort);
                    List<FilmVM> GenreFilmList = ConvertFilms(GenresFilms);

                    return GenreFilmList;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.FilmDAL.GetFilms(string genreName, Sort sort = Sort.none)", "List<FilmVM>", e);
                return new List<FilmVM>();
            }
        }

        /// <summary>
        /// Creates a list of all the films a customer owns.
        /// </summary>
        /// <param name="email">The customer e-mail</param>
        /// <returns>A list of the customer's films</returns>
        public List<FilmVM> GetUserFilms(string email)
        {
            try
            {
                var filmList = new List<FilmVM>();
                var orderList = new List<Order>();

                using (var db = new VideoDB())
                {
                    var kunde = db.Users.FirstOrDefault(k => k.Email == email);
                    orderList = db.Orders.Where(o => o.User.Id == kunde.Id).ToList();

                    foreach (var ordre in orderList)
                    {
                        foreach (var ordrelinje in ordre.OrderLines)
                        {
                            filmList.Add(ConvertFilm(ordrelinje.Film));
                        }
                    }
                }
                return filmList;
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.FilmDAL.GetUserFilms(string email)", email, e);
                return new List<FilmVM>();
            }
        }

        /// <summary>
        /// Sorts a list of films via a switch statement using the provided sort argument and
        /// returns the sorted list.
        /// </summary>
        /// <param name="filmList"></param>
        /// <param name="sort"></param>
        /// <returns>Sortert liste med FilmVM</returns>
        public List<FilmVM> SortFilmVM(List<FilmVM> filmList, Sort sort)
        {
            switch (sort)
            {
                case Sort.none:
                    return filmList.OrderBy(o => o.Id).ToList();

                case Sort.alfa:
                    return filmList.OrderBy(o => o.Title).ToList();

                case Sort.pris:
                    return filmList.OrderBy(o => o.Price).ToList();

                case Sort.prisHøy:
                    return filmList.OrderByDescending(o => o.Price).ToList();

                case Sort.tilf:
                    var rnd = new System.Random();
                    return filmList.OrderBy(item => rnd.Next()).ToList();

                default:
                    return filmList;
            }
        }

        /// <summary>
        /// Searches for films in the database that starts with the searchString, then adds them to a list.
        /// </summary>
        /// <param name="searchString">The string to search for in the database</param>
        /// <param name="sort">The sorting of the films</param>
        /// <seealso cref="ConvertFilm(Film)"/>
        /// <returns>En liste med filmVM </returns>
        public List<FilmVM> SearchByTitle(string searchString, Sort sort = Sort.alfa)
        {
            try
            {
                var allFilmsWithString = new List<FilmVM>();
                using (var db = new VideoDB())
                {
                    foreach (var film in db.Films.ToList())
                    {
                        if (film.Title.ToLower().StartsWith(searchString.ToLower()))
                        {
                            allFilmsWithString.Add(ConvertFilm(film));
                        }
                        // Adds films that has a title that starts with "the" when the searchstring does not
                        else if (film.Title.ToLower().StartsWith("the") && !searchString.ToLower().StartsWith("the"))
                        {
                            if (film.Title.ToLower().Substring(4).StartsWith(searchString.ToLower()))
                            {
                                allFilmsWithString.Add(ConvertFilm(film));
                            }
                        }
                    }
                }
                return allFilmsWithString;
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.FilmDAL.SearchByTitle(string searchString, Sorter sort = Sorter.alfa)", searchString, e);
                return new List<FilmVM>();
            }
        }

        /// <summary>
        /// Returns a helper class for creating new film objects by collecting
        /// the possible choices that is needed when selecting price class and genre(s).
        /// </summary>
        /// <returns>List of possible price classes to chose from</returns>
        public List<SelectListItem> GetPriceClassSelectList()
        {
            try
            {
                using (var db = new VideoDB())
                {
                    List<SelectListItem> PriceClassSelectList = db.PriceClasses.AsNoTracking()
                            .OrderBy(n => n.Id)
                            .Select(n => new SelectListItem
                            {
                                Value = n.Id.ToString(),
                                Text = "Prisklasse " + n.Id + " (" + n.Price + " kr)"
                            }).ToList();
                    var listTop = new SelectListItem()
                    {
                        Value = null,
                        Text = "--- Velg Prisklasse ---"
                    };
                    PriceClassSelectList.Insert(0, listTop);

                    return PriceClassSelectList;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.FilmDAL.GetPriceClassSelectList()", "PriceClassSelectList", e);
                return null;
            }
        }

        /// <summary>
        /// Gets helper class for selecting possible genres for films.
        /// </summary>
        /// <returns>A list of possible genres</returns>
        public List<SelectListItem> GetGenreSelectList()
        {
            try
            {
                using (var db = new VideoDB())
                {
                    List<SelectListItem> GenreSelectList = db.Genres.AsNoTracking()
                        .OrderBy(n => n.Id)
                        .Select(n => new SelectListItem
                        {
                            Value = n.Id.ToString(),
                            Text = n.Name.ToString()
                        }).ToList();
                    return GenreSelectList;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.FilmDAL.GetGenreSelectList()", "GenreSelectList", e);
                return null;
            }
        }

        /// <summary>
        /// Gets a new instance of the wrapper class for creating/editing
        /// films. Returns null if errors occur.
        /// </summary>
        /// <returns>The wrapper class for creating/editing films</returns>
        public AddFilmVM GetAddFilmVM()
        {
            AddFilmVM newAddFilmVM = new AddFilmVM();
            try
            {
                using (var db = new VideoDB())
                {
                    List<SelectListItem> GenreSelectList = db.Genres.AsNoTracking()
                        .OrderBy(n => n.Id)
                        .Select(n => new SelectListItem
                        {
                            Value = n.Id.ToString(),
                            Text = n.Name.ToString()
                        }).ToList();

                    List<SelectListItem> PriceClassSelectList = db.PriceClasses.AsNoTracking()
                            .OrderBy(n => n.Id)
                            .Select(n => new SelectListItem
                            {
                                Value = n.Id.ToString(),
                                Text = "Prisklasse " + n.Id + " (" + n.Price + " kr)"
                            }).ToList();
                    var listTop = new SelectListItem()
                    {
                        Value = null,
                        Text = "--- Velg Prisklasse ---"
                    };
                    PriceClassSelectList.Insert(0, listTop);

                    newAddFilmVM.PriceSelectList = PriceClassSelectList;
                    newAddFilmVM.GenreSelectList = GenreSelectList;

                    return newAddFilmVM;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.FilmDAL.GetAddFilmVM()", "Kunne ikke opprette AddFilmVM", e);
                return null;
            }
        }

        /// <summary>
        /// Creates a new film from the provided data and initializes a new row in the film
        /// database table with the required information returned from the view.
        /// </summary>
        /// <param name="newAddFilm">The wrapper class for creating/editing films</param>
        /// <param name="priceClassId">PK for the selected price class</param>
        /// <param name="genreIds">PKs for the selected genres</param>
        /// <returns>An empty string if the operations were successful, otherwise an error message</returns>
        public string CreateFilm(AddFilmVM newAddFilm, int priceClassId, int[] genreIds)
        {
            var newFilm = new FilmVM()
            {
                Title = newAddFilm.Title,
                Description = newAddFilm.Description,
                ImgURL = newAddFilm.ImgURL
            };

            try
            {
                using (var db = new VideoDB())
                {
                    var newDBFilm = new Film
                    {
                        Title = newFilm.Title,
                        Description = newFilm.Description,
                        ImgURL = newFilm.ImgURL
                    };

                    PriceClass findPriceClass = db.PriceClasses.Find(priceClassId);
                    newDBFilm.PriceClasses = findPriceClass;

                    newDBFilm.PriceClassId = priceClassId;

                    List<Genre> genreList = new List<Genre>();
                    List<int> genreIdList = new List<int>();
                    foreach (int genreId in genreIds)
                    {
                        genreList.Add(db.Genres.Find(genreId));
                        genreIdList.Add(genreId);
                    }

                    newDBFilm.Genres = genreList;
                    string gIDs = "";

                    if (genreIds.Length > 0)
                    {
                        gIDs = string.Join(" ", genreIds);
                    }

                    newDBFilm.GenreIds = gIDs;
                    db.Films.Add(newDBFilm);
                    db.SaveChanges();
                    return "";
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.CustomerDAL.CreateFilm(FilmVM newFilm)", newFilm.ToString(), e);
                return "Kunne ikke lagre filmen i databasen, vennligst kontakt kundeservice!";
            }
        }

        /// <summary>
        /// Populates an instance of the wrapper class for creating/editing
        /// films with the appropriate information from the film to be edited.
        /// </summary>
        /// <param name="filmId">PK for the film to be edited</param>
        /// <returns>An instnace of the wrapper class for creating/editing films</returns>
        public AddFilmVM GetEditFilm(int filmId)
        {
            FilmVM editFilmVM = GetFilm(filmId);
            var addFilmVM = GetAddFilmVM();
            if (editFilmVM != null)
            {
                addFilmVM.FilmId = editFilmVM.Id;
                addFilmVM.Title = editFilmVM.Title;
                addFilmVM.Description = editFilmVM.Description;
                addFilmVM.ImgURL = editFilmVM.ImgURL;
                addFilmVM.PriceId = editFilmVM.PriceClassId;

                List<int> ids = new List<int>();
                for (int i = 0; i < editFilmVM.Genres.Count; i++)
                {
                    ids.Add(editFilmVM.Genres[i].Id);
                }
                addFilmVM.CurrFilmGenreIds = ids;
            }

            if (editFilmVM == null || addFilmVM == null)
            {
                return null;
            }
            else
            {
                return addFilmVM;
            }
        }

        /// <summary>
        /// Edits the row in the database table that corresponds to the provided input film
        /// with information encapsulated in the wrapper class object.
        /// </summary>
        /// <param name="editAddFilm">The wrapper class for creating/editing films</param>
        /// <param name="priceClassId">PK for the price class that is selected</param>
        /// <param name="genreIds">PKs for the selected genres</param>
        /// <returns>An empty string if successful, an error message if not</returns>
        public string EditFilm(AddFilmVM editAddFilm, int priceClassId, int[] genreIds)
        {
            var editFilm = new FilmVM()
            {
                Id = editAddFilm.FilmId,
                Title = editAddFilm.Title,
                Description = editAddFilm.Description,
                ImgURL = editAddFilm.ImgURL
            };
            try
            {
                using (var db = new VideoDB())
                {
                    Film film = db.Films.Find(editFilm.Id);
                    film.Title = editFilm.Title;
                    film.Description = editFilm.Description;
                    film.ImgURL = editFilm.ImgURL;

                    PriceClass findPriceClass = db.PriceClasses.Find(priceClassId);
                    film.PriceClasses = findPriceClass;
                    film.PriceClassId = priceClassId;

                    //Bug i EF? Må iterere gjennom listen for å nullstille
                    foreach (Genre g in film.Genres)
                    {
                    }

                    if (genreIds != null)
                    {
                        film.Genres.RemoveRange(0, film.Genres.Count);
                        foreach (int genreId in genreIds)
                        {
                            film.Genres.Add(db.Genres.Find(genreId));
                        }
                    }

                    string gIDs = "";

                    if (genreIds.Length > 0)
                    {
                        gIDs = string.Join(" ", genreIds);
                    }
                    film.GenreIds = gIDs;

                    db.SaveChanges();
                    return "";
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.FilmDAL.EditFilm(FilmVM newFilm)", editFilm.ToString(), e);
                return "Kunne ikke lagre filmen i databasen, vennligst kontakt kundeservice!";
            }
        }

        /// <summary>
        /// Retrieves a film from the database and then converts it to a
        /// View-Model representation of a film and then returns it to the
        /// caller.
        /// </summary>
        /// <param name="filmId">PK of the film to return</param>
        /// <returns>The FilmVM if successful, null if not successful</returns>
        public FilmVM GetFilm(int filmId)
        {
            try
            {
                var filmVM = new FilmVM();

                using (var db = new VideoDB())
                {
                    var film = db.Films.Find(filmId);
                    filmVM = ConvertFilm(film);
                }

                return filmVM;
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.FilmDAL.GetFilm(int  filmId)", filmId.ToString(), e);
                return null;
            }
        }

        /// <summary>
        /// Toggles the Active instance variable in the film to indicate
        /// whether or not the film is active.
        /// </summary>
        /// <param name="id">PK of the film to activate/deactivate</param>
        /// <returns>A bool indicating if the activation/deactivation was successful</returns>
        public bool ToggleActivate(int id)
        {
            try
            {
                using (var db = new VideoDB())
                {
                    var film = db.Films.FirstOrDefault(f => f.Id == id);
                    film.Active = !film.Active;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.FilmDAL.ToggleActivate(int  id)", id.ToString(), e);
                return false;
            }
        }

        /// <summary>
        /// Creates a list of AddFilmVMs in order to edit films from the list of films.
        /// </summary>
        /// <returns>A list of wrapper class objects to create/edit films</returns>
        public List<AddFilmVM> GetAllAddFilmVMs()
        {
            List<AddFilmVM> addFilmVMs = new List<AddFilmVM>();
            AddFilmVM addFilmVM = new AddFilmVM
            {
                PriceSelectList = GetPriceClassSelectList(),
                GenreSelectList = GetGenreSelectList()
            };

            FilmListVM allFilms = new FilmListVM()
            {
                HeadLine = "Alle filmer",
                SortingText = Props.GetSortingText(Sort.none),
                Films = GetFilms(Sort.none)
            };

            if (addFilmVM.PriceSelectList == null || addFilmVM.GenreSelectList == null || allFilms.Films == null)
            {
                return new List<AddFilmVM>();
            }
            else
            {
                foreach (FilmVM film in allFilms.Films)
                {
                    AddFilmVM newFilm = new AddFilmVM()
                    {
                        FilmId = film.Id,
                        Title = film.Title,
                        Description = film.Description,
                        ImgURL = film.ImgURL,
                        CurrentGenres = film.Genres,
                        Active = film.Active,
                        FilmPriceClassId = film.PriceClassId,
                        CreatedDate = film.CreatedDate,
                        PriceId = addFilmVM.PriceId,
                        PriceSelectList = addFilmVM.PriceSelectList,
                        GenreIDs = addFilmVM.GenreIDs,
                        GenreSelectList = addFilmVM.GenreSelectList,
                 
                    };
                    newFilm.JsonSerialize = JsonConvert.SerializeObject(newFilm);

                    List<int> ids = new List<int>();
                    for (int i = 0; i < film.Genres.Count; i++)
                    {
                        ids.Add(film.Genres[i].Id);
                    }
                    newFilm.CurrFilmGenreIds = ids;
                    addFilmVMs.Add(newFilm);
                }

                return addFilmVMs;
            }
        }

        /// <summary>
        /// Gets the current count of films in the database and returns the number to the caller.
        /// </summary>
        /// <returns>Number of films if successful, 0 otherwise</returns>
        public int GetFilmCount()
        {
            try
            {
                using (var db = new VideoDB())
                {
                    return db.Films.Count();
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Dal.UserRepository.GetFilmCount()", null, e);
                return 0;
            }
        }

        #region Helper Methods

        /// <summary>
        /// Helpermethod that converts a list of DB-model films to the Domain-Model equivalent.
        /// </summary>
        /// <param name="inFilms">List of DB films</param>
        /// <seealso cref="ConvertFilm(Film)"/>
        /// <returns>List of View-Model representation of films</returns>
        private List<FilmVM> ConvertFilms(List<Film> inFilms)
        {
            var outFilms = new List<FilmVM>();
            foreach (var film in inFilms)
            {
                outFilms.Add(ConvertFilm(film));
            };

            return outFilms;
        }

        /// <summary>
        /// Helper method that converts a DB-Model film into a DomainModel equivalent.
        /// </summary>
        /// <param name="film"></param>
        /// <returns>FilmVM</returns>
        private FilmVM ConvertFilm(Film film)
        {
            var filmVM = new FilmVM()
            {
                Id = film.Id,
                Title = film.Title,
                Description = film.Description,
                ImgURL = film.ImgURL,
                CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(film.CreatedDate, tZone).ToString("dd.MM.yyyy HH:mm:ss"),
                //Makes a list of all the genres the film is in.
                Genres = film.Genres.Select(s => new GenreVM()
                {
                    Id = s.Id,
                    Name = s.Name
                }).ToList(),
                Price = film.PriceClasses.Price,
                Active = film.Active,
                PriceClassId = film.PriceClasses.Id
            };
            filmVM.JsonSerialize = JsonConvert.SerializeObject(filmVM);
            return filmVM;
        }

        /// <summary>
        /// Helper method that sorts a list of films by the chosen sort enum.
        /// </summary>
        /// <param name="inList"></param>
        /// <param name="sort"></param>
        /// <returns>Sortert liste med FilmVM</returns>
        private List<Film> SortFilms(List<Film> filmList, Sort sort)
        {
            switch (sort)
            {
                case Sort.none:
                    return filmList.OrderBy(o => o.Id).ToList();

                case Sort.alfa:
                    return filmList.OrderBy(o => o.Title).ToList();

                case Sort.pris:
                    return filmList.OrderBy(o => o.PriceClasses.Price).ToList();

                case Sort.prisHøy:
                    return filmList.OrderByDescending(o => o.PriceClasses.Price).ToList();

                case Sort.tilf:
                    var rnd = new System.Random();
                    return filmList.OrderBy(item => rnd.Next()).ToList();

                default:
                    return filmList;
            }
        }

        #endregion Helper Methods
    }
}