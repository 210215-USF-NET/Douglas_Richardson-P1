
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

/*        public void UpdateCart(int? cartID, Model.Order order){
            Entity.Cart thisOrder = context.Carts.Find(cartID);
            if(order.Customer.FirstName != null){
                thisOrder.Customer = new CustomerMapper().ParseCustomer(order.Customer);
            }
            if(order.Location.LocationName != null){
                thisOrder.Location = new Mapper.LocationMapper().ParseLocation(order.Location);
            }
            if(order.orderItems.Product != null){
                thisOrder.Item = new Mapper.ItemMapper().ParseItem(order.orderItems);
            }           
            thisOrder.Quantity = order.Quantity;
            thisOrder.Total = order.Total;
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }*/

/*        public List<Model.Order> FindCustomerOrder(int? customerID){
            var result = from order in context.OrderTables
            join item in context.Items on order.Item.Id equals item.Id 
            join location in context.LocationTables on order.Location.Id equals location.Id
            join product in context.Products on order.Item.Product.Id equals product.Id
            where order.CustomerId == customerID select new{order.Id,order.Location,order.Item,product,order.Quantity,order.Total};
            List<Model.Order> thisOrder = new List<Model.Order>();
            foreach(var getOrder in result){
                Model.Order newOrder = new Model.Order();
                newOrder.Location = new Mapper.LocationMapper().ParseLocation(getOrder.Location);
                newOrder.orderItems = new Mapper.ItemMapper().ParseItem(getOrder.Item);
                newOrder.orderItems.Product = new Mapper.ProductMapper().ParseProduct(getOrder.product);
                newOrder.Quantity = (int)getOrder.Quantity;
                newOrder.Total = (double)getOrder.Total;
                newOrder.Id = getOrder.Id;
                thisOrder.Add(newOrder);
            }
            return thisOrder;
        }

        public List<Model.Order> FindLocationOrder(int? locationID){
            var result = from order in context.OrderTables
            join item in context.Items on order.Item.Id equals item.Id 
            join location in context.LocationTables on order.Location.Id equals location.Id
            join customer in context.Customers on order.CustomerId equals customer.Id
            join product in context.Products on order.Item.Product.Id equals product.Id
            where order.LocationId == locationID select new{order.Id,order.Location,order.Item,product,order.Quantity,order.Total,customer};
            List<Model.Order> thisOrder = new List<Model.Order>();
            foreach(var getOrder in result){
                Model.Order newOrder = new Model.Order();
                newOrder.Location = new Mapper.LocationMapper().ParseLocation(getOrder.Location);
                newOrder.orderItems = new Mapper.ItemMapper().ParseItem(getOrder.Item);
                newOrder.orderItems.Product = new Mapper.ProductMapper().ParseProduct(getOrder.product);
                newOrder.Customer = new CustomerMapper().ParseCustomer(getOrder.customer);
                newOrder.Quantity = (int)getOrder.Quantity;
                newOrder.Total = (double)getOrder.Total;
                newOrder.Id = getOrder.Id;
                thisOrder.Add(newOrder);
            }
            return thisOrder;
        }*/
        public List<Model.Order> GetOrder(int? orderID)
        {
            var result = from order in context.Orders where order.Id == orderID select order;
            return result.ToList();
        }

    }//class
}
