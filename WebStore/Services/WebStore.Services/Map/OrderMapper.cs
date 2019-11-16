using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Domain.Entitys;
using WebStore.Domain.EntitysDTO;

namespace WebStore.Services.Map
{
    public static class OrderMapper
    {
        public static OrderDTO ToDTO(this Order order)
        {
            if (order is null)
                return null;
            return new OrderDTO
            {
                Id=order.Id,
                Address=order.Address,
                DateTime=order.DateTime,
                Items=order.Items.Select(o=>o.ToDTO()),
                Phone=order.Phone,
                UserName=order.User.UserName
            };
        }
        public static Order FromDTO(this OrderDTO order)
        {
            if (order is null)
                return null;
            return new Order
            {
                Id = order.Id,
                Address = order.Address,
                DateTime = order.DateTime,
                Items = order.Items.Select(OrderItemMapper.FromDTO).ToArray(),
                Phone = order.Phone,
                User= new User { UserName=order.UserName},
            };
        }
    }
}
