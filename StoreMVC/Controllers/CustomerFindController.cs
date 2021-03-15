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
using Microsoft.AspNetCore.Identity;

namespace StoreMVC.Controllers
{

    [Authorize]
    public class CustomerFindController : Controller
    {
        private readonly OrderBL orderBL;
        private readonly UserManager<StoreMVCUser> userManager;
        public CustomerFindController(OrderBL orderBL, UserManager<StoreMVCUser> userManager)
        {
            this.orderBL = orderBL;
            this.userManager = userManager;
        }
        public ActionResult FindCustomerMenu()
        {
            FindUserModel findUserModel = new FindUserModel();
            return View("SearchCustomer",findUserModel);
        }

        public ActionResult FindUserHistory(FindUserModel findUserModel)
        {
            List<OrderHistoryModel> userOrders = orderBL.GetOrder(findUserModel.Email);
            return View("ListCustomerOrders", userOrders);
        }

        [Route("/CustomerFind/FindHistoryByRoute/{email}")]
        public ActionResult FindHistoryByRoute(string email)
        {
            List<OrderHistoryModel> userOrders = orderBL.GetOrder(email);
            return View("ListCustomerOrders", userOrders);
        }

        public ActionResult DetailsOfOrder(int orderId, string email)
        {
            List<ItemModel> userItems = orderBL.GetOrderItems(orderId);
            var tuple = new Tuple<List<ItemModel>, string>(userItems, email);
            return View("ListCustomerItems", tuple);
        }

        public async Task<StoreMVCUser> GetUser()
        {
            return await userManager.GetUserAsync(User);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
