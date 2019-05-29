using Model.ViewModels;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IChangeLogBLL
    {
        List<ChangeLogVM> AllChanges();
    }
}