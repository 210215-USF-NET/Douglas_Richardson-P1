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

        public List<OrderHistoryModel>GetOrder(string email){
            return orderRepo.GetOrder(email);
        }
        public List<OrderHistoryModel> GetOrdersByLocation(int locationId){
            return orderRepo.GetOrdersByLocation(locationId);
        }

        public List<ItemModel> GetOrderItems(int orderId)
        {
            return orderRepo.GetOrderItems(orderId);
        }
 
        public void AddOrderItem(OrderItem orderItem)
        {
            orderRepo.AddOrderItem(orderItem);
        }
    }
}