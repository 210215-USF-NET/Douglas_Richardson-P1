using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreBL;
using StoreModels;
using StoreMVC.Models;
using StoreMVC.Models.Mappers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace StoreMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly LocationBL locationBL;
        private readonly ProductBL productBL;
        private readonly ItemBL itemBL;
        private readonly CartBL cartBL;
        private readonly OrderBL orderBL;
        private readonly CartMapper cartMapper;
        private readonly UserManager<StoreMVCUser> userManager;
        private readonly LocationMapper locationMapper;
        public CartController(LocationBL locationBL, ProductBL productBL, ItemBL itemBL, CartBL cartBL, OrderBL orderBL, CartMapper cartMapper, UserManager<StoreMVCUser> userManager, LocationMapper locationMapper)
        {
            this.locationBL = locationBL;
            this.productBL = productBL;
            this.itemBL = itemBL;
            this.cartBL = cartBL;
            this.orderBL = orderBL;
            this.cartMapper = cartMapper;
            this.userManager = userManager;
            this.locationMapper = locationMapper;
        }
        [HttpPost]
        public ActionResult UpdateItem(int updatedQuantity, int itemId, int locationId)
        {
            string cartIds = Request.Cookies["customerId"];
            if (cartIds == null)
            {
                var check = User.Identity.IsAuthenticated;
                if (check)
                {
                    cartIds = GetUser().Result.Id;
                }
            }
            List<Cart> carts = cartBL.GetCartFromCustomer(cartIds);
            if (carts != null && carts.Count > 0)
            {
                foreach (var cart in carts)
                {
                    if (cart.Item.Id == itemId && cart.Location.Id == locationId)
                    {
                        cart.Quantity = updatedQuantity;
                        cartBL.AddNewCart(cart);
                    }
                }
            }
            return Cart();
        }

        public ActionResult RemoveItem(int itemId)
        {
            string cartIds = Request.Cookies["customerId"];
            if (cartIds == null)
            {
                var check = User.Identity.IsAuthenticated;
                if (check)
                {
                    cartIds = GetUser().Result.Id;
                }
            }
            List<Cart> carts = cartBL.GetCartFromCustomer(cartIds);
            if (carts != null && carts.Count > 0)
            {
                Cart foundCart = carts.Select(cart => cart).Where(cart => cart.Item.Id == itemId).FirstOrDefault();
                cartBL.RemoveCart(foundCart);
            }
            return Cart();
        }

        public async Task<StoreMVCUser> GetUser()
        {
            return await userManager.GetUserAsync(User);
        }
        public ActionResult Cart()
        {
            string locationCookie = Request.Cookies["locationId"];
            int locationId = 0;
            if (locationCookie != null && !locationCookie.Equals(""))
            {
                try
                {
                    locationId = Int32.Parse(locationCookie);
                }
                catch
                {
                    return RedirectToAction("Locations", "Location");
                }
                Location foundLocation = locationBL.GetLocationFromId(locationId);
                if (foundLocation != null)
                {
                    string cartIds = Request.Cookies["customerId"];
                    if (cartIds == null)
                    {
                        var check = User.Identity.IsAuthenticated;
                        if (check)
                        {
                            cartIds = GetUser().Result.Id;
                        }
                    }
                    List<Cart> carts = cartBL.GetCartFromCustomer(cartIds);
                    if (carts != null && carts.Count > 0)
                    {
                        carts = carts.Select(cart => cart).Where(cart => cart.Location.Id == foundLocation.Id).ToList();
                        List<Item> items = itemBL.GetItems();
                        List<CartModel> listofModels = carts.Select(cart => cartMapper.castCartToCartModel(cart)).ToList();
                        foreach (var cart in listofModels)
                        {
                            //Debug.WriteLine("CartController: cart " + cart.ItemId);
                            Item foundItem = itemBL.GetItemOnId(cart.ItemId);
                            if (foundItem != null)
                            {
                                cart.MaxQuantity = foundItem.Quantity;
                                if (foundItem.Product != null)
                                {
                                    cart.Price = foundItem.Product.Price;
                                    cart.ProductName = foundItem.Product.ProductName;
                                    cart.Category = foundItem.Product.Category;
                                }
                            }
                        }
                        LocationModel locationModel = locationMapper.castLocationModel(foundLocation);
                        //Debug.WriteLine("CartController: found cart and location: "+ locationModel.Id);
                        var tuple1 = new Tuple<List<CartModel>, LocationModel>(listofModels, locationModel);
                        return View("Cart", tuple1);
                        
                    }
                    //Debug.WriteLine("CartController: no cart");
                    LocationModel locationModel2 = locationMapper.castLocationModel(foundLocation);
                    var tuple2 = new Tuple<List<CartModel>, LocationModel>(null, locationModel2);
                    return View("Cart", tuple2);
                }
                
                else
                {
                    return RedirectToAction("Locations", "Location");
                }
            }
            var tuple3 = new Tuple<List<ManagerItemModel>, LocationModel>(null, null);
            return View("Cart", tuple3);
        }//End of cart

        [Authorize]
        public ActionResult SubmitOrder(int locationId)
        {
            string cartIds = Request.Cookies["customerId"];
            if (cartIds == null)
            {
                var check = User.Identity.IsAuthenticated;
                if (check)
                {
                    cartIds = GetUser().Result.Id;
                }
            }
            List<Cart> carts = cartBL.GetCartFromCustomer(cartIds);
            if (carts != null && carts.Count > 0 && locationId != 0)
            {
                carts = carts.Select(cart => cart).Where(cart => cart.Location.Id == locationId).ToList();
                double newTotal = 0.0;
                foreach (var cart in carts)
                {
                    newTotal += (cart.Quantity * cart.Item.Product.Price);
                    cartBL.RemoveCart(cart);
                }
                Debug.WriteLine("New Order");
                Order newOrder = new Order();
                Location foundLocation = locationBL.GetLocationFromId(locationId);
                if (foundLocation != null)
                {
                    newOrder.Location = foundLocation;
                }
                newOrder.Total = newTotal;
                var check = User.Identity.IsAuthenticated;
                if (check)
                {
                    newOrder.Customer = GetUser().Result;
                }
                int orderId = orderBL.PushOrder(newOrder);
                foreach (var cart in carts)
                {
                    OrderItem newOrderItem = new OrderItem {
                        Customer = newOrder.Customer,
                        Item = cart.Item
                    };
                    newOrderItem.Quantity = cart.Quantity;
                    newOrderItem.Order = newOrder;
                    orderBL.AddOrderItem(newOrderItem);
                }
            }
            return Cart();
        }

    }
}
