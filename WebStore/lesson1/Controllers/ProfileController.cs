using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Infrastructure.Interfaces;
using WebStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebStore.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IOrderService orderService;

        public ProfileController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Orders()
        {
            var orders = orderService.GetOrders(User.Identity.Name);
            var userOrdersModel = new List<UserOrderViewModel>(orders.Count());
            foreach (var item in orders)
            {
                userOrdersModel.Add(new UserOrderViewModel
                {
                    Id=item.Id,
                    Order=new OrderViewModel
                    {
                        Address=item.Address,
                        Phone=item.Phone
                    },
                    TotalPrice=item.Items.Sum(i=>i.TotalPrice)
                });
            }
            return View(userOrdersModel);
        }
    }
}