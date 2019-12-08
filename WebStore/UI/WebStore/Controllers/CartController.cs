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
using Microsoft.Extensions.Logging;

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
        public IActionResult CheckOut(OrderDetailsViewModel model, [FromServices] ILogger<CartController> log)
        {
            if (ModelState.IsValid)
            {
                log.LogInformation($"Отправка нового заказа для сохранения в БД для пользователя {User.Identity.Name}");
                OrderDTO OrderDetails;
                try
                {
                    OrderDetails = orderService.CreateOrder(new CreateOrderModel
                    {
                        OrderItems = cartService.TransformCart().Items.Select(e => new OrderItemDTO
                        {
                            ProductId = e.Key.Id,
                            Quantity = e.Value,

                        }).ToList(),
                        //Cart=cartService.TransformCart(),
                        Order = model.Order
                    }, User.Identity.Name);
                }
                catch (Exception e)
                {
                    log.LogError($"При обращении к сервису заказов возникли проблемы. {e.Message}");
                    throw; //Здесь нужен редирект на что-то осмысленное.
                }
                cartService.RemoveAll();
                log.LogInformation($"Заказ создан и записан в БД. Идентфикатор заказа {OrderDetails.Id}");
                return RedirectToAction("OrderConfirmed", new { id = OrderDetails.Id });
            }
            else
            {
                log.LogError($"При оформлении заказа передана не валидная модель.");
                return View("Details", new OrderDetailsViewModel
                {
                    Cart = cartService.TransformCart(),
                    Order = model.Order
                });
            }
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
        #region API
        [AllowAnonymous]
        public IActionResult GetCartView() => ViewComponent("Cart");

        public IActionResult AddToCartAPI(int id)
        {
            cartService.AddToCart(id);
            return Json(new { id, message = $"Товар с идентификатором: {id} добавлен в корзину" });
        }
        public IActionResult DecrementFromCartAPI(int id)
        {
            cartService.DecrementFromCart(id);
            return Json(new { id, message = $"Товар с идентификатором: {id} убран из корзины" });
        }

        public IActionResult RemoveFromCartAPI(int id)
        {
            cartService.RemoveFromCart(id);
            return Json(new { id, message = $"Товар с идентификатором: {id} удвлен из корзину" });
        }

        public IActionResult RemoveAllAPI()
        {
            cartService.RemoveAll();
            return Json(new {message = $"Корзина очищенна." });
        }
        #endregion

    }
}