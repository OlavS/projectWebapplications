using Model.ViewModels;
using System;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IErrorLogRepository
    {
        List<ErrorLogVM> AllErrors();

        bool CreateError(string message, string parameter, Exception e);
    }
}