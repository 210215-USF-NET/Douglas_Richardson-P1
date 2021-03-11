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
        public LocationController(LocationBL locationBL, LocationMapper locationMapper, ProductBL productBL, ItemBL itemBL, ItemMapper itemMapper)
        {
            this.locationBL = locationBL;
            this.locationMapper = locationMapper;
            this.productBL = productBL;
            this.itemBL = itemBL;
            this.itemMapper = itemMapper;
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
            string locationId = Request.Cookies["locationId"];
            if (locationId != null)
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
            }
            return Locations();        
        }

        public ActionResult ChosenLocation(int Id)
        {
            Location foundLocation = locationBL.GetLocationFromId(Id);
            if (foundLocation != null)
            {
                HttpContext.Session.SetInt32(SessionKeyLocation, Id);
                Response.Cookies.Append("locationId", Id.ToString());
                List<Item> items = itemBL.GetItems();
                if (items != null)
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
