using Model.ViewModels;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface IOrderBLL
    {
        List<OrderVM> AlleOfCustomersOrders(string email);

        List<int> RemoveFilm(List<int> chosenFilmIdList, int id);

        List<int> JsonArrayToList(string filmIdList);

        List<ShoppingCartVM> CreateShoppingCart(List<int> chosenFilmIdList);

        bool CreateOrder(UserVM kunde, List<int> chosenFilmIdList);

        List<ExpandedOrderVM> AllOrders();

        bool RefundFilm(int orderNr, string title);

        bool RefundOrder(int orderNr);

        List<ExpandedOrderVM> RemoveFilmFromOrder(int orderNr, string title, List<ExpandedOrderVM> orderList);

        List<ExpandedOrderVM> RemoveOrderNr(int orderNr, List<ExpandedOrderVM> orderList);
    }
}