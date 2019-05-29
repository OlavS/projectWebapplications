using Model.ViewModels;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IUserBLL
    {
        string CreateUser(UserVM newCustomer);

        bool EditUser(int id, EditUserVM inCustomer);

        bool FindEmail(string email);

        List<int> ListOfFilmIds(List<FilmVM> films);

        UserVM UserLogIn(LogInVM logInDetails);

        List<UserVM> AllUser();

        UserVM GetUserVM(int customerId);

        bool ResetPassword(UserVM customer);

        bool CheckPersonalia(UserVM customer);

        bool ToggleActivate(int id);

        List<UserVM> ToggleActivate(int id, List<UserVM> users);

        EditUserVM GetEditUser(int id);

        bool ResetPassword(PassWordChangeVM passwordChangeVM);

        bool IsAdmin(int id);
    }
}