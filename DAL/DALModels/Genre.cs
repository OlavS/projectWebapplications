using System.Collections.Generic;

namespace DAL.DALModels
{
    /// <summary>
    /// The database's genre model.
    /// </summary>
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<Film> Films { get; set; }
    }
}