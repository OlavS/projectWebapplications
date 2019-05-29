using DAL.DALModels;
using Model;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class GenreRepository : Interfaces.IGenreRepository
    {
        /// <summary>
        /// Search for genre VM based on the genre's id.
        /// </summary>
        /// <param name="id">Genre id to search for.</param>
        /// <returns>GenreVM</returns>
        public GenreVM GetGenre(int id)
        {
            using (var db = new VideoDB())
            {
                Genre genre = db.Genres.FirstOrDefault(g => g.Id == id);
                return ConvertGenre(genre);
            }
        }

        /// <summary>
        /// Search for genre VM based on the genre name.
        /// </summary>
        /// <param name="name">Genre name to search for.</param>
        /// <returns>GenreVM</returns>
        public GenreVM GetGenre(string name)
        {
            using (var db = new VideoDB())
            {
                Genre genre = db.Genres.FirstOrDefault(g => g.Name == name);
                return ConvertGenre(genre);
            }
        }

        /// <summary>
        /// Returns a list of genres, based on a sorting type.
        /// </summary>
        /// <param name="sort">Sorting type to base the sort on.</param>
        /// <returns>List of genre vms.</returns>
        /// <seealso cref="Props"/>
        public List<GenreVM> GetGenres(Sort sort = Sort.alfa)
        {
            using (var db = new VideoDB())
            {
                List<Genre> AllGenres = db.Genres.OrderBy(g => g.Name).ToList();
                AllGenres = SortGenre(AllGenres, sort);
                List<GenreVM> Genres = ConvertGenres(AllGenres);

                return Genres;
            }
        }

        /// <summary>
        /// Returnes a list of genre vms, based on a sorting type, and where the
        /// number of films associated with it is at a minimum of filmCount.
        /// </summary>
        /// <param name="filmCount">Minimum number of films associated with the genre.</param>
        /// <param name="sort">Sorting algorithm to base the list on.</param>
        /// <returns>A list of genre vms</returns>
        /// <seealso cref="Props"/>
        public List<GenreVM> GetGenres(int filmCount, Sort sort = Sort.alfa)
        {
            using (var db = new VideoDB())
            {
                List<Genre> GenresOverCount = db.Genres.Where(g => g.Films.Count() >= filmCount).ToList();
                GenresOverCount = SortGenre(GenresOverCount, sort);
                List<GenreVM> GenreList = ConvertGenres(GenresOverCount);

                return GenreList;
            }
        }

        #region Helper Methods

        /// <summary>
        /// Sorting the genre list based on a sorting type.
        /// </summary>
        /// <param name="genreList">List to sort</param>
        /// <param name="sort">Sorting type to use.</param>
        /// <returns>List of genres</returns>
        /// <seealso cref="Props"/>
        private List<Genre> SortGenre(List<Genre> genreList, Sort sort)
        {
            switch (sort)
            {
                case Sort.none:
                    return genreList.OrderBy(g => g.Id).ToList();

                case Sort.alfa:
                    return genreList.OrderBy(g => g.Name).ToList();

                case Sort.tilf:
                    var rnd = new Random();
                    return genreList.OrderBy(item => rnd.Next()).ToList();

                default:
                    return genreList;
            }
        }

        private List<GenreVM> ConvertGenres(List<Genre> inList)
        {
            List<GenreVM> outList = new List<GenreVM>();
            foreach (var genre in inList)
            {
                outList.Add(ConvertGenre(genre));
            }

            return outList;
        }

        private GenreVM ConvertGenre(Genre inGenre)
        {
            var Filmrepo = new FilmRepository();
            GenreVM Genre = new GenreVM
            {
                Id = inGenre.Id,
                Name = inGenre.Name
            };

            return Genre;
        }

        #endregion Helper Methods
    }
}