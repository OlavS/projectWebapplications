using DAL.DALModels;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;

namespace DAL.Repositories
{
    /// <summary>
    /// Handles the database's customer.
    /// </summary>
    public class UserRepository : Interfaces.IUserRepository
    {
        //Finner norsk tidsone
        private readonly TimeZoneInfo tZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

        /// <summary>
        /// Allows creation of a customer in the database.
        /// </summary>
        /// <param name="inUser">Customer information</param>
        /// <returns>A empty string if successful, else a string containing an appropriate error message to the user.</returns>
        public string CreateUser(UserVM inUser)
        {
            try
            {
                var email = inUser.Email;
                bool emailFound = FindEmail(email);

                if (emailFound == true)
                {
                    return "Epostadressen du har oppgitt finnes allerede i databasen.";
                }

                byte[] salt = CreateSalt();
                byte[] passWord = CreateHashedPassword(inUser.PassWord, salt);

                using (var db = new VideoDB())
                {
                    var newUserReg = new User
                    {
                        FirstName = inUser.FirstName,
                        SurName = inUser.SurName,
                        Address = inUser.Address,
                        PhoneNr = inUser.PhoneNr,
                        Email = inUser.Email,
                        Admin = inUser.Admin,
                        Salt = salt,
                        PassWord = passWord
                    };

                    PostalAddress findPostPlace = db.PostalAddresses.Find(inUser.PostalNr);

                    if (findPostPlace == null)
                    {
                        var newPostal = new PostalAddress
                        {
                            PostalCode = inUser.PostalNr,
                            Postal = inUser.Postal
                        };
                        newUserReg.PostalAddress = newPostal;
                    }
                    else
                    {
                        newUserReg.PostalAddress = db.PostalAddresses.Find(inUser.PostalNr);
                    }

                    db.Users.Add(newUserReg);
                    db.SaveChanges();
                    return "";
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.UserDAL.CreateUser(UserVM inUser)", inUser.ToString(), e);
                return "Kunne ikke lagre kunden i databasen, vennligst kontakt kundeservice!";
            }
        }

        /// <summary>
        /// Grants editable User object.
        /// </summary>
        /// <param name="inUser"></param>
        /// <returns></returns>
        public bool EditUser(int id, EditUserVM inUser)
        {
            using (var db = new VideoDB())
            {
                try
                {
                    User user = db.Users.Find(id);
                    user.FirstName = inUser.FirstName;
                    user.SurName = inUser.SurName;
                    user.Address = inUser.Address;

                    PostalAddress findPostPlace = db.PostalAddresses.Find(inUser.PostalNr);

                    if (findPostPlace == null)
                    {
                        var newPostal = new PostalAddress
                        {
                            PostalCode = inUser.PostalNr,
                            Postal = inUser.Postal
                        };
                        db.PostalAddresses.Add(newPostal);
                        user.PostalAddress = newPostal;
                    }
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    new ErrorLogRepository().CreateError("Oblig1.DAL.UserDAL.EditUser(int id, UserVM inUser)", inUser.ToString() + " | id:" + id, e);
                    return false;
                }
            }
        }

        /// <summary>
        /// Creates a list of film ids.
        /// </summary>
        /// <param name="films">A list of filmVMs to base the id list on.</param>
        /// <returns>A lsit of film ids</returns>
        public List<int> ListOfFilmIds(List<FilmVM> films)
        {
            var ownedFilmIdList = new List<int>();
            foreach (var item in films)
            {
                ownedFilmIdList.Add(item.Id);
            }
            return ownedFilmIdList;
        }

        /// <summary>
        /// Checks if a email exists in the database.
        /// </summary>
        /// <param name="email">Email to check</param>
        /// <returns>False if the email doesnt exist, else true.
        /// </returns>
        public bool FindEmail(String email)
        {
            using (var db = new VideoDB())
            {
                var User = db.Users.FirstOrDefault(k => k.Email == email);
                if (User != null)
                {
                    return true;
                }
            }
            return false;
        }

        public User GetUser(string email)
        {
            using (var db = new VideoDB())
            {
                return db.Users.FirstOrDefault(k => k.Email == email);
            }
        }

        /// <summary>
        /// Creates a hashed password.
        /// The hashing algorithm runs 1000 times for improved security.
        /// </summary>
        /// <param name="passWord">Plaintext password, to base it on.</param>
        /// <param name="salt">SALT to base the password on.</param>
        /// <returns>A hashed password.</returns>
        [NonAction]
        public byte[] CreateHashedPassword(string passWord, byte[] salt)
        {
            const int keyLength = 24;
            var pbkdf = new Rfc2898DeriveBytes(passWord, salt, 1000);
            return pbkdf.GetBytes(keyLength);
        }

        /// <summary>
        /// Creates a random salt.
        /// </summary>
        /// <returns>A random SALT.</returns>
        [NonAction]
        public byte[] CreateSalt()
        {
            var csprng = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csprng.GetBytes(salt);
            return salt;
        }

        /// <summary>
        /// Gets all users from the database.
        /// </summary>
        /// <returns>A list of UserVMs.</returns>
        public List<UserVM> AllUsers()
        {
            using (var db = new VideoDB())
            {
                List<User> User = db.Users.ToList();

                List<UserVM> UserVMLIst = ConvertUsers(User);

                return UserVMLIst;
            }
        }

        /// <summary>
        /// Helpermethod that converts a list of DB-model User to the Domain-Model equivalent.
        /// </summary>
        /// <param name="users"></param>
        /// <seealso cref="ConvertUser(User)"/>
        /// <returns></returns>
        private List<UserVM> ConvertUsers(List<User> users)
        {
            var outUsers = new List<UserVM>();
            foreach (var user in users)
            {
                outUsers.Add(ConvertUser(user));
            };

            return outUsers;
        }

        /// <summary>
        /// Helpermethod that converts a DB-Model film into a DomainModel equivalent.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>UserVM</returns>
        private UserVM ConvertUser(User user)
        {
            var cVM = new UserVM()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                SurName = user.SurName,
                PostalNr = user.PostalAddress.PostalCode,
                Postal = user.PostalAddress.Postal,
                Address = user.Address,
                Email = user.Email,
                PhoneNr = user.PhoneNr,
                Active = user.Active,
                Admin = user.Admin,
                CreatedDate = TimeZoneInfo.ConvertTimeFromUtc(user.CreatedDate, tZone).ToString("dd.MM.yyyy HH:mm:ss")
            };
            return cVM;
        }

