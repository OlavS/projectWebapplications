using System.Collections.Generic;
using Oblig1.DAL.DALModels;
using Oblig1.Model;

namespace DAL.Interfaces
{
    public interface ICustomerRepository
    {
        string CreateCustomer(CustomerVM inCustomer);
        bool EditCustomer(int id, CustomerVM inCustomer);
        bool FinnEpost(string email);
        Customer GetCustomer(string email);
        byte[] LagSalt();
        byte[] CreateHashedPassword(string passWord, byte[] salt);
        List<int> ListOfFilmIds(List<FilmVM> films);
        List<CustomerVM> AllCustomers(bool admin = false);
        CustomerVM GetCustomerVM(int customerId);
    }
}