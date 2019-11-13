using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.EntitysDTO;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.ServicesHosting.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase, IOrderService
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }
        [HttpPost("{UserName}")]
        public OrderDTO CreateOrder([FromBody] OrderDetailsViewModel OrderDetails, string UserName) =>
            orderService.CreateOrder(OrderDetails, UserName);
        [HttpGet("{Id}")]
        public OrderDTO GetOrderById(int Id) => orderService.GetOrderById(Id);
        [HttpGet("user/{UserName}")]
        public IEnumerable<OrderDTO> GetOrders(string UserName) => orderService.GetOrders(UserName);
    }
}