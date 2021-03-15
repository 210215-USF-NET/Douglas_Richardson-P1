using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StoreMVC.Models;
using StoreBL;
using StoreMVC.Models.Mappers;
using StoreModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace StoreMVC.Controllers
{
    public class LocationController : Controller
    {
        public const string SessionKeyLocation = "_LocationID";
        public string Session_LocationID { get; private set; }
        public int Id { get; set; }
        private LocationBL locationBL;
        private LocationMapper locationMapper;
        private ProductBL productBL;
        private ItemBL itemBL;
        private ItemMapper itemMapper;
        private CartBL cartBL;
        private readonly UserManager<StoreMVCUser> userManager;
        public LocationController(LocationBL locationBL, LocationMapper locationMapper, ProductBL productBL, ItemBL itemBL, ItemMapper itemMapper, CartBL cartBL, UserManager<StoreMVCUser> userManager)
        {
            this.locationBL = locationBL;
            this.locationMapper = locationMapper;
            this.productBL = productBL;
            this.itemBL = itemBL;
            this.itemMapper = itemMapper;
            this.cartBL = cartBL;
            this.userManager = userManager;
        }

        //This method gets all the locations and displays them to the user, lets the user choose which store they want to choose from
        public IActionResult Locations()
        {
            List<Location> locations = locationBL.GetLocations();
            if (locations != null)
            {
                return View("Locations",locations.Select(location => locationMapper.castLocationModel(location)).ToList());
            }
            return View("Locations", null);
        }

        public IActionResult OneLocation()
        {
            //Response.Cookies myCookie = Request.Cookies["topDogCookie"];
            string locationCookie = Request.Cookies["locationId"];
            if (locationCookie != null)
            {
                int locationId;
                try
                {
                    locationId = Int32.Parse(locationCookie);
                }
                catch
                {
                    return Locations();
                }
                if (locationId != 0)
                {
                    return ChosenLocation(locationId);
                }
            }
            else
            {
                return Locations();
            }
            /*if (locationId != null)
            {
                try
                {
                    HttpContext.Session.SetInt32(SessionKeyLocation, Int32.Parse(locationId));
                }
                catch
                {
                    return Error();
                }
            }
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyLocation)))
            {
                return Locations();
            }
            else
            {
                Id = (int)HttpContext.Session.GetInt32(SessionKeyLocation);
                if (Id != 0)
                {
                    return ChosenLocation(Id);
                }
            }*/
            return Locations();        
        }

        public ActionResult ChosenLocation(int Id)
        {
            Location foundLocation = locationBL.GetLocationFromId(Id);
            if (foundLocation != null)
            {
                //HttpContext.Session.SetInt32(SessionKeyLocation, Id);
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddYears(1);
                Response.Cookies.Append("locationId", Id.ToString(), option);
                List<Item> items = itemBL.GetItems();
                if (items != null && items.Count > 0)
                {
                    items = items.Select(item => item).Where(item => item.ItemLocation != null).ToList();
                    items = items.Select(item => item).Where(item => item.ItemLocation.Id == Id).ToList();
                    List<ManagerItemModel> listofModels = items.Select(item => itemMapper.castManagerItemModel(item)).ToList();
                    LocationModel locationModel = locationMapper.castLocationModel(foundLocation);
                    var tuple = new Tuple<List<ManagerItemModel>, LocationModel>(listofModels, locationModel);
                    return View("OneLocation", tuple);
                }
                else
                { 
                    LocationModel locationModel = locationMapper.castLocationModel(foundLocation);
                    var tuple = new Tuple<List<ManagerItemModel>, LocationModel>(null, locationModel);
                    return View("OneLocation", tuple);
                }
            }

            return View("Locations", null);
        }

        public async Task<StoreMVCUser> GetUser()
        {
            return await userManager.GetUserAsync(User);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //Add the selected tiem to their cart
        public IActionResult AddItem(ManagerItemModel managerItemModel)
        {
            Debug.WriteLine("item id is " + managerItemModel.ItemId + " amount: " + managerItemModel.ChosenAmount + " location id: "+ managerItemModel.LocationId);
            //check if theres a customer, if not then check cookie for carts
            var principal = User as ClaimsPrincipal;
            var check = User.Identity.IsAuthenticated;
            StoreMVCUser Customer = new StoreMVCUser();

            Cart newCart = new Cart
            {
                CustomerId = Customer.Id
            };

            Item foundItem = itemBL.GetItemOnId(managerItemModel.ItemId);
            if (foundItem != null)
            {
                newCart.Item = foundItem;
            }
            else
            {
                newCart.Item = new Item();
                newCart.Item.Id = managerItemModel.ItemId;
            }

            Location foundLocation = locationBL.GetLocationFromId(managerItemModel.LocationId);
            if (foundLocation != null)
            {
                newCart.Location = foundLocation;
            }
            else
            {
                newCart.Location = new Location();
                newCart.Location.Id = managerItemModel.LocationId;
            }

            newCart.Quantity = managerItemModel.ChosenAmount;
            if (check)
            {
                Debug.WriteLine("LocationController: Customer is logged in");
                newCart.CustomerId = GetUser().Result.Id;
                cartBL.AddNewCart(newCart);
            }
            else
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddYears(1);
                
                string cartIds = Request.Cookies["customerId"];
                if (cartIds == null)
                {
                    Response.Cookies.Append("customerId", newCart.CustomerId, option);
                }
                else
                {
                    newCart.CustomerId = cartIds;
                }//End of check carts
                List<Cart> carts = cartBL.GetCartFromCustomer(newCart.CustomerId);
                if (carts != null)
                {
                    foreach (var cart in carts)
                    {
                        if (cart.Item.Id == newCart.Item.Id && cart.Location.Id == foundLocation.Id)
                        {
                            cart.Quantity = newCart.Quantity;
                            cartBL.AddNewCart(cart);
                            ViewBag.Message = "Changed " + managerItemModel.ProductName + " to "+managerItemModel.ChosenAmount;
                            return OneLocation();
                        }
                    }
                }
                cartBL.AddNewCart(newCart);
            }//End of check user
            
            
            ViewBag.Message = "Added "+managerItemModel.ChosenAmount+" of "+managerItemModel.ProductName;
            return OneLocation();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
