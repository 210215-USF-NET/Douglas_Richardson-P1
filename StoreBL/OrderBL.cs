using StoreModels;
using StoreDL;
using System.Collections.Generic;
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

        public List<Order> GetOrder(int? orderId){
            return orderRepo.GetOrder(orderId);
        }
    }
}