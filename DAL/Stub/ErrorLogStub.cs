using Model.ViewModels;
using System;
using System.Collections.Generic;

namespace DAL.Stub
{
    public class ErrlorLogStub : Interfaces.IErrorLogRepository
    {
        public List<ErrorLogVM> AllErrors()
        {
            return new List<ErrorLogVM>()
            {
                new ErrorLogVM()
                {
                    Id = 1,
                    Message = "Test1",
                    Parameter = "TestParameter1",
                    StackTrace = "TestSTack",
                    Time = "20.10.2018 01:01:01"
                }, new ErrorLogVM()
                {
                    Id = 2,
                    Message = "Test2",
                    Parameter = "TestParameter2",
                    StackTrace = "TestSTack",
                    Time = "20.10.2018 01:01:02"
                }, new ErrorLogVM()
                {
                    Id = 3,
                    Message = "Test3",
                    Parameter = "TestParameter3",
                    StackTrace = "TestSTack",
                    Time = "20.10.2018 01:01:03"
                }
            };
        }

        public bool CreateError(string message, string parameter, Exception e)
        {
            return true;
        }
    }
}