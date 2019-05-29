using Model.ViewModels;
using System;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IErrorLogBLL
    {
        List<ErrorLogVM> AllErrors();

        bool CreateError(string message, string parameter, Exception e);

        void CreateError(string v, int filmId, Exception exception);
    }
}