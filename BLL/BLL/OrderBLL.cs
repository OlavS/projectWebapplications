using DAL.Interfaces;
using DAL.Repositories;
using Model.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BLL.BLL
{
    /// <summary>
    /// Kontroller for alt knyttet til ordre, altså handlekurv, betaling og ordreopprettelse.
    /// </summary>
    public class OrderBLL : Interfaces.IOrderBLL
    {
        private IOrderRepository _ordreRepository;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public OrderBLL()
        {
            _ordreRepository = new OrderRepository();
        }

        /// <summary>
        /// Allowing stub as parameter.
        /// </summary>
        /// <param name="stub"></param>
        public OrderBLL(IOrderRepository stub)
        {
            _ordreRepository = stub;
        }

        public bool CreateOrder(UserVM kunde, List<int> chosenFilmIdList)
        {
            return _ordreRepository.CreateOrder(kunde, chosenFilmIdList);
        }

        public List<ShoppingCartVM> CreateShoppingCart(List<int> chosenFilmIdList)
        {
            return _ordreRepository.CreateShoppingCart(chosenFilmIdList);
        }

        public List<OrderVM> AlleOfCustomersOrders(string email)
        {
            return _ordreRepository.AllOfUserOrders(email);
        }

        /// <summary>
        /// Converts a list that contains filmIds in the form of a Json serialized object, to
        /// a C# List.
        /// </summary>
        /// <param name="filmIdList">List in form of a Json serialized object</param>
        /// <returns>List of film IDs</returns>
        public List<int> JsonArrayToList(string filmIdList)
        {
            dynamic liste = JsonConvert.DeserializeObject(filmIdList);
            var filmIdListen = new List<int>();
            foreach (string id in liste)
            {
                filmIdListen.Add(Convert.ToInt32(id));
            }

            return filmIdListen.Any() ? filmIdListen : null;
        }

        /// <summary>
        /// Removes a single film from the shopping cart.
        /// </summary>
        /// <param name="chosenFilmIdList">List of film ids.</param>
        /// <param name="id">Film id that is about to be removed.</param>
        /// <returns>Film id list or null if it is empty.</returns>
        public List<int> RemoveFilm(List<int> chosenFilmIdList, int id)
        {
            chosenFilmIdList.Remove(id);
            return chosenFilmIdList.Any() ? chosenFilmIdList : null;
        }

        public List<ExpandedOrderVM> AllOrders()
        {
            return _ordreRepository.AllOrders();
        }

        public bool RefundFilm(int orderNr, string title)
        {
            return _ordreRepository.RefundFilm(orderNr, title);
        }

        public bool RefundOrder(int orderNr)
        {
            return _ordreRepository.RefundOrder(orderNr);
        }

        /// <summary>
        /// Removes order from a list of ExpandedOrderVM.
        /// </summary>
        /// <param name="orderNr">To be removed</param>
        /// <param name="orderList">List to remove from</param>
        /// <returns>List without the order</returns>
        public List<ExpandedOrderVM> RemoveOrderNr(int orderNr, List<ExpandedOrderVM> orderList)
        {
            foreach (var order in orderList)
            {
                if (order.Order.OrderNr == orderNr)
                {
                    orderList.Remove(order);
                    break;
                }
            }
            return orderList;
        }

        /// <summary>
        /// Remove film from order.
        /// </summary>
        /// <param name="orderNr">To be targeted</param>
        /// <param name="title">To remove</param>
        /// <param name="orderList">List to be remove from</param>
        /// <returns>List without the film</returns>
        public List<ExpandedOrderVM> RemoveFilmFromOrder(int orderNr, string title, List<ExpandedOrderVM> orderList)
        {
            foreach (var order in orderList)
            {
                if (order.Order.OrderNr == orderNr)
                {
                    foreach (var orderline in order.Order.OrderLines)
                    {
                        if (orderline.Title == title)
                        {
                            order.Order.TotalPrice -= orderline.Price;
                            order.Order.OrderLines.Remove(orderline);
                            if (order.Order.OrderLines.Count == 0)
                            {
                                orderList.Remove(order);
                            }
                            return orderList;
                        }
                    }
                }
            }
            return orderList;
        }
    }
}