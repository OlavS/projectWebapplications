using Model.ViewModels;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IPriceRepository
    {
        List<PriceClassVM> GetPriceClasses();

        bool ChangePrice(int id, int newPrice);
    }
}