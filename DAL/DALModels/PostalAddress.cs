using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.DALModels
{
    /// <summary>
    /// The database's porstal address model.
    /// </summary>
    public class PostalAddress
    {
        [Key]
        public string PostalCode { get; set; }

        public string Postal { get; set; }

        public virtual IEnumerable<User> Users { get; set; }
    }
}