using DAL.DALModels;
using Model.ViewModels;
using System;

namespace DAL.Stub
{
    public class LogOnStub : Interfaces.ILogOnRepository
    {
        public bool ControlPassWord(User customer, string passWord)
        {
            throw new NotImplementedException();
        }

        public UserVM UserLogIn(LogInVM logInInfo)
        {
            throw new NotImplementedException();
        }
    }
}