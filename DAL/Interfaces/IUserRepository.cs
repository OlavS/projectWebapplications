using DAL.DALModels;
using Model.ViewModels;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IUserRepository
    {
        string CreateUser(UserVM inUser);

        bool EditUser(int id, EditUserVM inUser);

        bool FindEmail(string email);

        User GetUser(string email);

        byte[] CreateSalt();

        byte[] CreateHashedPassword(string passWord, byte[] salt);

        List<int> ListOfFilmIds(List<FilmVM> films);

        List<UserVM> AllUsers();

        UserVM GetUserVM(int inUserID);

        bool ResetPassWord(UserVM inUser);

        bool CheckPersonalia(UserVM inUser);

        bool ToggleActivate(int id);

        EditUserVM GetEditUser(int id);

        bool ResetPassWord(PassWordChangeVM passwordChangeVM);

        bool IsAdmin(int id);

        int GetUserCount();
    }
}