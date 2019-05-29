using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.DALModels
{
    /// <summary>
    /// The database's order model.
    /// </summary>
    public class Order
    {
        [Key]
        public int OrderNr { get; set; }

        public DateTime Date { get; set; }
        public bool Active { get; set; } = true;

        public virtual User User { get; set; }
        public virtual List<OrderLine> OrderLines { get; set; }
    }
}