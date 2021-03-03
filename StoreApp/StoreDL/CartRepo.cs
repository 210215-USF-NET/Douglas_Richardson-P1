
using System;
using Model = StoreModels;
using Entity = StoreDL.Entities;
using Mapper = StoreDL.Mappers;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Serilog;
namespace StoreDL
{
    /// <summary>
    /// The cart repo
    /// </summary>
    public class CartRepo
    {
        private Entity.P0DatabaseContext context;
        private Mapper.CartMapper mapper; 
        public CartRepo(Entity.P0DatabaseContext context, Mapper.CartMapper mapper){
            this.mapper = mapper;
            this.context = context;
        }
        public int? AddNewCart(Model.Order order)
        {
            Entity.Cart newCart = mapper.ParseOrder(order);
            context.Entry(newCart).State = EntityState.Added;
            context.Carts.Add(newCart);
            //Console.WriteLine("new order");
            context.SaveChanges();
            context.ChangeTracker.Clear();
            context.Entry(newCart).State = EntityState.Detached;
            return newCart.Id;
        }

        public int? GetCartId(int? customerID){
            var result = from cart in context.Carts
            join customer in context.Customers on cart.Customer.Id equals customer.Id
            where cart.Customer.Id == customerID select cart.Id;
            return result.FirstOrDefault();
        }

        public void UpdateCart(int? cartID, Model.Order order){
            Model.Order result = GetCartOrder(cartID);
            context.Carts.AsNoTracking();
            if(result.Customer == null && order.Customer != null){
                result = GetCartWithCustomer(cartID,order.Customer.Id);
            }
            //Console.WriteLine("result id "+result.Id);
            if(result != null){
                Entity.Cart thisOrder = new Mapper.CartMapper().ParseOrder(result);
                thisOrder.Id = (int)cartID;
                context.Entry(thisOrder).State = EntityState.Modified;
                //Console.WriteLine("cartID: "+cartID);
                            
                if(order.Customer != null){
                    thisOrder.CustomerId = order.Customer.Id;
                }
                if(order.Location != null){
                    thisOrder.LocationId = order.Location.LocationID;
                }
                if(order.orderItems != null){
                    thisOrder.ItemId = (int)order.orderItems.ItemID;
                }                
                thisOrder.Quantity = order.Quantity;
                thisOrder.Total = order.Total;
                
                context.SaveChanges();
                context.Entry(thisOrder).State = EntityState.Detached;
                context.ChangeTracker.Clear();
                
            }
        }

        public Model.Order FindCustomerCartOrder(int? customerID){
            Model.Order foundOrder = context.Carts.Include("Customer").Select(x => mapper.ParseOrder(x)).ToList().FirstOrDefault(x => x.Customer.Id == customerID);
            return foundOrder;
        }

        public Model.Order GetCartWithCustomer(int? cartID, int? customerID){
            var result = from cart in context.Carts
            join item in context.Items on cart.Item.Id equals item.Id 
            join customer in context.Customers on cart.Customer.Id equals customer.Id
            join location in context.LocationTables on cart.Location.Id equals location.Id
            join product in context.Products on cart.Item.Product.Id equals product.Id
            where cart.Id == cartID && cart.CustomerId == customerID select new{cart.Id,cart.Customer,cart.Location,cart.Item,product,cart.Quantity,cart.Total};

            Model.Order thisOrder = new Model.Order();
            foreach(var getOrder in result){
                if(getOrder.Id == cartID){
                    thisOrder.Customer = new CustomerMapper().ParseCustomer(getOrder.Customer);
                    thisOrder.Location = new Mapper.LocationMapper().ParseLocation(getOrder.Location);
                    thisOrder.orderItems = new Mapper.ItemMapper().ParseItem(getOrder.Item);
                    thisOrder.orderItems.Product = new Mapper.ProductMapper().ParseProduct(getOrder.product);
                    thisOrder.Quantity = (int)getOrder.Quantity;
                    thisOrder.Total = (double)getOrder.Total;
                    thisOrder.Id = getOrder.Id;
                }
            }
            return thisOrder;
        }

