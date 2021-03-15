
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
        public OrderRepo(CustomerDBContext context){
            this.context = context;
        }
        public int PushOrder(Model.Order order)
        {
            context.Entry(order).State = EntityState.Added;
            context.Orders.Add(order);
            context.SaveChanges();
            context.ChangeTracker.Clear();
            context.Entry(order).State = EntityState.Detached;

            Log.Information("New order was created. "+ order.Id);
            return order.Id;
        }

        public Tuple<List<Model.OrderHistoryModel>, List<Model.ItemModel>> GetOrder(string email)
        {
            string customerId = "";
            string firstName = "";
            string lastName = "";
            var findUser = from users in context.Users
                           where users.Email.Equals(email)
                           select new {users.Email,users.Id,users.FirstName,users.LastName};
            if (findUser != null)
            {
                customerId = findUser.FirstOrDefault().Id;
                firstName = findUser.FirstOrDefault().FirstName;
                lastName = findUser.FirstOrDefault().LastName;
                var findOrders = from order in context.Orders
                             where order.Customer.Id == customerId
                             select new {order};

                List<Model.OrderHistoryModel> orderHistoryModels = new List<Model.OrderHistoryModel>();
                foreach (var element in findOrders)
                {
                    Model.OrderHistoryModel newOrderHistoryModel = new Model.OrderHistoryModel
                    {
                        Email = email,
                        FirstName = firstName,
                        LastName = lastName,
                        Total = element.order.Total
                    };
                    orderHistoryModels.Add(newOrderHistoryModel);
                }//End of adding to orderhistory

                var findItems = from item in context.Items
                                 join orderitem in context.OrderItems on item.Id equals orderitem.Item.Id
                                 join order in context.Orders on orderitem.Order.Id equals order.Id
                                 join product in context.Products on item.Product.Id equals product.Id
                                 join location in context.Locations on order.Location.Id equals location.Id
                                 where order.Customer.Id == customerId
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
                var tuple = new Tuple<List<Model.OrderHistoryModel>, List<Model.ItemModel>>(orderHistoryModels, itemModels);
                return tuple;
            }
            else
            {
                return null;
            }
            
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
