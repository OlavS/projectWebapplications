using DAL.Interfaces;
using DAL.Repositories;
using Model.ViewModels;
using System.Collections.Generic;

namespace BLL.BLL
{
    /// <summary>
    /// Class for logic related to the representation of menus.
    /// </summary>
    public class MenuBLL : Interfaces.IMenuBLL
    {
        private IGenreRepository _genreRepository;
        private IUserRepository _userRepository;
        private IFilmRepository _filmRepository;
        private IOrderRepository _orderRepository;
        private IChangeLogRepository _changeLogRepository;

        //Constructor for FilmBLL using a repository using the DBcontext
        public MenuBLL()
        {
            this._genreRepository = new GenreRepository();
            this._userRepository = new UserRepository();
            this._filmRepository = new FilmRepository();
            this._orderRepository = new OrderRepository();
            this._changeLogRepository = new ChangeLogRepository();
        }

        //Constructor for FilmBLL using a repository with stubs (for testing)
        public MenuBLL(IGenreRepository GenreStub, IUserRepository UserStub, IFilmRepository FilmStub, IOrderRepository OrderStub, IChangeLogRepository ChangeLogStub)
        {
            this._genreRepository = GenreStub;
            this._userRepository = UserStub;
            this._filmRepository = FilmStub;
            this._orderRepository = OrderStub;
            this._changeLogRepository = ChangeLogStub;
        }

        /// <summary>
        /// Returns a list of genres with the set amount if films or more.
        /// </summary>
        /// <returns>A list of Film-objects by the different genres</returns>
        public List<GenreVM> GetDropdownList(int filmCount)
        {
            return _genreRepository.GetGenres(filmCount);
        }

        public AdminFrontPageVM GetFrontPageVM()
        {
            AdminFrontPageVM frontPageVM = new AdminFrontPageVM()
            {
                UserCount = _userRepository.GetUserCount(),
                FilmCount = _filmRepository.GetFilmCount(),
                OrdersCount = _orderRepository.GetOrderCount(),
                ChangeCount = _changeLogRepository.GetChangeCount()
            };

            return frontPageVM;
        }
    }
}