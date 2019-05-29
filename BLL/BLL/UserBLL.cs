using DAL.Interfaces;
using DAL.Repositories;
using Model.ViewModels;
using System.Collections.Generic;

namespace BLL.BLL
{
    public class UserBLL : Interfaces.IUserBLL
    {
        private IUserRepository _userRepository;
        private ILogOnRepository _logOnRepository;

        public UserBLL()
        {
            _userRepository = new UserRepository();
            _logOnRepository = new LogOnRepository();
        }

        public UserBLL(IUserRepository customerStub, ILogOnRepository logOnStub)
        {
            _userRepository = customerStub;
            _logOnRepository = logOnStub;
        }

        //TEST v1 Onlycustomer

        public UserBLL(IUserRepository userStub)
        {
            _userRepository = userStub;
        }

        /// <summary>
        /// Creates a new customer by redirecting the View-Model customer-object to the
        /// appropriate method in the DAL.
        /// </summary>
        /// <param name="newUser">The new View-Model customer</param>
        /// <returns>Redirects an empty string from the method if the creation
        /// of a customer is successful and an appropriate error message otherwise.
        /// </returns>
        public string CreateUser(UserVM newUser)
        {
            string resultat = _userRepository.CreateUser(newUser);
            return resultat;
        }

        /// <summary>
        /// Change customer.
        /// </summary>
        /// <param name="inUser"></param>
        /// <returns></returns>
        public bool EditUser(int id, EditUserVM inUser)
        {
            bool endret = _userRepository.EditUser(id, inUser);
            return endret;
        }

        /// <summary>
        /// Uses the DAL to check if a customer is already registered with the provided e-mail address.
        /// </summary>
        /// <param name="email">The e-mail address to check</param>
        /// <returns>True if the e-mail address is already registered. False otherwise.</returns>
        public bool FindEmail(string email)
        {
            return _userRepository.FindEmail(email);
        }

        /// <summary>
        /// Uses the DAL to check if log-in credentials are valid.
        /// </summary>
        /// <param name="logInDetails"></param>
        /// <returns>The View-Model customer-object if the log-in credentials are valid. Null otherwise.</returns>
        public UserVM UserLogIn(LogInVM logInDetails)
        {
            return _logOnRepository.UserLogIn(logInDetails);
        }

        /// <summary>
        /// Converts a list of View-Model Film-objects to an int-list of their Ids.
        /// </summary>
        /// <param name="films">The list of View-Model Film-objects</param>
        /// <returns></returns>
        public List<int> ListOfFilmIds(List<FilmVM> films)
        {
            return _userRepository.ListOfFilmIds(films);
        }

        public UserVM GetUserVM(int UserId)
        {
            return _userRepository.GetUserVM(UserId);
        }

        /// <summary>
        /// Fetches all View-Model representations of Customer-objects in a single View-Model
        /// representation of a list of Customers.
        /// </summary>
        /// <returns></returns>
        public List<UserVM> AllUser()
        {
            return _userRepository.AllUsers();
        }

        /// <summary>
        /// Checks Email, Firstname, Surname, PhoneNr in CustomerVM
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool CheckPersonalia(UserVM user)
        {
            return _userRepository.CheckPersonalia(user);
        }

        /// <summary>
        /// Changes password for user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool ResetPassword(UserVM user)
        {
            return _userRepository.ResetPassWord(user);
        }

        /// <summary>
        /// Toogles user.active true or false
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ToggleActivate(int id)
        {
            return _userRepository.ToggleActivate(id);
        }

        /// <summary>
        /// Toggles user.active true or false, in a list of users.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="users"></param>
        /// <returns></returns>
        public List<UserVM> ToggleActivate(int id, List<UserVM> users)
        {
            foreach (var user in users)
            {
                if (user.Id == id)
                {
                    user.Active = !user.Active;
                    return users;
                }
            }

            return users;
        }

        /// <summary>
        /// Get EditUserVM  based on user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EditUserVM GetEditUser(int id)
        {
            return _userRepository.GetEditUser(id);
        }

        /// <summary>
        /// Resets password for user
        /// </summary>
        /// <param name="passwordChangeVM"></param>
        /// <returns></returns>
        public bool ResetPassword(PassWordChangeVM passwordChangeVM)
        {
            return _userRepository.ResetPassWord(passwordChangeVM);
        }

        /// <summary>
        /// Check if user is admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsAdmin(int id)
        {
            return _userRepository.IsAdmin(id);
        }
    }
}