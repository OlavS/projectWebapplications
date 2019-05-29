using System;
using System.Collections.Generic;

namespace DAL.DALModels
{
    /// <summary>
    /// The database's film model.
    /// </summary>
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgURL { get; set; }

        public virtual PriceClass PriceClasses { get; set; }
        public virtual List<OrderLine> OrderLines { get; set; }
        public virtual List<Genre> Genres { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool Active { get; set; } = true;

        public int PriceClassId { get; set; }

        public string GenreIds { get; set; }
    }
}