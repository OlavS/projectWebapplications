using DAL.DALModels;
using Model;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;

namespace DAL.Repositories
{
    public class ErrorLogRepository : Interfaces.IErrorLogRepository
    {
        private TimeZoneInfo tZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

        /// <summary>
        /// Gets all error items in the database.
        /// </summary>
        /// <returns>A list of all the errors</returns>
        public List<ErrorLogVM> AllErrors()
        {
            try
            {
                using (var db = new VideoDB())
                {
                    List<ErrorLogVM> errorLogs = new List<ErrorLogVM>();
                    foreach (var log in db.ErrorLogs)
                    {
                        errorLogs.Add(new ErrorLogVM()
                        {
                            Id = log.Id,
                            Message = log.Message,
                            Parameter = log.Parameter,
                            StackTrace = log.StackTrace,
                            Time = TimeZoneInfo.ConvertTimeFromUtc(log.Time, tZone).ToString("dd.MM.yyyy HH:mm:ss")
                        });
                    }

                    return errorLogs;
                }
            }
            catch (Exception e)
            {
                CreateError("Oblig1.DAL.Repositories.ErrorRepository.AllChanges", null, e);
                return new List<ErrorLogVM>();
            }
        }

        /// <summary>
        /// Creates a errorlog item and saves it to  a new string line in ~/App_Data/Error.txt and to the database.
        /// </summary>
        /// <param name="message">Location of the error.</param>
        /// <param name="parameter">Parameters for the error instance.</param>
        /// <param name="e">Exception from the error.</param>
        /// <returns>If passed true.</returns>
        public bool CreateError(string message, string parameter, Exception e)
        {
            //If there is an error in the CreateError, then it should crash here.
            //Because errors are no longer logged, and the integrity is invalid.
            Console.Error.WriteLine(e.StackTrace);
            try
            {
                using (var db = new VideoDB())
                {
                    Error error = new Error
                    {
                        Message = message,
                        Parameter = parameter,
                        StackTrace = e.StackTrace
                    };

                    using (StreamWriter writetext = new StreamWriter(HostingEnvironment.MapPath("~/App_Data/Error.txt"), true))
                    {
                        writetext.WriteLine(error.ToString() + Environment.NewLine);
                    }

                    db.ErrorLogs.Add(error);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw new DatabaseErrorException(message);
            }
            throw new DatabaseErrorException(message);
        }
    }
}