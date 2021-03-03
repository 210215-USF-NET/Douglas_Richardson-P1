using Model = StoreModels;
using Entity = StoreDL.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
namespace StoreDL.Mappers
{
    public class CartMapper
    {
        public Model.Order ParseOrder(Entity.Cart Order){            
            if(Order.Customer != null && Order.Location != null && Order.Item != null){
                return new Model.Order{
                    Customer = new CustomerMapper().ParseCustomer(Order.Customer),
                    Total = (double) Order.Total,
                    Quantity = (int) Order.Quantity,
                    Location = new LocationMapper().ParseLocation(Order.Location),
                    orderItems = new ItemMapper().ParseItem(Order.Item),
                    Id = Order.Id
                };
            }else{
                return new Model.Order{
                    Customer = null,
                    Total = (double) Order.Total,
                    Quantity = (int) Order.Quantity,
                    Location = new LocationMapper().ParseLocation(Order.Location),
                    orderItems = new ItemMapper().ParseItem(Order.Item),
                    Id = Order.Id
                };
            }            
        }
        public Entity.Cart ParseOrder(Model.Order Order){
            
            if(Order.Id == null){
                return new Entity.Cart{
                    Customer = null,
                    Total = Order.Total,
                    Quantity = Order.Quantity,
                    Location = null,
                    Item = null
                };
            }
            if(Order.Customer != null){
                return new Entity.Cart{
                    Customer = new CustomerMapper().ParseCustomer(Order.Customer),
                    Total = Order.Total,
                    Quantity = Order.Quantity,
                    Location = new LocationMapper().ParseLocation(Order.Location),
                    Item = new ItemMapper().ParseItem(Order.orderItems),
                    Id = (int)Order.Id
                };
            }else{
                return new Entity.Cart{
                    Customer = new Entity.Customer(),
                    Total = Order.Total,
                    Quantity = Order.Quantity,
                    Location = new LocationMapper().ParseLocation(Order.Location),
                    Item = new ItemMapper().ParseItem(Order.orderItems),
                    Id = (int)Order.Id
                };
            }
            
        }

    }
}