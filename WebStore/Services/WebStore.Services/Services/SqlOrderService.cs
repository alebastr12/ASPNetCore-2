using WebStore.Interfaces.Services;
using WebStore.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL;
using WebStore.Domain.Entitys;
using WebStore.Domain.EntitysDTO;
using WebStore.Services.Map;
using Microsoft.Extensions.Logging;

namespace WebStore.Services.Services
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreContext context;
        private readonly UserManager<User> userManager;
        private readonly ILogger<SqlOrderService> logger;

        public SqlOrderService(WebStoreContext context, UserManager<User> userManager, ILogger<SqlOrderService> logger)
        {
            this.context = context;
            this.userManager = userManager;
            this.logger = logger;
        }
        public OrderDTO CreateOrder(CreateOrderModel OrderDetails, string UserName)
        {
            using (logger.BeginScope($"Попытка добавления заказа для пользователя {UserName}"))
            {
                if (OrderDetails is null || OrderDetails.Order is null)
                {
                    logger.LogError("При попытке добавления заказа обнаружено, что передана пустая ссылка на тип.");
                    throw new ArgumentNullException(nameof(OrderDetails));
                }
                var user = userManager.FindByNameAsync(UserName).Result;
                if (user is null)
                {
                    logger.LogError($"Пользователь с именем {UserName} не найден в системе.");
                    throw new ArgumentException("Пользователь не найден", nameof(UserName));
                }
                if (OrderDetails.OrderItems?.Count > 0)
                    using (var trans = context.Database.BeginTransaction())
                    {
                        Order order = new Order
                        {
                            Address = OrderDetails.Order.Address,
                            DateTime = DateTime.Now,
                            Phone = OrderDetails.Order.Phone,
                            User = user
                        };
                        context.Orders.Add(order);
                        logger.LogInformation($"Добавление заказа: Адрес - {OrderDetails.Order.Address}, телефон - {OrderDetails.Order.Phone}");
                        foreach (var item in OrderDetails.OrderItems)
                        {
                            var product = context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                            if (product is null)
                            {
                                logger.LogError($"Товар заявленный в заказе не найден в БД магазина. (ИД {item.ProductId})");
                                throw new InvalidOperationException("Товар не найден в БД");
                            }
                            var orderItem = new OrderItem
                            {
                                Order = order,
                                Product = product,
                                Quantity = item.Quantity,
                                TotalPrice = item.Quantity * product.Price
                            };
                            context.OrderItems.Add(orderItem);
                            logger.LogInformation($"Добавление продукта с id {product.Id} к заказу.");
                        }
                        if (context.SaveChanges() > 0)
                            logger.LogInformation($"Заказ с id {order.Id} успешно записан в БД");
                        else
                            logger.LogError($"При записи заказа в БД возникли ошибки.");
                        trans.Commit();

                        return order.ToDTO();
                    }
                else
                {
                    logger.LogError($"Заказ содержит пустой список товаров.");
                    throw new ArgumentException("OrderItems не содержит элементов", nameof(OrderDetails));
                }
            }
        }

        public OrderDTO GetOrderById(int Id)
        {
            return context.Orders.Include(o => o.User).Include(o => o.Items).FirstOrDefault(o => o.Id == Id).ToDTO();
        }

        public IEnumerable<OrderDTO> GetOrders(string UserName)
        {
            var orders = context.Orders.Include(o => o.User).Include(o => o.Items).Where(o => o.User.UserName == UserName).ToArray();
            return orders.Select(OrderMapper.ToDTO);
        }
    }
}
