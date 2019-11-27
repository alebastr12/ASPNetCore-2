
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebStore.Controllers;
using WebStore.Domain.Entitys;
using WebStore.Domain.EntitysDTO;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;
using Assert = Xunit.Assert;

namespace WebStore.Tests
{
    [TestClass]
    public class CartControllerTest
    {
        [TestMethod]
        public void CheckOut_ModelState_Invalid_Returns_ViewModel()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }));

            var store = new Mock<IUserStore<User>>();
            var user_manager_mock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var cart_service_mock = new Mock<ICartService>();
            var order_service_mock = new Mock<IOrderService>();
            var logger_mock = new Mock<ILogger<CartController>>();

            var controller = new CartController(cart_service_mock.Object, user_manager_mock.Object, order_service_mock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = user
                    }
                }
            };
            controller.ModelState.AddModelError("error", "Bad model");
            const string expected_phone_order = "123456";
            const string expected_addres_order = "addres";

            var result = controller.CheckOut(new OrderDetailsViewModel
            {
                Order = new OrderViewModel
                {
                    Address = expected_addres_order,
                    Phone = expected_phone_order
                }
            }, logger_mock.Object);

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<OrderDetailsViewModel>(view_result.Model);

            Assert.Equal(expected_addres_order, model.Order.Address);
            Assert.Equal(expected_phone_order, model.Order.Phone);
        }
        [TestMethod]
        public void CheckOut_Redirect_To()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }));

            var store = new Mock<IUserStore<User>>();
            var user_manager_mock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var cart_service_mock = new Mock<ICartService>();
            var order_service_mock = new Mock<IOrderService>();
            var logger_mock = new Mock<ILogger<CartController>>();
            cart_service_mock
               .Setup(c => c.TransformCart())
               .Returns(() => new CartViewModel
               {
                   Items = new Dictionary<ProductViewModel, int>
                    {
                        { new ProductViewModel(), 1 }
                    }
               });

            const int expected_order_id = 1;
            order_service_mock
               .Setup(c => c.CreateOrder(It.IsAny<CreateOrderModel>(), It.IsAny<string>()))
               .Returns(new OrderDTO { Id = expected_order_id });

            var controller = new CartController(cart_service_mock.Object, user_manager_mock.Object, order_service_mock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = user
                    }
                }
            };
            var result = controller.CheckOut(new OrderDetailsViewModel
            {
                Order = new OrderViewModel
                {
                    Address = "Addres",
                    Phone = "Phone"
                }
            }, logger_mock.Object);

            var redirect_result = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirect_result.ControllerName);
            Assert.Equal(nameof(CartController.OrderConfirmed), redirect_result.ActionName);

            Assert.Equal(expected_order_id, redirect_result.RouteValues["id"]);
        }
        [TestMethod]
        public void Details_Correct_Model()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "1") }));

            var store = new Mock<IUserStore<User>>();
            var user_manager_mock = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var cart_service_mock = new Mock<ICartService>();
            var order_service_mock = new Mock<IOrderService>();
            var logger_mock = new Mock<ILogger<CartController>>();

            int expected_product_count = 3;
            string expected_phone = "1234";
            user_manager_mock
                .Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns<ClaimsPrincipal>(u =>                 
                    Task.FromResult(new User { UserName = "User" })
                );
            user_manager_mock
                .Setup(u => u.GetPhoneNumberAsync(It.IsAny<User>()))
                .Returns<User>(u => Task.FromResult(expected_phone));
            cart_service_mock
               .Setup(c => c.TransformCart())
               .Returns(() => new CartViewModel
               {
                   Items = new Dictionary<ProductViewModel, int>
                    {
                        { new ProductViewModel(), expected_product_count }
                    }
               });
            var controller = new CartController(cart_service_mock.Object, user_manager_mock.Object, order_service_mock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = user
                    }
                }
            };
            var result = controller.Details();

            var view_result = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<OrderDetailsViewModel>(view_result.Model);

            Assert.Equal(expected_phone, model.Order.Phone);
            Assert.Equal(expected_product_count, model.Cart.ItemsCount);
        }
    }
}
