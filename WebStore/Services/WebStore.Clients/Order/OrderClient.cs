using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebStore.Clients.Base;
using WebStore.Domain.EntitysDTO;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Order
{
    public class OrderClient : BaseClient, IOrderService
    {
        public OrderClient(IConfiguration conf) : base(conf, "api/orders")
        {

        }
        public OrderDTO CreateOrder(CreateOrderModel OrderDetails, string UserName) =>
            Post($"{_serviceAddress}/{UserName}", OrderDetails)
            .Content
            .ReadAsAsync<OrderDTO>()
            .Result;

        public OrderDTO GetOrderById(int Id) => Get<OrderDTO>($"{_serviceAddress}/{Id}");

        public IEnumerable<OrderDTO> GetOrders(string UserName) => Get<List<OrderDTO>>($"{_serviceAddress}/user/{UserName}");
    }
}
