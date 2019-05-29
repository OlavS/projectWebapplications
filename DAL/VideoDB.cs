using DAL.DALModels;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace DAL
{
    public class VideoDB : DbContext
    {
        public VideoDB() : base("name=VideoDB")
        {
            Database.SetInitializer(new DBinit());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public virtual DbSet<PostalAddress> PostalAddresses { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Film> Films { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderLine> OrderLines { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<PriceClass> PriceClasses { get; set; }
        public virtual DbSet<Error> ErrorLogs { get; set; }
        public virtual DbSet<ChangeLog> ChangeLogs { get; set; }

        /// <summary>
        /// Changelog implementation that tracks changes in as modifications and new entries.
        /// We never delete anything from the DB, so this is not nesessary to track.
        /// We modified the online source to track new entries aswell.
        ///
        /// Online source:
        /// Matthew Jones. Entity Change Tracking using DbContext in Entity Framework 6. Link:
        /// https://exceptionnotfound.net/entity-change-tracking-using-dbcontext-in-entity-framework-6/ (27.10.2018)
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            var modifiedEntities = ChangeTracker.Entries().Where(p => p.State == EntityState.Modified).ToList();
            var now = DateTime.Now;
            foreach (var change in modifiedEntities)
            {
                var entityName = change.Entity.GetType().Name;

                int underScoreIndex = entityName.IndexOf("_");
                if (underScoreIndex > 0) entityName = entityName.Substring(0, underScoreIndex);

                var primaryKey = GetPrimaryKeyValue(change);

                foreach (var prop in change.OriginalValues.PropertyNames)
                {
                    var originalValue = change.OriginalValues[prop].ToString();
                    var currentValue = change.CurrentValues[prop].ToString();
                    if (originalValue != currentValue)
                    {
                        ChangeLog log = new ChangeLog()
                        {
                            EntityName = entityName,
                            PrimaryKeyValue = primaryKey.ToString(),
                            PropertyName = prop,
                            OldValue = originalValue,
                            NewValue = currentValue,
                            DateChanged = now
                        };
                        ChangeLogs.Add(log);
                    }
                }
            }

            var newRows = ChangeTracker.Entries().Where(c => c.State == EntityState.Added).ToList();

            foreach (var change in newRows)
            {
                var entityName = change.Entity.GetType().Name;
                if (entityName == "ChangeLog" || entityName == "Error")
                {
                    break;
                };
                var prop = change.CurrentValues.PropertyNames.ToList();

                ChangeLog log = new ChangeLog()
                {
                    EntityName = entityName,
                    PropertyName = prop[0],
                    NewValue = "ADDED",
                    DateChanged = now
                };
                base.SaveChanges();

                //To get the right PK value we pick it out after the base call to savechanges.
                var primary = prop[0];
                var primaryVal = change.CurrentValues[primary].ToString();

                if (primaryVal == "0")
                    primaryVal = "1";

                log.PrimaryKeyValue = primaryVal;
                ChangeLogs.Add(log);
            }

            return base.SaveChanges();
        }

        /// <summary>
        /// Retrieving the primary key from the entity.
        ///
        /// Online source:
        /// Matthew Jones. Entity Change Tracking using DbContext in Entity Framework 6. Link:
        /// https://exceptionnotfound.net/entity-change-tracking-using-dbcontext-in-entity-framework-6/ (27.10.2018)
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private object GetPrimaryKeyValue(DbEntityEntry entry)
        {
            var objectStateEntry =
                ((IObjectContextAdapter)this).ObjectContext.ObjectStateManager.GetObjectStateEntry(entry.Entity);
            return objectStateEntry.EntityKey.EntityKeyValues[0].Value;
        }
    }
}