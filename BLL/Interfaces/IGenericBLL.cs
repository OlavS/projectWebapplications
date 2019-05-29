using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IGenericBLL<T>
    {
        List<T> Search(string searchString, List<T> inList);
    }
}