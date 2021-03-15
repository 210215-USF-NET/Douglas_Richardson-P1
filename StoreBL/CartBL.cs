using StoreDL;
using StoreModels;
using System;
using Serilog;
using System.Collections.Generic;

namespace StoreBL
{   
    public class CartBL
    {
        /// <summary>
        /// The cart holds items for the customer when they are shopping, This holds the precusor to an order.         
        /// The cart is not saved to the database until there is a customer applied to it
        /// </summary>
        private CartRepo cartRepo;
        private OrderRepo orderRepo;
        public Order cartOrder;
        
        public CartBL(CartRepo newCartRepo, OrderRepo newOrderRepo){
            cartRepo = newCartRepo;
            orderRepo = newOrderRepo;
        }

        public List<Cart> GetCartFromCustomer(string customerId)
        {
            return cartRepo.GetCartFromCustomer(customerId);
        }
        
        public int AddNewCart(Cart cart){
            return cartRepo.AddNewCart(cart);
        }

        public void RemoveCart(Cart cart)
        {
            cartRepo.RemoveCart(cart);
        }
        public void UpdateCustomerInCart(string oldCustomerId, string newCustomerId)
        {
            cartRepo.UpdateCustomerInCart(oldCustomerId, newCustomerId);
        }
    }
}