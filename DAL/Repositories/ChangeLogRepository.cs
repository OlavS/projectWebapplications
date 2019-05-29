using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class ChangeLogRepository : Interfaces.IChangeLogRepository
    {
        private TimeZoneInfo tZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

        /// <summary>
        /// Gets all changelog items in the database.
        /// </summary>
        /// <returns>List of changelog items.</returns>
        public List<ChangeLogVM> AllChanges()
        {
            try
            {
                using (var db = new VideoDB())
                {
                    List<ChangeLogVM> changes = new List<ChangeLogVM>();
                    foreach (var change in db.ChangeLogs)
                    {
                        changes.Add(new ChangeLogVM()
                        {
                            EntityName = change.EntityName,
                            PropertyName = change.PropertyName,
                            PrimaryKeyValue = change.PrimaryKeyValue,
                            OldValue = change.OldValue,
                            NewValue = change.NewValue,
                            DateChanged = TimeZoneInfo.ConvertTimeFromUtc(change.DateChanged, tZone).ToString("dd.MM.yyyy HH:mm:ss")
                        });
                    }
                    return changes;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.Repositories.OrdreRepository.AllChanges", null, e);
                return new List<ChangeLogVM>();
            }
        }

        /// <summary>
        /// Get number of changes.
        /// </summary>
        /// <returns></returns>
        public int GetChangeCount()
        {
            try
            {
                using (var db = new VideoDB())
                {
                    return db.ChangeLogs.Count();
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Dal.UserRepository.GetChangeCount()", null, e);
                return 0;
            }
        }
    }
}