        public Model.Order GetCartOrder(int? cartID)
        {
            //Entity.Cart thisOrder = context.Carts.Find(cartID);context.Carts.Select(x => mapper.ParseOrder(x)).ToList().FirstOrDefault(x => x.Id == cartID);
            var result = from cart in context.Carts
            join item in context.Items on cart.Item.Id equals item.Id 
            join customer in context.Customers on cart.Customer.Id equals customer.Id
            join location in context.LocationTables on cart.Location.Id equals location.Id
            join product in context.Products on cart.Item.Product.Id equals product.Id
            where cart.Id == cartID select new{cart.Id,cart.Customer,cart.Location,cart.Item,product,cart.Quantity,cart.Total};

            Model.Order thisOrder = new Model.Order();
            foreach(var getOrder in result){
                if(getOrder.Id == cartID){
                    thisOrder.Customer = new CustomerMapper().ParseCustomer(getOrder.Customer);
                    thisOrder.Location = new Mapper.LocationMapper().ParseLocation(getOrder.Location);
                    thisOrder.orderItems = new Mapper.ItemMapper().ParseItem(getOrder.Item);
                    thisOrder.orderItems.Product = new Mapper.ProductMapper().ParseProduct(getOrder.product);
                    thisOrder.Quantity = (int)getOrder.Quantity;
                    thisOrder.Total = (double)getOrder.Total;
                    thisOrder.Id = getOrder.Id;
                }
            }
            return thisOrder;
        }
        public Model.Order GetCartOrderWithNoCustomer(int? cartID)
        {
            //Entity.Cart thisOrder = context.Carts.Find(cartID);context.Carts.Select(x => mapper.ParseOrder(x)).ToList().FirstOrDefault(x => x.Id == cartID);
            var result = from cart in context.Carts
            join item in context.Items on cart.Item.Id equals item.Id 
            join location in context.LocationTables on cart.Location.Id equals location.Id
            join product in context.Products on cart.Item.Product.Id equals product.Id
            where cart.Id == cartID select new{cart.Id,cart.Location,cart.Item,product,cart.Quantity,cart.Total,cart.CustomerId};
            Model.Order thisOrder = new Model.Order();
            foreach(var getOrder in result){
                if(getOrder.Id == cartID){
                    thisOrder.Location = new Mapper.LocationMapper().ParseLocation(getOrder.Location);
                    thisOrder.orderItems = new Mapper.ItemMapper().ParseItem(getOrder.Item);
                    thisOrder.orderItems.Product = new Mapper.ProductMapper().ParseProduct(getOrder.product);
                    thisOrder.Quantity = (int)getOrder.Quantity;
                    thisOrder.Total = (double)getOrder.Total;
                    thisOrder.Id = getOrder.Id;
                }
            }
            return thisOrder;
        }
        // public void SetPrice(){

        // }

        // public double GetPrice(){
        //     Model.Order cartOrder = GetCartOrder();
        //     return cartOrder.Total;
        // }

        public void EmptyCartNoCustomer(int? cartId){
            Model.Order result = GetCartOrderWithNoCustomer(cartId);
            Entity.Cart convertOrder= new Mapper.CartMapper().ParseOrder(result);
            context.Entry(convertOrder).State = EntityState.Modified;
            convertOrder.LocationId = null;
            convertOrder.ItemId = null;
            convertOrder.Quantity = 0;
            convertOrder.Total = 0.0;
            //context.Carts.Remove(convertOrder);
            try{
                context.SaveChanges();
            }catch(Exception e){
                Log.Error(e.ToString());
            }
            context.Entry(convertOrder).State = EntityState.Detached;
            context.ChangeTracker.Clear();
        }

        public void EmptyCart(int? cartId){
            Model.Order result = GetCartOrder(cartId);
            Entity.Cart convertOrder= new Mapper.CartMapper().ParseOrder(result);
            context.Entry(convertOrder).State = EntityState.Modified;
            convertOrder.LocationId = null;
            convertOrder.ItemId = null;
            convertOrder.Quantity = 0;
            convertOrder.Total = 0.0;
            convertOrder.CustomerId = result.Customer.Id;
            //context.Carts.Remove(convertOrder);
            try{
                context.SaveChanges();
            }catch(Exception e){
                Log.Error(e.ToString());
            }
            context.Entry(convertOrder).State = EntityState.Detached;
            context.ChangeTracker.Clear();
        }
    }//class
}
