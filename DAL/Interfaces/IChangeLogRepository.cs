using Model.ViewModels;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IChangeLogRepository
    {
        List<ChangeLogVM> AllChanges();

        int GetChangeCount();
    }
}