using Model = StoreModels;
using Entity = StoreDL.Entities;
using System.Linq;
using System.Collections.Generic;
namespace StoreDL.Mappers
{
    public class OrderMapper
    {
        public Model.Order ParseOrder(Entity.OrderTable Order){
            return new Model.Order{
                Customer = new CustomerMapper().ParseCustomer(Order.Customer),
                Total = (double) Order.Total,
                Quantity = (int) Order.Quantity,
                Location = new LocationMapper().ParseLocation(Order.Location),
                orderItems = new ItemMapper().ParseItem(Order.Item)
            };
        }
        public Entity.OrderTable ParseOrder(Model.Order Order){
            return new Entity.OrderTable{
                Customer = new CustomerMapper().ParseCustomer(Order.Customer),
                Total = Order.Total,
                Quantity = Order.Quantity,
                Location = new LocationMapper().ParseLocation(Order.Location),
                Item = new ItemMapper().ParseItem(Order.orderItems)
            };
        }

    }
}