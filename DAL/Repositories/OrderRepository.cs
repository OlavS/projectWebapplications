using DAL.DALModels;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories
{
    /// <summary>
    /// Handles the web applications order features.
    /// </summary>
    public class OrderRepository : Interfaces.IOrderRepository
    {
        //Finner norsk tidsone
        private readonly TimeZoneInfo tZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");

        /// <summary>
        /// Creates an order, based on a film ID list and a user.
        /// </summary>
        /// <param name="customerVM">The customer to create the order for.</param>
        /// <param name="selectedFilmIdList">The selected film id list to base the order on.</param>
        /// <returns>True if the order was created successfully, else false.</returns>
        public bool CreateOrder(UserVM customerVM, List<int> selectedFilmIdList)
        {
            try
            {
                using (var db = new VideoDB())
                {
                    User customer = db.Users.FirstOrDefault(k => k.Email == customerVM.Email);

                    Order order = new Order()
                    {
                        User = customer,
                        Date = DateTime.Now
                    };

                    List<OrderLine> orderLine = new List<OrderLine>();
                    for (int i = 0; i < selectedFilmIdList.Count; i++)
                    {
                        Film film = db.Films.Find(selectedFilmIdList[i]);

                        orderLine.Add(new OrderLine()
                        {
                            Order = order,
                            Film = film,
                            Price = db.PriceClasses.Find(film.PriceClassId).Price
                        });
                    }
                    db.OrderLines.AddRange(orderLine);
                    db.Orders.Add(order);
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.OrderRepository.CreateOrder(CustomerVM customerVM, List<int> selectedFilmIdList)", customerVM.ToString() + "\r\n" + selectedFilmIdList.ToString(), e);

                return false;
            }
        }

        /// <summary>
        /// Gets all of customers orders.
        /// </summary>
        /// <param name="email">To search for in the database.</param>
        /// <returns>A list of orderVMs.</returns>
        public List<OrderVM> AllOfUserOrders(string email)
        {
            try
            {
                var orderVMlist = new List<OrderVM>();
                User customer;
                using (var db = new VideoDB())
                {
                    customer = db.Users.FirstOrDefault(k => k.Email == email);
                    List<Order> orders = db.Orders.Where(o => o.User.Id == customer.Id).ToList();
                    foreach (var ordre in orders)
                    {
                        var OrderVM = new OrderVM()
                        {
                            OrderNr = ordre.OrderNr,

                            //Setting the time to "Romance Standard Time after retrieval from database.
                            //Our server is running with UTC time zone.
                            Date = TimeZoneInfo.ConvertTimeFromUtc(ordre.Date, tZone).ToString("dd.MM.yyyy HH:mm:ss"),

                            OrderLines = ordre.OrderLines.Select(o => new OrdreLinjeVM()
                            {
                                Title = o.Film.Title,
                                Price = o.Price
                            }).ToList()
                        };

                        int totalPrice = 0;
                        foreach (var li in OrderVM.OrderLines)
                        {
                            totalPrice += li.Price;
                        };
                        OrderVM.TotalPrice = totalPrice;

                        orderVMlist.Add(OrderVM);
                    }
                }
                return orderVMlist;
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.OrderRepository.AllOfCustomersOrders(string email))", email, e);
                return new List<OrderVM>();
            }
        }

        /// <summary>
        /// Creates a list of ShoppingCartVMs, that represents the content in the shopping cart.
        /// </summary>
        /// <param name="selectedFilmIdList">A list of film Ids, that represents the shopping cart stored in session.</param>
        /// <returns>A list of shoppingCartVMs.</returns>
        public List<ShoppingCartVM> CreateShoppingCart(List<int> selectedFilmIdList)
        {
            try
            {
                if (selectedFilmIdList != null)
                {
                    var cartTableVM = new List<ShoppingCartVM>();
                    using (var db = new VideoDB())
                    {
                        var sortedIdList = selectedFilmIdList.OrderBy(P => P).ToList();
                        List<Film> films = db.Films.ToList();
                        List<Film> filmList = new List<Film>();
                        int index = 0;
                        foreach (var film in films)
                        {
                            if (film.Id == sortedIdList[index])
                            {
                                filmList.Add(film);
                                index++;
                                if (index == sortedIdList.Count)
                                {
                                    break;
                                }
                            }
                        }

                        foreach (var film in filmList)
                        {
                            cartTableVM.Add(new ShoppingCartVM()
                            {
                                Id = film.Id,
                                Title = film.Title,
                                ImgURL = film.ImgURL,
                                Price = film.PriceClasses.Price
                            });
                        }
                    }
                    return cartTableVM;
                }

                return new List<ShoppingCartVM>();
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.OrderRepository.CreateShoppingCart(List<int> selectedFilmIdList)", selectedFilmIdList.ToString(), e);

                //Returns new empty cart.
                return new List<ShoppingCartVM>();
            }
        }

        /// <summary>
        /// Gets all orders stored in the database.
        /// </summary>
        /// <returns>List of ExpandedOrderVMs.</returns>
        public List<ExpandedOrderVM> AllOrders()
        {
            try
            {
                TimeZoneInfo tZone = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                using (var db = new VideoDB())
                {
                    List<ExpandedOrderVM> allOrders = new List<ExpandedOrderVM>();
                    foreach (var order in db.Orders)
                    {
                        if (order.Active)
                        {
                            var tempOrder = new ExpandedOrderVM()
                            {
                                FirstName = order.User.FirstName,
                                SurName = order.User.SurName,
                                Email = order.User.Email,
                                Order = new OrderVM()
                                {
                                    OrderNr = order.OrderNr,
                                    Date = TimeZoneInfo.ConvertTimeFromUtc(order.Date, tZone)
                                        .ToString("dd.MM.yyyy HH:mm:ss")
                                }
                            };
                            List<OrdreLinjeVM> list = new List<OrdreLinjeVM>();
                            foreach (var line in order.OrderLines)
                            {
                                if (line.Active)
                                {
                                    var orderLineVM = new OrdreLinjeVM
                                    {
                                        Title = line.Film.Title,
                                        Price = line.Price
                                    };
                                    list.Add(orderLineVM);
                                }
                            }

                            tempOrder.Order.OrderLines = list;

                            int totalPrice = 0;
                            foreach (var item in order.OrderLines)
                            {
                                totalPrice += item.Price;
                            }

                            tempOrder.Order.TotalPrice = totalPrice;

                            allOrders.Add(tempOrder);
                        }
                    }

                    return allOrders;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Oblig1.DAL.OrderRepository.AllOrders()", null, e);

                //Returns empty list
                return new List<ExpandedOrderVM>();
            }
        }

        /// <summary>
        /// Refunds a film from a specific order, by setting it inactive.
        /// </summary>
        /// <param name="ordernr">The order nr. to remove a film from.</param>
        /// <param name="title">The title to remove</param>
        /// <returns>True if succedded, else false.</returns>
        public bool RefundFilm(int ordernr, string title)
        {
            try
            {
                using (var db = new VideoDB())
                {
                    Order order = db.Orders.Find(ordernr);
                    OrderLine orderLine = order.OrderLines.FirstOrDefault(f => f.Film.Title == title);
                    orderLine.Active = false;
                    var counter = 0;
                    foreach (var orderline in order.OrderLines)
                    {
                        if (orderline.Active)
                        {
                            counter++;
                        }
                    }

                    if (counter == 0)
                    {
                        order.Active = false;
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError(
                    "Oblig1.DAL.OrderRepository.RefundFilm(int ordernr, string title)",
                    "Ordernr: " + ordernr + ", title: " + title,
                    e
                );
                return false;
            }
        }

        /// <summary>
        /// Refunds a order, by setting it inactive in the database.
        /// </summary>
        /// <param name="ordernr">Order nr to refund.</param>
        /// <returns>True if succedded, else false.</returns>
        public bool RefundOrder(int ordernr)
        {
            try
            {
                using (var db = new VideoDB())
                {
                    Order order = db.Orders.Find(ordernr);

                    List<OrderLine> orderLines = db.OrderLines.Where(o => o.Order.OrderNr == ordernr).ToList();
                    foreach (var line in orderLines)
                    {
                        line.Active = false;
                    }

                    order.Active = false;

                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError(
                    "Oblig1.DAL.OrderRepository.RefundOrder(int ordernr)",
                    "Ordernr: " + ordernr,
                    e
                );

                return false;
            }
        }

        public int GetOrderCount()
        {
            try
            {
                using (var db = new VideoDB())
                {
                    return db.Orders.Count();
                }
            }
            catch (Exception e)
            {
                new ErrorLogRepository().CreateError("Dal.UserRepository.GetOrderCount()", null, e);
                return 0;
            }
        }
    }
}