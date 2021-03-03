using Model = StoreModels;
using Entity = StoreDL.Entities;
using System.Linq;
using System.Collections.Generic;
using System;

namespace StoreDL.Mappers
{
    public class ItemMapper
    {
        public Model.Item ParseItem(Entity.Item Item){
            if(Item.Product != null){
                return new Model.Item{
                    Product = new ProductMapper().ParseProduct(Item.Product),
                    ItemID = Item.Id,
                    ItemLocation = new LocationMapper().ParseLocation(Item.Location),
                    Quantity = (int) Item.Quantity
                };
            }
            return null;
        }
        public Entity.Item ParseItem(Model.Item Item){
            
            if(Item.ItemLocation != null){
                return new Entity.Item{
                    Product = new ProductMapper().ParseProduct(Item.Product),
                    Location = new LocationMapper().ParseLocation(Item.ItemLocation),
                    Quantity = Item.Quantity
                };
            }else{
                return new Entity.Item{
                    Product = new ProductMapper().ParseProduct(Item.Product),
                    Quantity = Item.Quantity
                };
            }
        }

    }
}