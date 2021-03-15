
using System;
using Model = StoreModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Serilog;
namespace StoreDL
{
    /// <summary>
    /// Pushes orders to the database, also retrieves the location and customer order histories
    /// </summary>
    public class OrderRepo
    {
        private CustomerDBContext context;
        public OrderRepo(CustomerDBContext context) {
            this.context = context;
        }
        public int PushOrder(Model.Order order)
        {
            context.Entry(order).State = EntityState.Added;
            context.Orders.Add(order);
            context.SaveChanges();
            context.ChangeTracker.Clear();
            context.Entry(order).State = EntityState.Detached;

            Log.Information("New order was created. " + order.Id);
            return order.Id;
        }

        public List<Model.OrderHistoryModel> GetOrder(string email)
        {
            List<Model.OrderHistoryModel> orderHistoryModels = null;
            if (email != null) { 
                string customerId = "";
                string firstName = "";
                string lastName = "";
                var findUser = from users in context.Users
                               where users.Email.Equals(email)
                               select new { users.Email, users.Id, users.FirstName, users.LastName };
                if (findUser != null)
                {
                    customerId = findUser.FirstOrDefault().Id;
                    firstName = findUser.FirstOrDefault().FirstName;
                    lastName = findUser.FirstOrDefault().LastName;
                    var findOrders = from order in context.Orders
                                     join location in context.Locations on order.Location.Id equals location.Id
                                     where order.Customer.Id == customerId
                                     orderby order, order.Total ascending
                                     select new { order, location.LocationName };

                    orderHistoryModels = new List<Model.OrderHistoryModel>();
                    foreach (var element in findOrders)
                    {
                        Model.OrderHistoryModel newOrderHistoryModel = new Model.OrderHistoryModel
                        {
                            LocationName = element.LocationName,
                            FirstName = firstName,
                            LastName = lastName,
                            Total = element.order.Total,
                            Id = element.order.Id,
                            Email = email
                        };
                        orderHistoryModels.Add(newOrderHistoryModel);
                    }//End of adding to orderhistory
                }
                return orderHistoryModels;
            }
            else
            {
                return null;
            }

        }//End of GetOrder()
        
        public List<Model.OrderHistoryModel> GetOrdersByLocation(int locationId)
        {
            List<Model.OrderHistoryModel> orderHistoryModels = new List<Model.OrderHistoryModel>();
            if (locationId != 0) {

                var findOrders = from order in context.Orders
                                 join location in context.Locations on order.Location.Id equals location.Id
                                 join user in context.Users on order.Customer.Id equals user.Id
                                 where order.Location.Id == locationId
                                 orderby location, order.Total ascending
                                 select new { order, location.LocationName, user.Email, user.Id, user.FirstName, user.LastName };

                string customerId = "";
                string firstName = "";
                string lastName = "";
                string email = "";
                    
                foreach (var element in findOrders)
                {
                    /*                    var findUser = from users in context.Users
                                                       where users.Email.Equals(element.Email)
                                                       select new { users.Email, users.Id, users.FirstName, users.LastName };
                                        if (findUser != null)
                                        {

                                        }*/
                    customerId = element.Id;
                    firstName = element.FirstName;
                    lastName = element.LastName;
                    email = element.Email;
                    Model.OrderHistoryModel newOrderHistoryModel = new Model.OrderHistoryModel
                    {
                        LocationName = element.LocationName,
                        FirstName = firstName,
                        LastName = lastName,
                        Total = element.order.Total,
                        Id = element.order.Id,
                        Email = email,
                        LocationId = locationId
                    };
                    orderHistoryModels.Add(newOrderHistoryModel);
                }//End of adding to orderhistory
                return orderHistoryModels;
            }
            else
            {
                return null;
            }

        }

        public List<Model.ItemModel> GetOrderItems(int orderId){
            var findItems = from orderitem in context.OrderItems
                            join item in context.Items on orderitem.Item.Id equals item.Id
                            join order in context.Orders on orderitem.Order.Id equals order.Id
                            join product in context.Products on item.Product.Id equals product.Id
                            join location in context.Locations on order.Location.Id equals location.Id
                            where order.Id == orderId
                            select new { item, location, product, orderitem.Quantity };

            List<Model.ItemModel> itemModels = new List<Model.ItemModel>();
            foreach (var element in findItems)
            {
                Model.ItemModel newItemModel = new Model.ItemModel
                {
                    Quantity = element.Quantity,
                    LocationName = element.location.LocationName,
                    ProductName = element.product.ProductName,
                    Price = element.product.Price,
                    Category = element.product.Category,
                };
                itemModels.Add(newItemModel);
            }//End of adding to orderhistory
            itemModels = itemModels.OrderBy(x => x.Price).ToList();

            return itemModels;
        }

        public void AddOrderItem(Model.OrderItem orderItem)
        {
            context.Entry(orderItem).State = EntityState.Added;
            context.OrderItems.Add(orderItem);
            context.SaveChanges();
            context.ChangeTracker.Clear();
            context.Entry(orderItem).State = EntityState.Detached;
        }
    }//class
}
