using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entitys.BaseEntitys.Interface;

namespace WebStore.Models
{
    public class ProductViewModel : INamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public string BrandName { get; set; }
    }
}
