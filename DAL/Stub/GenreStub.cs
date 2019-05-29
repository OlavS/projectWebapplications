using Model;
using Model.ViewModels;
using System;
using System.Collections.Generic;

namespace DAL.Stub
{
    public class GenreStub : Interfaces.IGenreRepository
    {
        public GenreVM GetGenre(int id)
        {
            throw new NotImplementedException();
        }

        public GenreVM GetGenre(string name)
        {
            throw new NotImplementedException();
        }

        public List<GenreVM> GetGenres(Sort sort = Sort.alfa)
        {
            throw new NotImplementedException();
        }

        public List<GenreVM> GetGenres(int filmCount, Sort sort = Sort.alfa)
        {
            throw new NotImplementedException();
        }
    }
}