using Model.ViewModels;
using System.Collections.Generic;

namespace DAL.Interfaces
{
    public interface IOrderRepository
    {
        bool CreateOrder(UserVM customerVM, List<int> selectedFilmIdList);

        List<OrderVM> AllOfUserOrders(string email);

        List<ShoppingCartVM> CreateShoppingCart(List<int> selectedFilmIdList);

        List<ExpandedOrderVM> AllOrders();

        bool RefundFilm(int ordernr, string title);

        bool RefundOrder(int ordernr);

        int GetOrderCount();
    }
}