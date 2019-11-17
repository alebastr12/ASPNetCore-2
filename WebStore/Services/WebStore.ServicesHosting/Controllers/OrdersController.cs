using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<OrdersController> logger;
        /// <summary>
        /// Конструктор контроллера заказов
        /// </summary>
        /// <param name="orderService">Сервис для работы с заказами</param>
        /// <param name="logger">Логгер</param>
        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            this.orderService = orderService;
            this.logger = logger;
        }
        /// <summary>
        /// Создание нового заказа в БД
        /// </summary>
        /// <param name="OrderDetails">Объект деталей заказа</param>
        /// <param name="UserName">Имя пользователя создавшего заказ</param>
        /// <returns>Объект заказа</returns>
        [HttpPost("{UserName}")]
        public OrderDTO CreateOrder([FromBody] CreateOrderModel OrderDetails, string UserName)
        {
            if (OrderDetails is null)
            {
                logger.LogError("При создании заказа передана пустая ссылка на OrderDetails");
                throw new ArgumentNullException(nameof(OrderDetails));
            }
            var order = orderService.CreateOrder(OrderDetails, UserName);
            if (order is null)
                logger.LogError("При добавлении заказа возникли ошибки, возвращенна пустая ссылка на заказ");
            else
                logger.LogInformation($"Заказ успешно создан. Идентификатор {order.Id}");
            return order;
        }
        /// <summary>
        /// Получить заказ по идентификатору
        /// </summary>
        /// <param name="Id">Идентификатор заказа</param>
        /// <returns>Объект заказа</returns>   
        [HttpGet("{Id}")]
        public OrderDTO GetOrderById(int Id) => orderService.GetOrderById(Id);
        /// <summary>
        /// Получить все заказы пользователя
        /// </summary>
        /// <param name="UserName">Имя пользователя</param>
        /// <returns>Список заказов пользователя</returns>
        [HttpGet("user/{UserName}")]
        public IEnumerable<OrderDTO> GetOrders(string UserName) => orderService.GetOrders(UserName);
    }
}