using DAL.DALModels;
using Model.ViewModels;

namespace DAL.Interfaces
{
    public interface ILogOnRepository
    {
        bool ControlPassWord(User customer, string passWord);

        UserVM UserLogIn(LogInVM logInInfo);
    }
}