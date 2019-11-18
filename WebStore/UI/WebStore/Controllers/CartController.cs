using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Interfaces.Services;
using WebStore.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entitys;
using WebStore.Domain.EntitysDTO;

namespace WebStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly UserManager<User> userManager;
        private readonly IOrderService orderService;

        public CartController(ICartService cartService,UserManager<User> userManager, IOrderService orderService)
        {
            this.cartService = cartService;
            this.userManager = userManager;
            this.orderService = orderService;
        }
        public IActionResult Details()
        {
            User user = userManager.GetUserAsync(User).Result;
            var model = new OrderDetailsViewModel
            {
                Cart = cartService.TransformCart(),
                Order = new OrderViewModel { Phone=userManager.GetPhoneNumberAsync(user).Result}
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckOut(OrderDetailsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var OrderDetails = orderService.CreateOrder(new CreateOrderModel
                {
                    OrderItems = cartService.TransformCart().Items.Select(e=>new OrderItemDTO { 
                        ProductId=e.Key.Id,
                        Quantity=e.Value,
                        
                    }).ToList(),
                    //Cart=cartService.TransformCart(),
                    Order=model.Order
                }, User.Identity.Name);
                cartService.RemoveAll();
                return RedirectToAction("OrderConfirmed", new { id = OrderDetails.Id });
            };
            
            return View("Details", new OrderDetailsViewModel
            {
                Cart = cartService.TransformCart(),
                Order = model.Order
            });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
        public IActionResult DecrementFromCart(int id)
        {
            cartService.DecrementFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveFromCart(int id)
        {
            cartService.RemoveFromCart(id);
            return RedirectToAction("Details");
        }

        public IActionResult RemoveAll()
        {
            cartService.RemoveAll();
            return RedirectToAction("Details");
        }

        public IActionResult AddToCart(int id, string returnUrl)
        {
            cartService.AddToCart(id);
            return Redirect(returnUrl);
        }

    }
}