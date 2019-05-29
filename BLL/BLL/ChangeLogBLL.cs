using DAL.Interfaces;
using DAL.Repositories;
using Model.ViewModels;
using System.Collections.Generic;

namespace BLL.BLL
{
    public class ChangeLogBLL : Interfaces.IChangeLogBLL
    {
        private IChangeLogRepository _changeLogRepository;

        public ChangeLogBLL()
        {
            _changeLogRepository = new ChangeLogRepository();
        }

        public ChangeLogBLL(IChangeLogRepository changeLogStub)
        {
            _changeLogRepository = changeLogStub;
        }

        public List<ChangeLogVM> AllChanges()
        {
            return _changeLogRepository.AllChanges();
        }
    }
}