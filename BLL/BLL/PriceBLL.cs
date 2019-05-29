using BLL.Interfaces;
using DAL.Interfaces;
using DAL.Repositories;
using Model.ViewModels;
using System.Collections.Generic;

namespace BLL.BLL
{
    public class PriceBLL : IPriceBLL
    {
        private IPriceRepository _priceRepository;

        public PriceBLL()
        {
            this._priceRepository = new PriceRepository();
        }

        public PriceBLL(IPriceRepository priceStub)
        {
            this._priceRepository = priceStub;
        }

        public List<PriceClassVM> GetPriceClasses()
        {
            List<PriceClassVM> PriceClassList = _priceRepository.GetPriceClasses();

            return PriceClassList;
        }

        /// <summary>
        /// Getter for price class changes list.
        /// </summary>
        /// <returns>Price class changes list</returns>
        public List<PriceClassChangeVM> GetPriceChangeVMs()
        {
            List<PriceClassVM> PriceList = _priceRepository.GetPriceClasses();
            List<PriceClassChangeVM> ChangeList = new List<PriceClassChangeVM>();
            foreach (var price in PriceList)
            {
                ChangeList.Add(new PriceClassChangeVM()
                {
                    Id = price.Id,
                    Price = price.Price
                });
            }

            return ChangeList;
        }

        /// <summary>
        /// Changes the price class prices.
        /// </summary>
        /// <param name="changedList"></param>
        /// <returns></returns>
        public bool ChangePrices(List<PriceClassChangeVM> changedList)
        {
            bool result = true;
            foreach (var item in changedList)
            {
                if (item.NewPrice != 0)
                {
                    if (!ChangePrice(item))
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        #region Helper Methods

        private bool ChangePrice(PriceClassChangeVM priceChanged)
        {
            return _priceRepository.ChangePrice(priceChanged.Id, priceChanged.NewPrice);
        }

        #endregion Helper Methods
    }
}