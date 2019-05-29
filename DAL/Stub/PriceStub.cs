using DAL.Interfaces;
using Model.ViewModels;
using System.Collections.Generic;

namespace DAL.Stub
{
    public class PriceStub : IPriceRepository
    {
        public List<PriceClassVM> GetPriceClasses()
        {
            return new List<PriceClassVM>()
            {
                new PriceClassVM(){
                Id = 1,
                Price = 50
                },
                 new PriceClassVM(){
                Id = 2,
                Price = 100
                },
                new PriceClassVM(){
                Id = 3,
                Price = 150
                }
            };
        }

        public bool ChangePrice(int id, int newPrice)
        {
            if (id == 0)
            {
                return false;
            }

            return true;
        }
    }
}