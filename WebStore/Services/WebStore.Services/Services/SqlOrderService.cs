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
        public Order CreateOrder(OrderDetailsViewModel OrderDetails, string UserName)
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
                foreach (var item in OrderDetails.Cart.Items)
                {
                    var productVM = item.Key;
                    var product = context.Products.FirstOrDefault(p => p.Id == productVM.Id);
                    if (product is null)
                    {
                        throw new InvalidOperationException("Товар не найден в БД");
                    }
                    var orderItem = new OrderItem
                    {
                        Order=order,
                        Product=product,
                        Quantity=item.Value,
                        TotalPrice= item.Value*product.Price
                    };
                    context.OrderItems.Add(orderItem);
                    
                }
                context.SaveChanges();
                trans.Commit();

                return order;
            }
            

        }

        public Order GetOrderById(int Id)
        {
            return context.Orders.Include(o => o.User).Include(o => o.Items).FirstOrDefault(o => o.Id == Id);
        }

        public IEnumerable<Order> GetOrders(string UserName)
        {
            return context.Orders.Include(o => o.User).Include(o => o.Items).Where(o => o.User.UserName == UserName).ToList();
        }
    }
}
