using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreBL;
using StoreModels;
using StoreMVC.Models;
using StoreMVC.Models.Mappers;
using System;
using System.Collections.Generic;
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

        public ActionResult UpdateItem(int updatedQuantity, int itemId, string customerId)
        {
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
                    if (carts != null)
                    {
                        //TODO: get the data from the itemBL and put it in the cartModel
                        List<CartModel> listofModels = carts.Select(cart => cartMapper.castCartToCartModel(cart)).ToList();
                        LocationModel locationModel = locationMapper.castLocationModel(foundLocation);
                        var tuple1 = new Tuple<List<CartModel>, LocationModel>(listofModels, locationModel);
                        return RedirectToAction("Cart", "Home", tuple1);
                    }
                    else
                    {
                        LocationModel locationModel = locationMapper.castLocationModel(foundLocation);
                        var tuple2 = new Tuple<List<CartModel>, LocationModel>(null, locationModel);
                        return RedirectToAction("Cart", "Home", tuple2);
                    }
                }
                
                else
                {
                    return RedirectToAction("Locations", "Location");
                }
            }
            var tuple3 = new Tuple<List<ManagerItemModel>, LocationModel>(null, null);
            return RedirectToAction("Cart", "Home", tuple3);
        }
    }
}
