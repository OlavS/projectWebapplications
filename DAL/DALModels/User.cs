using System;
using System.Linq;

namespace DAL.DALModels
{
    /// <summary>
    /// The database's user model.
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Address { get; set; }
        public string PhoneNr { get; set; }
        public string Email { get; set; }
        public byte[] Salt { get; set; }
        public byte[] PassWord { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool Admin { get; set; } = false;

        public virtual PostalAddress PostalAddress { get; set; }
        public virtual IQueryable<Order> Order { get; set; }
        public bool Active { get; set; } = true;

        override
        public string ToString()
        {
            return
                "KundeVM:" + "\r\n" +
                "Id: " + Id + "\r\n" +
                "Fornavn: " + FirstName + "\r\n" +
                "Etternavn: " + SurName + "\r\n" +
                "Epost: " + Email + "\r\n" +
                "Adresse: " + Address + "\r\n" +
                "Admin: " + Admin + "\r\n" +
                "Aktiv: " + Address;
        }
    }
}