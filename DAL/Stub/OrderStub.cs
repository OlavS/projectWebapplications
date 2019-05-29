using Model.ViewModels;
using System;
using System.Collections.Generic;

namespace DAL.Stub
{
    public class OrderStub : Interfaces.IOrderRepository
    {
        public List<ExpandedOrderVM> AllOrders()
        {
            var expectedList = new List<ExpandedOrderVM>();
            var expandedOrder = new ExpandedOrderVM()
            {
                FirstName = "Ola",
                SurName = "Nordmann",
                Email = "ola@nordmann.no",
            };
            var expandedOrder2 = new ExpandedOrderVM()
            {
                FirstName = "Tore",
                SurName = "Sagen",
                Email = "tore@sagen.no",
            };
            OrderVM order = new OrderVM()
            {
                OrderNr = 0,
                Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10, 40, 00).ToString()
            };

            OrderVM order2 = new OrderVM()
            {
                OrderNr = 1,
                Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 10, 20, 00).ToString()
            };
            OrdreLinjeVM orderline = new OrdreLinjeVM()
            {
                Price = 49,
                Title = "Mad Max"
            };
            OrdreLinjeVM orderline2 = new OrdreLinjeVM()
            {
                Price = 99,
                Title = "Tomb Raider"
            };
            OrdreLinjeVM orderline3 = new OrdreLinjeVM()
            {
                Price = 129,
                Title = "Kontiki"
            };
            List<OrdreLinjeVM> orderlines = new List<OrdreLinjeVM>();
            List<OrdreLinjeVM> orderlines2 = new List<OrdreLinjeVM>();

            orderlines.Add(orderline);
            orderlines.Add(orderline3);

            orderlines2.Add(orderline);
            orderlines2.Add(orderline2);
            orderlines2.Add(orderline3);

            expandedOrder.Order = order;
            expandedOrder.Order.OrderLines = orderlines;
            expandedOrder.Order.TotalPrice = 49 + 129;

            expandedOrder2.Order = order2;
            expandedOrder2.Order.OrderLines = orderlines2;
            expandedOrder2.Order.TotalPrice = 49 + 99 + 129;

            expectedList.Add(expandedOrder);
            expectedList.Add(expandedOrder2);

            return expectedList;
        }

        public bool RefundFilm(int ordernr, string title)
        {
            if (ordernr == 0 && title == "Mad Max")
            {
                return true;
            }

            return false;
        }

        public bool RefundOrder(int ordernr)
        {
            if (ordernr == 0 || ordernr == 1)
            {
                return true;
            }

            return false;
        }

        public int GetOrderCount()
        {
            return 30;
        }

        public bool CreateOrder(UserVM customerVM, List<int> selectedFilmIdList)
        {
            throw new NotImplementedException();
        }

        public List<OrderVM> AllOfUserOrders(string email)
        {
            throw new NotImplementedException();
        }

        public List<ShoppingCartVM> CreateShoppingCart(List<int> selectedFilmIdList)
        {
            throw new NotImplementedException();
        }
    }
}