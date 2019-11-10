using WebStore.Domain.Entitys.BaseEntitys;

namespace WebStore.Domain.Entitys
{
    public class OrderItem: BaseEntity
    {
        public decimal TotalPrice { get; set; }
        public int Quantity { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}