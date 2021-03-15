
using System;
using Model = StoreModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Serilog;
using System.Diagnostics;
namespace StoreDL
{
    /// <summary>
    /// The cart repo
    /// </summary>
    public class CartRepo
    {
        private CustomerDBContext context;
        private readonly ItemRepo itemRepo;
        public CartRepo(CustomerDBContext context, ItemRepo itemRepo)
        {
            this.context = context;
            this.itemRepo = itemRepo;
        }
        public int AddNewCart(Model.Cart cart)
        {
            Tuple<Model.Cart, int> tuple = GetCartFromItemId(cart.Item.Id, cart.CustomerId);
            if (tuple?.Item1 != null)
            {
                context.Entry(cart).State = EntityState.Modified;
                if (cart.Quantity > tuple?.Item2)
                {
                    cart.Quantity = tuple.Item2;
                }
                context.SaveChanges();
                context.ChangeTracker.Clear();
                context.Entry(cart).State = EntityState.Detached;
            }
            else
            {
                context.Entry(cart).State = EntityState.Added;
                context.Carts.Add(cart);
                context.SaveChanges();
                context.ChangeTracker.Clear();
                context.Entry(cart).State = EntityState.Detached;
            }

            return cart.Id;
        }

        public List<Model.Cart> GetCartFromCustomer(string customerID)
        {
            var result = from cart in context.Carts
                         join product in context.Products on cart.Item.Product.Id equals product.Id
                         join location in context.Locations on cart.Location.Id equals location.Id
                         where cart.CustomerId == customerID
                         select new { cart, cart.Item, location, product };
            foreach (var element in result)
            {
                element.cart.Item = element.Item;
                element.cart.Item.Product = element.product;
                element.cart.Location = element.location;
            }
            return result.Select(x => x.cart).ToList();
        }

        public Tuple<Model.Cart, int> GetCartFromItemId(int itemId, string customerID)
        {
            var result = from cart in context.Carts
                         join item in context.Items on cart.Item.Id equals item.Id
                         join product in context.Products on cart.Item.Product.Id equals product.Id
                         join location in context.Locations on cart.Location.Id equals location.Id
                         where cart.CustomerId == customerID && cart.Item.Id == itemId
                         select new { cart, item, location, product };
            foreach (var element in result)
            {
                element.cart.Item = element.item;
                element.cart.Item.Product = element.product;
                element.cart.Location = element.location;
            }


            var tuple = new Tuple<Model.Cart, int>(result.Select(x => x.cart).FirstOrDefault(), result.Select(y => y.item.Quantity).FirstOrDefault());

            return tuple;
        }

        public void RemoveCart(Model.Cart cart)
        {
            context.Entry(cart).State = EntityState.Deleted;
            context.Carts.Remove(cart);
            context.SaveChanges();
            context.ChangeTracker.Clear();
            context.Entry(cart).State = EntityState.Detached;
        }

        public void UpdateCustomerInCart(string oldCustomerId, string newCustomerId)
        {
            List<Model.Cart> foundList = GetCartFromCustomer(oldCustomerId);
            foreach (var cart in foundList)
            {
                cart.CustomerId = newCustomerId;
                UpdateCart(cart);
            }
        }

        public void UpdateCart(Model.Cart cart)
        {
            context.Entry(cart).State = EntityState.Modified;
            context.SaveChanges();
            context.ChangeTracker.Clear();
            context.Entry(cart).State = EntityState.Detached;
        }
    }
}