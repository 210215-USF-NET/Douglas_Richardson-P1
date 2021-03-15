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

    [Authorize(Roles = "Manager")]
    public class ManagerFindController : Controller
    {
        private readonly OrderBL orderBL;
        private readonly UserManager<StoreMVCUser> userManager;
        public ManagerFindController(OrderBL orderBL, UserManager<StoreMVCUser> userManager)
        {
            this.orderBL = orderBL;
            this.userManager = userManager;
        }
        public ActionResult FindCustomerMenu()
        {
            FindUserModel findUserModel = new FindUserModel();
            return View("SearchCustomer",findUserModel);
        }

        [HttpPost]
        public ActionResult FindUserHistory(FindUserModel findUserModel)
        {
            var tuple = orderBL.GetOrder(findUserModel.Email);
            return View("ListCustomerOrders", tuple);
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
