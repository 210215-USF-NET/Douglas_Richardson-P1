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

namespace StoreMVC.Controllers
{
    public class ManagerLocationController : Controller
    {
        private LocationBL locationBL;
        private LocationMapper locationMapper;
        public ManagerLocationController(LocationBL locationBL, LocationMapper locationMapper)
        {
            this.locationBL = locationBL;
            this.locationMapper = locationMapper;
        }

        public IActionResult ManagerLocations()
        {
            List<Location> locations = locationBL.GetLocations();
            if (locations != null)
            {
                return View(locations.Select(location => locationMapper.castManagerLocationModel(location)).ToList());
            }
            else
            {
                return View(null);
            }
            
        }

        public ActionResult Create()
        {
            return View("CreateLocation");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateLocationModel createLocationModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    locationBL.AddNewLocation(locationMapper.castLocation(createLocationModel));
                    return RedirectToAction(nameof(ManagerLocations));
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        public ActionResult EditLocation(int Id)
        {
            return View(locationMapper.castEditLocationModel(locationBL.GetLocationFromId(Id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLocation(EditLocationModel editLocationModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    locationBL.UpdateLocation(locationMapper.castLocationFromEditLocationModel(editLocationModel));
                    return RedirectToAction(nameof(ManagerLocations));
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
