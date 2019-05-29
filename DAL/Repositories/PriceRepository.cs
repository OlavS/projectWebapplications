using DAL.DALModels;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    public class PriceRepository : Interfaces.IPriceRepository
    {
        /// <summary>
        /// Getter for price class VMs
        /// </summary>
        /// <returns>A list of all price class VMs.</returns>
        public List<PriceClassVM> GetPriceClasses()
        {
            try
            {
                using (var db = new VideoDB())
                {
                    List<PriceClass> DBPrices = db.PriceClasses.ToList();
                    List<PriceClassVM> PriceClassList = new List<PriceClassVM>();
                    foreach (var price in DBPrices)
                    {
                        PriceClassList.Add(ConvertPriceClass(price));
                    }

                    return PriceClassList;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.PriceRepository.RefundOrder()", null, e);
                return new List<PriceClassVM>(); // RETURNS EMPTY
            }
        }

        /// <summary>
        /// Saving price changes to the database.
        /// </summary>
        /// <param name="id">Price class id</param>
        /// <param name="newPrice">New price value</param>
        /// <returns>True if successful, else false.</returns>
        public bool ChangePrice(int id, int newPrice)
        {
            try
            {
                using (var db = new VideoDB())
                {
                    PriceClass Price = db.PriceClasses.Find(id);
                    Price.Price = newPrice;
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.PriceRepository.ChangePrice(int id, int newPrice)", id.ToString() + "/r/n" + newPrice.ToString(), e);

                return false;
            }
        }

        #region Helper Methods

        private PriceClassVM ConvertPriceClass(PriceClass price)
        {
            PriceClassVM OutPrice = new PriceClassVM()
            {
                Id = price.Id,
                Price = price.Price
            };

            return OutPrice;
        }

        #endregion Helper Methods
    }
}