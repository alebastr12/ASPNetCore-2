using WebStore.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entitys;
using WebStore.Domain.EntitysDTO;

namespace WebStore.Interfaces.Services
{
    public interface IOrderService
    {
        IEnumerable<OrderDTO> GetOrders(string UserName);
        OrderDTO GetOrderById(int Id);
        OrderDTO CreateOrder(OrderDetailsViewModel OrderDetails, string UserName);

    }
}
