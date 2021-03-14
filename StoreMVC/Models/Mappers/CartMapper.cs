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
            return new ManagerItemModel
            {
                Quantity = cart.Quantity,
                ItemId = cart.Id,
                ProductName = cart.Product.ProductName,
                Price = cart.Product.Price,
                Category = cart.Product.Category,
                ProductId = cart.Product.Id
            };
        }
    }
}
