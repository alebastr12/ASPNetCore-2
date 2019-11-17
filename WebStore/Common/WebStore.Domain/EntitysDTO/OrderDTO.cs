using System;
using System.Collections.Generic;
using WebStore.Domain.Entitys.BaseEntitys;

namespace WebStore.Domain.EntitysDTO
{
    public class OrderDTO : BaseEntity
    {
        public string Phone { get; set; }
        public DateTime DateTime { get; set; }
        public string Address { get; set; }
        public string UserName { get; set; }
        public IEnumerable<OrderItemDTO> Items { get; set; }
        
    }
}