        /// <summary>
        /// Get UserVM based on userID parameter.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public UserVM GetUserVM(int userID)
        {
            using (var db = new VideoDB())
            {
                User c = db.Users.FirstOrDefault(K => K.Id == userID);
                return ConvertUser(c);
            }
        }

        /// <summary>
        /// Resets password with new values in UserVM
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool ResetPassWord(UserVM user)
        {
            try
            {
                using (var db = new VideoDB())
                {
                    byte[] salt = CreateSalt();
                    byte[] password = CreateHashedPassword(user.PassWord, salt);

                    var cDB = db.Users.FirstOrDefault(c => c.Email == user.Email);
                    cDB.Salt = salt;
                    cDB.PassWord = password;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.UserDAL.ResetPassWord(userVM User)", user.ToString(), e);
                return false;
            }
        }

        /// <summary>
        /// Test if user has inputed correct credentials.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool CheckPersonalia(UserVM user)
        {
            var dbUser = GetUser(user.Email);
            if (dbUser == null ||
                dbUser.FirstName != user.FirstName ||
                dbUser.SurName != user.SurName ||
                dbUser.PhoneNr != user.PhoneNr) return false;

            return true;
        }

        /// <summary>
        /// Sets user.active to true, false.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ToggleActivate(int id)
        {
            try
            {
                using (var db = new VideoDB())
                {
                    var user = db.Users.FirstOrDefault(c => c.Id == id);
                    user.Active = !user.Active;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.UserDAL.ToggleActivate(int  id)", id.ToString(), e);
                return false;
            }
        }

        /// <summary>
        /// Get EditUserVM where UserVM is input id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EditUserVM GetEditUser(int id)
        {
            UserVM existing = GetUserVM(id);
            var editUserVM = new EditUserVM()
            {
                FirstName = existing.FirstName,
                SurName = existing.SurName,
                Address = existing.Address,
                Postal = existing.Postal,
                PostalNr = existing.PostalNr,
                Email = existing.Email,
                Id = existing.Id,
                PhoneNr = existing.PhoneNr,
            };

            return editUserVM;
        }

        /// <summary>
        /// Changes password for user with passwordChangeVM.id. With the new password in passwordChangeVM.PassWord
        /// </summary>
        /// <param name="passwordChangeVM"></param>
        /// <returns></returns>
        public bool ResetPassWord(PassWordChangeVM passwordChangeVM)
        {
            try
            {
                using (var db = new VideoDB())
                {
                    byte[] salt = CreateSalt();
                    byte[] password = CreateHashedPassword(passwordChangeVM.PassWord, salt);

                    var cDB = db.Users.FirstOrDefault(c => c.Id == passwordChangeVM.Id);
                    cDB.Salt = salt;
                    cDB.PassWord = password;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.UserDAL.ResetPassWord(PassWordChangeVM passwordChangeVM))", passwordChangeVM.ToString(), e);
                return false;
            }
        }

        /// <summary>
        /// Check if user is admin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsAdmin(int id)
        {
            try
            {
                using (var db = new VideoDB())
                {
                    var user = db.Users.Find(id);

                    return user.Admin;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.UserDAL.IsAdmin(int id))", id.ToString(), e);
                return false;
            }
        }

        /// <summary>
        /// Count number of users.
        /// </summary>
        /// <returns></returns>
        public int GetUserCount()
        {
            try
            {
                using (var db = new VideoDB())
                {
                    return db.Users.Count();
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Dal.UserRepository.GetUserCount()", null, e);
                return 0;
            }
        }
    }
}