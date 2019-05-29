using DAL.Interfaces;
using DAL.Repositories;
using Model.ViewModels;
using System;
using System.Collections.Generic;

namespace BLL.BLL
{
    public class ErrorLogBLL : Interfaces.IErrorLogBLL
    {
        private IErrorLogRepository _errorLogRepository;

        public ErrorLogBLL()
        {
            _errorLogRepository = new ErrorLogRepository();
        }

        public ErrorLogBLL(IErrorLogRepository errorLogStub)
        {
            _errorLogRepository = errorLogStub;
        }

        public List<ErrorLogVM> AllErrors()
        {
            return _errorLogRepository.AllErrors();
        }

        public bool CreateError(string message, string parameter, Exception e)
        {
            return _errorLogRepository.CreateError(message, parameter, e);
        }

        public void CreateError(string v, int filmId, Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}