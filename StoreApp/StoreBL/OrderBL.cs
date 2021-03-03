using StoreDL.Entities;
using StoreModels;
using StoreDL.Mappers;
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
        private IUserBL userBL;
        public OrderBL(OrderRepo newOrderRepo, IUserBL newUserBL){
            orderRepo = newOrderRepo;
            userBL = newUserBL;
        }
        public List<StoreModels.Order> GetCustomerOrders(int? customerId){
            return orderRepo.FindCustomerOrder(customerId);
        }

        public List<StoreModels.Order> GetLocationOrders(int? locationID){
            return orderRepo.FindLocationOrder(locationID);
        }
    }
}