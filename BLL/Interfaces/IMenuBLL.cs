using Model.ViewModels;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IMenuBLL
    {
        List<GenreVM> GetDropdownList(int flimCount);

        AdminFrontPageVM GetFrontPageVM();
    }
}