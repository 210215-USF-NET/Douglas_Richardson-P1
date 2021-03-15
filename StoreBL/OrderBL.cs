using StoreModels;
using StoreDL;
using System.Collections.Generic;
using System;

namespace StoreBL
{
    /// <summary>
    /// Allows the ui to get the order histories
    /// </summary>
    public class OrderBL
    {
        private OrderRepo orderRepo;
        public OrderBL(OrderRepo newOrderRepo){
            orderRepo = newOrderRepo;
        }
        public int PushOrder(Order order){
            return orderRepo.PushOrder(order);
        }

        public Tuple<List<OrderHistoryModel>, List<ItemModel>> GetOrder(string email){
            return orderRepo.GetOrder(email);
        }

        public void AddOrderItem(OrderItem orderItem)
        {
            orderRepo.AddOrderItem(orderItem);
        }
    }
}