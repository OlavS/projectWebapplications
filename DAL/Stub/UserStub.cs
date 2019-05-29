using DAL.DALModels;
using Model;
using Model.ViewModels;
using System;
using System.Collections.Generic;

namespace DAL.Stub
{
    public class UserStub : Interfaces.IUserRepository
    {
        public bool nullVal = true;

        public List<UserVM> AllUsers()
        {
            if (nullVal)
            {
                var userList = new List<UserVM>(){
            new UserVM()
            {
                Active = false,
                Id = 3,
                FirstName = "Jan",
                SurName = "Teigen",
                Address = "Nøtterøys veien 69",
                Postal = "Tønsberg",
                PostalNr = "3162",
                CreatedDate = "21.10.2018 14:49:31",
                PassWord = "MilEtterMil",
                PassWordRepeat = "MilEtterMil",
                Email = "teigen@gmail.com",
                PhoneNr = "81549300",
            },

                new UserVM()
                {
                    Active = false,
                    Id = 4,
                    FirstName = "Petter",
                    SurName = "Dass",
                    Address = "Olav Ryes gate 90",
                    Postal = "Tønsberg",
                    PostalNr = "3162",
                    CreatedDate = "23.10.2018 15:49:31",
                    PassWord = "DusjForheng",
                    PassWordRepeat = "DusjForheng",
                    Email = "Petter@Dass.com",
                    PhoneNr = "81122200",
                }
                };

                return userList;
            }
            else
            {
                throw new DatabaseErrorException("fail");
            }
        }

        public void CreateNull(bool val)
        {
            nullVal = val;
        }

        public bool CheckPersonalia(UserVM customer)
        {
            throw new NotImplementedException();
        }

        public string CreateUser(UserVM inCustomer)
        {
            if (inCustomer.Id == 666)
            {
                throw new DatabaseErrorException("Fail");
            }
            if (inCustomer.Id != 0)
            {
                return "";
            }
            else
            {
                CreateNull(false);
                return "false";
            }
        }

        public byte[] CreateHashedPassword(string passWord, byte[] salt)
        {
            throw new NotImplementedException();
        }

        public bool EditUser(int id, UserVM inUSer)
        {
            if (inUSer.FirstName == ""
                || inUSer.SurName == ""
                || inUSer.Email == ""
                || inUSer.PassWord == ""
                || inUSer.PhoneNr == ""
                || inUSer.Postal == ""
                || inUSer.PostalNr == ""
                || inUSer.Address == ""
                || inUSer.Id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool EditUser(int id, EditUserVM inCustomer)
        {
            if (id == 666)
            {
                throw new DatabaseErrorException("Fail");
            }
            if (id != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool FindEmail(string email)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string email)
        {
            throw new NotImplementedException();
        }

        public UserVM GetUserVM(int userId)
        {
            return new UserVM()
            {
                Active = false,
                Id = 3,
                FirstName = "Jan",
                SurName = "Teigen",
                Address = "Nøtterøys veien 69",
                Postal = "Tønsberg",
                PostalNr = "3162",
                CreatedDate = "21.10.2018 14:49:31",
                Admin = true,
                PassWord = "MilEtterMil",
                PassWordRepeat = "MilEtterMil",
                Email = "teigen@gmail.com",
                PhoneNr = "81549300",
            };
        }

        public EditUserVM GetEditUser(int id)
        {
            EditUserVM customer = new EditUserVM()
            {
                Id = 3,
                FirstName = "Jan",
                SurName = "Teigen",
                Address = "Nøtterøys veien 69",
                Postal = "Tønsberg",
                PostalNr = "3162",
                Email = "teigen@gmail.com",
                PhoneNr = "81549300",
            };
            return customer;
        }

        public byte[] CreateSalt()
        {
            throw new NotImplementedException();
        }

        public List<int> ListOfFilmIds(List<FilmVM> films)
        {
            throw new NotImplementedException();
        }

        public bool ResetPassWord(UserVM customer)
        {
            throw new NotImplementedException();
        }

        public bool ResetPassWord(PassWordChangeVM passwordChangeVM)
        {
            if (passwordChangeVM.Id == 666)
            {
                throw new DatabaseErrorException("Fail");
            }
            if (passwordChangeVM.Id == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ToggleActivate(int id)
        {
            if (id != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsAdmin(int id)
        {
            if (nullVal == false || id == 666)
            {
                throw new DatabaseErrorException("fail");
            }
            return id == 1;
        }

        public int GetUserCount()
        {
            int userCount = 10;

            return userCount;
        }
    }
}