using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entitys;
using WebStore.Domain.EntitysDTO;

namespace WebStore.Services.Map
{
    public static class OrderItemMapper
    {
        public static OrderItemDTO ToDTO(this OrderItem item)
        {
            if (item is null)
                return null;
            return new OrderItemDTO
            {
                Id = item.Id,
                OrderId = item.Order?.Id ?? 0,
                ProductId = item.Product?.Id ?? 0,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice
            };
        }
        public static OrderItem FromDTO(this OrderItemDTO item)
        {
            if (item is null)
                return null;
            return new OrderItem
            {
                Id=item.Id,
                Order=new Order { Id=item.OrderId},
                Product=new Product { Id=item.ProductId},
                Quantity=item.Quantity,
                TotalPrice=item.TotalPrice
            };
        }
    }
}
