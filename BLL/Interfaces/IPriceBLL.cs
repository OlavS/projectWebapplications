using Model.ViewModels;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IPriceBLL
    {
        List<PriceClassVM> GetPriceClasses();

        List<PriceClassChangeVM> GetPriceChangeVMs();

        bool ChangePrices(List<PriceClassChangeVM> changedList);
    }
}