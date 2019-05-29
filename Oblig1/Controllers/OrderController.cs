using BLL.BLL;
using BLL.Interfaces;
using Model.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Oblig1.Controllers
{
    /// <summary>
    /// Controller for everything associated with orders.
    /// </summary>
    public class OrderController : Controller
    {
        private IOrderBLL _ordreBLL;
        private IUserBLL _uesrBLL;
        private IFilmBLL _filmBLL;

        public OrderController()
        {
            _ordreBLL = new OrderBLL();
            _uesrBLL = new UserBLL();
            _filmBLL = new FilmBLL();
        }

        public OrderController(IOrderBLL orderStub, IUserBLL userStub, IFilmBLL filmStub)
        {
            _ordreBLL = orderStub;
            _uesrBLL = userStub;
            _filmBLL = filmStub;
        }

        /// <summary>
        /// Partial view based on a film id list, retrieved from the Session variable ShoppingCart.
        /// </summary>
        /// <seealso cref="HandlekurvHandler.LagHandlekurv"/>
        /// <returns>A table with film information, with the possibility to remove films.</returns>
        [ChildActionOnly]
        public ActionResult ShoppingCartTable()
        {
            var chosenFilmIdList = (List<int>)Session["ShoppingCart"];
            List<ShoppingCartVM> shoppingCart = _ordreBLL.CreateShoppingCart(chosenFilmIdList);
            return PartialView(shoppingCart);
        }

        /// <summary>
        /// A shopping cart view that contains a partial view ShoppingCartTable. If the user
        /// is'nt signed in or have'nt put anything in the shopping cart, will a message be
        /// displayed to the user instead.
        /// </summary>
        /// <seealso cref="ShoppingCartTable"/>
        /// <seealso cref="OrderBLL.CreateShoppingCart"/>
        /// <returns>View ShoppingCart.cshtml</returns>
        public ActionResult ShoppingCart()
        {
            return View(Session["ShoppingCart"] != null);
        }

        /// <summary>
        /// Grants the possibility to remove films from the shoppingcart.
        /// </summary>
        /// <param name="id">The film id to be removed.</param>
        /// <seealso cref="OrderBLL.RemoveFilm"/>
        /// <returns>Viewet ShoppingCart.cshtml</returns>
        public ActionResult RemoveFilmFromShoppingCart(int id)
        {
            var chosenFilmIdList = (List<int>)Session["ShoppingCart"];
            Session["ShoppingCart"] = _ordreBLL.RemoveFilm(chosenFilmIdList, id);
            return RedirectToAction("ShoppingCart");
        }

        /// <summary>
        /// A payInfo form page, where the user type in his payInfo information.
        /// The page also contains the shopping cart table, where the user can se the
        /// total price of the order, and remove films.
        /// </summary>
        /// <returns>Payment form page.</returns>
        public ActionResult Payment()
        {
            return View();
        }

        /// <summary>
        /// Validerer betalingsinformasjonen.
        /// Validating payInfo information.
        /// </summary>
        /// <param name="payInfo">Payment information.</param>
        /// <returns>
        /// If validation success redirecting to CreateOrder, if not it returns the same view.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment(PayVM payInfo)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("CreateOrder");
            }

            return View();
        }

        /// <summary>
        /// Creates customers orders.
        /// </summary>
        /// <returns>Redirects to ShowMessage, a message for order successfully created is
        /// displayed, alternatively a error message.</returns>
        public ActionResult CreateOrder()
        {
            var chosenFilmIdList = (List<int>)Session["ShoppingCart"];
            var customer = (UserVM)Session["Kunde"];
            var registered = _ordreBLL.CreateOrder(customer, chosenFilmIdList);
            if (registered)
            {
                Session["ShoppingCart"] = null;
                return RedirectToAction("ShowMessage", "Home", new { header = ("Takk for at du bestilte hos oss, " + customer.FirstName + "!"), message = "" });
            }
            else
            {
                return RedirectToAction("ShowMessage", "Home", new { header = "Noe gikk galt under opprettelsen av ordren!", message = "" });
            }
        }

        /// <summary>
        /// Creates a session containing the film ids representing the shopping cart.
        /// </summary>
        /// <param name="filmIdList">Json serialized string containing film ids</param>
        public void CreateShoppingCart(string filmIdList)
        {
            List<int> chosenFilmIdList = _ordreBLL.JsonArrayToList(filmIdList);
            Session["ShoppingCart"] = chosenFilmIdList;
        }

        /// <summary>
        /// Getter for the film ids representing the shopping cart
        /// </summary>
        /// <returns>Json serialized string containing film Ids.</returns>
        public string GetShoppingCart()
        {
            var chosenFilmIdList = (List<int>)Session["ShoppingCart"];

            return (JsonConvert.SerializeObject(chosenFilmIdList));
        }

        /// <summary>
        /// Getter for the customers owned films.
        /// </summary>
        /// <returns>Json serialized string of the customers owned films as film ids.</returns>
        public string GetOwnedFilms()
        {
            var customer = (UserVM)Session["Kunde"];
            if (customer != null)
            {
                List<FilmVM> filmList = _filmBLL.AllOfUsersFilms(customer.Email);
                List<int> idList = _uesrBLL.ListOfFilmIds(filmList);
                return (JsonConvert.SerializeObject(idList));
            }

            return JsonConvert.SerializeObject(null);
        }

        /// <summary>
        /// A view with the loged on users orders.
        /// </summary>
        /// <returns>View with the a spesific customers orders if a user is signed in, else back to the frontpage.</returns>
        public ActionResult CustomersOrders()
        {
            var customer = (UserVM)Session["Kunde"];
            if (customer != null)
            {
                List<OrderVM> list = _ordreBLL.AlleOfCustomersOrders(customer.Email);
                return View(list);
            }
            return RedirectToAction("Frontpage", "Film");
        }
    }
}