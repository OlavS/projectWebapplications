namespace DAL.DALModels
{
    /// <summary>
    /// The database's orderline model.
    /// </summary>
    public class OrderLine
    {
        public int Id { get; set; }
        public bool Active { get; set; } = true;
        public int Price { get; set; }

        public virtual Order Order { get; set; }
        public virtual Film Film { get; set; }
    }
}