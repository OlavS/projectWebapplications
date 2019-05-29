using Model;
using Model.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DAL.Stub
{
    public class FilmStub : Interfaces.IFilmRepository
    {
        public FilmVM GetFilm(int filmId)
        {
            throw new NotImplementedException();
        }

        public string EditFilm(FilmVM newFilm, int priceClassId, int[] genreIds)
        {
            throw new NotImplementedException();
        }

        public bool ToggleActivate(int id)
        {
            if (id >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool GetAddFilmTest = true;

        public List<AddFilmVM> GetAllAddFilmVMs()
        {
            var addFilmVMListe = new List<AddFilmVM>();

            List<SelectListItem> PriceClassSelectList = new List<SelectListItem>();
            PriceClassSelectList.Add(new SelectListItem
            {
                Value = "1",
                Text = "Prisklasse 1"
            });
            PriceClassSelectList.Add(new SelectListItem
            {
                Value = "2",
                Text = "Prisklasse 2"
            });

            List<SelectListItem> GenreSelectList = new List<SelectListItem>();
            GenreSelectList.Add(new SelectListItem
            {
                Value = "1",
                Text = "Sjanger 1"
            });
            GenreSelectList.Add(new SelectListItem
            {
                Value = "2",
                Text = "Sjanger 2"
            });
            GenreSelectList.Add(new SelectListItem
            {
                Value = "3",
                Text = "Sjanger 3"
            });
            GenreSelectList.Add(new SelectListItem
            {
                Value = "4",
                Text = "Sjanger 4"
            });

            List<GenreVM> Genres = new List<GenreVM>();
            Genres.Add(new GenreVM
            {
                Id = 1,
                Name = "Sjanger 1"
            });
            Genres.Add(new GenreVM
            {
                Id = 2,
                Name = "Sjanger 2"
            });

            int[] genreIds = { 1, 2 };

            List<GenreVM> Genres2 = new List<GenreVM>();
            Genres2.Add(new GenreVM
            {
                Id = 3,
                Name = "Sjanger 3"
            });
            Genres2.Add(new GenreVM
            {
                Id = 4,
                Name = "Sjanger 4"
            });

            int[] genreIds2 = { 3, 4 };

            AddFilmVM newFilm = new AddFilmVM()
            {
                FilmId = 1,
                Title = "FilmTittel",
                Description = "Beskrivelse av filmen",
                ImgURL = "www.google.com/bilde.jpg",
                CurrentGenres = Genres2,
                Active = true,
                FilmPriceClassId = 1,
                PriceId = 1,
                PriceSelectList = PriceClassSelectList,
                GenreIDs = genreIds2,
                GenreSelectList = GenreSelectList
            };
            newFilm.JsonSerialize = JsonConvert.SerializeObject(newFilm);

            AddFilmVM newFilm2 = new AddFilmVM()
            {
                FilmId = 2,
                Title = "IkkeFilmTittel",
                Description = "Beskrivelse av filmen 2",
                ImgURL = "www.google.com/bilde2.jpg",
                CurrentGenres = Genres,
                Active = true,
                FilmPriceClassId = 1,
                PriceId = 1,
                PriceSelectList = PriceClassSelectList,
                GenreIDs = genreIds,
                GenreSelectList = GenreSelectList
            };
            newFilm2.JsonSerialize = JsonConvert.SerializeObject(newFilm2);

            List<int> ids = new List<int>();
            for (int i = 0; i < Genres.Count; i++)
            {
                ids.Add(Genres[i].Id);
            }
            newFilm.CurrFilmGenreIds = ids;

            var size = 10;
            for (var i = 0; i < size; i++)
            {
                addFilmVMListe.Add(newFilm);
                addFilmVMListe.Add(newFilm2);
            }

            return addFilmVMListe;
        }

        public AddFilmVM GetAddFilmVM()
        {
            if (GetAddFilmTest)
            {
                var addFilmVM = new AddFilmVM();

                List<SelectListItem> PriceClassSelectList = new List<SelectListItem>();
                PriceClassSelectList.Add(new SelectListItem
                {
                    Value = "1",
                    Text = "Prisklasse 1"
                });
                PriceClassSelectList.Add(new SelectListItem
                {
                    Value = "2",
                    Text = "Prisklasse 2"
                });

                List<SelectListItem> GenreSelectList = new List<SelectListItem>();
                GenreSelectList.Add(new SelectListItem
                {
                    Value = "1",
                    Text = "Sjanger 1"
                });
                GenreSelectList.Add(new SelectListItem
                {
                    Value = "2",
                    Text = "Sjanger 2"
                });
                addFilmVM.PriceSelectList = PriceClassSelectList;
                addFilmVM.GenreSelectList = GenreSelectList;

                return addFilmVM;
            }
            else
            {
                return null;
            }
        }

        public AddFilmVM GetEditFilm(int filmId)
        {
            GetAddFilmTest = !GetAddFilmTest;
            if (filmId == 1)
            {
                AddFilmVM ExAddFilmVM = new AddFilmVM()
                {
                    Active = true,
                    CurrFilmGenreIds = null,
                    CurrentGenres = null,
                    Description = "Beskrivelse",
                    FilmId = 0,
                    FilmPriceClassId = 0,

                    GenreSelectList = null,
                    ImgURL = "www.google.com/bilde.jpg",
                    JsonSerialize = null,
                    PriceId = 1,
                    PriceSelectList = null,
                    Title = "FilmTittel"
                };
                List<int> ids = new List<int>();
                for (int i = 1; i < 4; i++)
                {
                    ids.Add(i);
                }
                ExAddFilmVM.CurrFilmGenreIds = ids;

                List<GenreVM> Genres = new List<GenreVM>();
                Genres.Add(new GenreVM
                {
                    Id = 1,
                    Name = "Sjanger 1"
                });
                Genres.Add(new GenreVM
                {
                    Id = 2,
                    Name = "Sjanger 2"
                });
                ExAddFilmVM.CurrentGenres = Genres;
                int[] genreIds = { 1, 2 };
                ExAddFilmVM.GenreIDs = genreIds;

                List<SelectListItem> PriceClassSelectList = new List<SelectListItem>();
                PriceClassSelectList.Add(new SelectListItem
                {
                    Value = "1",
                    Text = "Prisklasse 1"
                });
                PriceClassSelectList.Add(new SelectListItem
                {
                    Value = "2",
                    Text = "Prisklasse 2"
                });

                List<SelectListItem> GenreSelectList = new List<SelectListItem>();
                GenreSelectList.Add(new SelectListItem
                {
                    Value = "1",
                    Text = "Sjanger 1"
                });
                GenreSelectList.Add(new SelectListItem
                {
                    Value = "2",
                    Text = "Sjanger 2"
                });
                ExAddFilmVM.PriceSelectList = PriceClassSelectList;
                ExAddFilmVM.GenreSelectList = GenreSelectList;

                return ExAddFilmVM;
            }
            else
            {
                return null;
            }
        }

        public string EditFilm(AddFilmVM editAddFilm, int priceClassId, int[] genreIds)
        {
            if (editAddFilm.Title == "FilmTittel")
            {
                return "";
            }
            else
            {
                return "Error";
            }
        }

        public int GetFilmCount()
        {
            return 20;
        }

        public List<FilmVM> GetUserFilms(string email)
        {
            throw new NotImplementedException();
        }

        public List<FilmVM> GetFilms(Sort sort = Sort.none)
        {
            throw new NotImplementedException();
        }

        public List<FilmVM> GetFilms(string genreName, Sort sort = Sort.none)
        {
            throw new NotImplementedException();
        }

        public List<FilmVM> SearchByTitle(string searchString, Sort sort = Sort.alfa)
        {
            throw new NotImplementedException();
        }

        public List<FilmVM> SortFilmVM(List<FilmVM> filmList, Sort sort)
        {
            throw new NotImplementedException();
        }

        public AddFilmVM GetNewAddFilmVM()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetPriceClassSelectList()
        {
            throw new NotImplementedException();
        }

        public List<SelectListItem> GetGenreSelectList()
        {
            throw new NotImplementedException();
        }

        public string CreateFilm(AddFilmVM newFilm, int priceClassId, int[] genreIds)
        {
            if (newFilm.Title == "FilmTittel")
            {
                return "";
            }
            else
            {
                return "Error";
            }
        }
    }
}