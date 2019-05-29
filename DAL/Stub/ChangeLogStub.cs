using Model.ViewModels;
using System.Collections.Generic;

namespace DAL.Stub
{
    public class ChangeLogStub : Interfaces.IChangeLogRepository
    {
        public List<ChangeLogVM> AllChanges()
        {
            return new List<ChangeLogVM>()
            {
                new ChangeLogVM()
                {
                   DateChanged = "20.10.2018 01:01:02",
                   EntityName = "EntityName1",
                   NewValue ="NewVal1",
                   OldValue ="OldVal",
                   PrimaryKeyValue = "PrimaryKeyVal1",
                   PropertyName = "PropertyName1"
                }, new ChangeLogVM()
                {
                   DateChanged = "21.10.2018 01:01:05",
                   EntityName = "EntityName1",
                   NewValue ="NewVal1",
                   OldValue ="OldVal",
                   PrimaryKeyValue = "PrimaryKeyVal1",
                   PropertyName = "PropertyName1"
                }, new ChangeLogVM()
                {
                   DateChanged = "22.10.2018 01:01:10",
                   EntityName = "EntityName1",
                   NewValue ="NewVal1",
                   OldValue ="OldVal",
                   PrimaryKeyValue = "PrimaryKeyVal1",
                   PropertyName = "PropertyName1"
                }
            };
        }

        public int GetChangeCount()
        {
            return 40;
        }
    }
}