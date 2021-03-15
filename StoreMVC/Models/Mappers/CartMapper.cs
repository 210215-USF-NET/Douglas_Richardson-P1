using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreModels;
namespace StoreMVC.Models.Mappers
{
    public class CartMapper
    {
        public CartModel castCartToCartModel(Cart cart)
        {
            return new CartModel
            {
                Quantity = cart.Quantity,
                ProductName = "",
                ItemId = cart.Item.Id,
                Price = 0.0,
                Category = Category.Nothing,
                CustomerId = cart.CustomerId,
                LocationId = cart.Location.Id,
            };
        }
    }
}
