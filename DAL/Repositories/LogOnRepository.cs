using DAL.DALModels;
using Model.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DAL.Repositories
{
    /// <summary>
    /// Handles the web applications login feature.
    /// </summary>
    public class LogOnRepository : Interfaces.ILogOnRepository
    {
        /// <summary>
        /// Authentication of user credentials on the credentials in the database.
        /// </summary>
        /// <param name="logInInfo">User credentials</param>
        /// <returns>A customer object if authentication passed, null if not.</returns>
        public UserVM UserLogIn(LogInVM logInInfo)
        {
            try
            {
                using (var db = new VideoDB())
                {
                    var email = logInInfo.Email;
                    var password = logInInfo.PassWord;
                    var dbUser = db.Users.FirstOrDefault(k => k.Email == logInInfo.Email);

                    if (dbUser == null || !dbUser.Active) return null;

                    if (dbUser != null && ControlPassWord(dbUser, password))
                    {
                        UserVM user = new UserVM
                        {
                            Id = dbUser.Id,
                            FirstName = dbUser.FirstName,
                            SurName = dbUser.SurName,
                            Address = dbUser.Address,
                            Email = email,
                            PhoneNr = dbUser.PhoneNr,
                            PostalNr = dbUser.PostalAddress.PostalCode,
                            Postal = dbUser.PostalAddress.Postal,
                            PassWord = null,
                            Admin = dbUser.Admin
                        };
                        return user;
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.LogOnRepository.UserLogIn(LogINVM  logininfo)", logInInfo.ToString(), e);
                return null;
            }
        }

        /// <summary>
        /// Authenticating the password hash in the database
        /// with the password the user typed in.
        /// </summary>
        /// <param name="user">Customer to be tested.</param>
        /// <param name="passWord">Plaintext password to be tested.</param>
        /// <returns>True if the test passes, false if not.</returns>
        [NonAction]
        public bool ControlPassWord(User user, string passWord)
        {
            byte[] testPassWord = (new UserRepository()).CreateHashedPassword(passWord, user.Salt);
            bool result = user.PassWord.SequenceEqual(testPassWord);
            return result;
        }
    }
}