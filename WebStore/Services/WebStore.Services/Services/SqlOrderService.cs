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

namespace WebStore.Services.Services
{
    public class SqlOrderService : IOrderService
    {
        private readonly WebStoreContext context;
        private readonly UserManager<User> userManager;

        public SqlOrderService(WebStoreContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public OrderDTO CreateOrder(CreateOrderModel OrderDetails, string UserName)
        {
            var user = userManager.FindByNameAsync(UserName).Result;
            if (user is null)
                throw new InvalidOperationException("Пользователь не найден");
            using(var trans = context.Database.BeginTransaction())
            {
                Order order = new Order
                {
                    Address = OrderDetails.Order.Address,
                    DateTime = DateTime.Now,
                    Phone = OrderDetails.Order.Phone,
                    User = user
                };
                context.Orders.Add(order);
                foreach (var item in OrderDetails.OrderItems)
                {
                    var product = context.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product is null)
                    {
                        throw new InvalidOperationException("Товар не найден в БД");
                    }
                    var orderItem = new OrderItem
                    {
                        Order=order,
                        Product=product,
                        Quantity=item.Quantity,
                        TotalPrice= item.Quantity*product.Price
                    };
                    context.OrderItems.Add(orderItem);
                    
                }
                context.SaveChanges();
                trans.Commit();

                return new OrderDTO { 
                    Id=order.Id,
                    Address=order.Address,
                    DateTime=order.DateTime,
                    Phone = order.Phone,
                    UserName=order.User.UserName,
                    Items = order.Items.Select(i=>new OrderItemDTO {
                        Id=i.Id,
                        OrderId=i.Order.Id,
                        ProductId=i.Product.Id,
                        Quantity=i.Quantity,
                        TotalPrice=i.TotalPrice
                    }
                    )
                };
            }
            

        }

        public OrderDTO GetOrderById(int Id)
        {
            var item = context.Orders.Include(o => o.User).Include(o => o.Items).FirstOrDefault(o => o.Id == Id);
            return item is null ? null :
                new OrderDTO
                {
                    Id = item.Id,
                    Address = item.Address,
                    DateTime = item.DateTime,
                    Phone = item.Phone,
                    UserName = item.User.UserName,
                    Items = item.Items.Select(i => new OrderItemDTO
                    {
                        Id = i.Id,
                        OrderId = i.Order.Id,
                        ProductId = i.Product.Id,
                        Quantity = i.Quantity,
                        TotalPrice = i.TotalPrice
                    }
                    )
                };
        }

        public IEnumerable<OrderDTO> GetOrders(string UserName)
        {
            var orders = context.Orders.Include(o => o.User).Include(o => o.Items).AsEnumerable().Where(o => o.User.UserName == UserName);
            return orders.Select(o => new OrderDTO
            {
                Id = o.Id,
                Address = o.Address,
                DateTime = o.DateTime,
                Phone = o.Phone,
                UserName = o.User.UserName,
                Items = o.Items.Select(i => new OrderItemDTO
                {
                    Id = i.Id,
                    OrderId = i.Order.Id,
                    ProductId = i.Product.Id,
                    Quantity = i.Quantity,
                    TotalPrice = i.TotalPrice
                })
            });
        }
    }
}
