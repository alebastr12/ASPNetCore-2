using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entitys.BaseEntitys;

namespace WebStore.Domain.EntitysDTO
{
    public class OrderItemDTO : BaseEntity
    {
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
    }
}
