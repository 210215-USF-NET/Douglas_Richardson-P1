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
using Microsoft.AspNetCore.Authorization;

namespace StoreMVC.Controllers
{

    [Authorize(Roles = "Manager")]
    public class ManagerItemController : Controller
    {
        private LocationBL locationBL;
        private ProductBL productBL;
        private ItemBL itemBL;
        private ItemMapper itemMapper;
        private LocationMapper locationMapper;
        public ManagerItemController(LocationBL locationBL, ProductBL productBL, ItemBL itemBL, ItemMapper itemMapper, LocationMapper locationMapper)
        {
            this.locationBL = locationBL;
            this.itemBL = itemBL;
            this.productBL = productBL;
            this.itemMapper = itemMapper;
            this.locationMapper = locationMapper;
        }
        public IActionResult ManagerItems()
        {
            List<Item> items = itemBL.GetItems();
            //Item item = itemBL.GetItemOnId(4);
            if (items != null)
            {
                //items.Add(item);
                return View("DisplayItems", items.Select(item => itemMapper.castManagerItemModel(item)).ToList());
            }
            else
            {
                return View("DisplayItems",null);
            }
            
        }

        public ActionResult Create()
        {
            return View("CreateItem");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateItemModel createItemModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Product newProduct = new Product
                    {
                        ProductName = createItemModel.ProductName,
                        Price = createItemModel.Price,
                        Category = createItemModel.Category
                    };
                    Item newItem = itemMapper.castItem(createItemModel);
                    newItem.Product.Id = productBL.AddProduct(newProduct);
                    itemBL.AddItemToRepo(newItem);
                    return RedirectToAction(nameof(ManagerItems));
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        public ActionResult EditItem(int Id)
        {
            return View(itemMapper.castEditItemModel(itemBL.GetItemOnId(Id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditItem(EditItemModel editItemModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Item getItem = itemMapper.castItemFromEditItemModel(editItemModel);
                    itemBL.UpdateItem(getItem);
                    return RedirectToAction(nameof(ManagerItems));
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }

        public ActionResult DeleteItem(int Id)
        {
            itemBL.DeleteItem(Id);
            return RedirectToAction(nameof(ManagerItems));
        }

        public ActionResult ChooseLocation(int Id)
        {
            List<Location> locations = locationBL.GetLocations();
            if (locations != null)
            {
                List<ManagerLocationModel> listofModels = locations.Select(location => locationMapper.castManagerLocationModel(location)).ToList();
                EditItemModel editItemModel = itemMapper.castEditItemModel(itemBL.GetItemOnId(Id));
                var tuple = new Tuple<List<ManagerLocationModel>, EditItemModel>(listofModels, editItemModel);
                return View("ChooseLocationForItem", tuple);
            }
            else
            {
                return RedirectToAction("ManagerLocations","ManagerLocation",null);
            }
            
        }

        public ActionResult ChooseLocationAction(int LocationId, int ItemId)
        {
            Item getItem = itemBL.GetItemOnId(ItemId);
            Location newLocation = new Location();
            getItem.ItemLocation = newLocation;
            getItem.ItemLocation.Id = LocationId;
            itemBL.UpdateItem(getItem);
            return RedirectToAction(nameof(ManagerItems));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
