using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Models;

namespace WebStore.Domain.EntitysDTO
{
    public class CreateOrderModel
    {
        public OrderViewModel Order { get; set; }
        public List<OrderItemDTO> OrderItems {get;set;}
    }
}
