using System;

namespace DAL.DALModels
{
    /// <summary>
    /// The database's changelog model.
    ///
    /// Online source:
    /// Matthew Jones. Entity Change Tracking using DbContext in Entity Framework 6. Link:
    /// https://exceptionnotfound.net/entity-change-tracking-using-dbcontext-in-entity-framework-6/ (27.10.2018)
    /// </summary>
    public class ChangeLog
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public string PropertyName { get; set; }
        public string PrimaryKeyValue { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime DateChanged { get; set; }
    }
}