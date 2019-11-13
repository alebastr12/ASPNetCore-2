using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using WebStore.Domain.Entitys.BaseEntitys;

namespace WebStore.Domain.Entitys
{
    public class Order: BaseEntity
    {
        public string Phone { get; set; }
        public DateTime DateTime { get; set; }
        public string Address { get; set; }
        public virtual User User { get; set; }
        public virtual Collection<OrderItem> Items { get; set; }
    }
}
