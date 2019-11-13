using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entitys.BaseEntitys;
using WebStore.Domain.Entitys.BaseEntitys.Interface;

namespace WebStore.Domain.EntitysDTO
{
    public class ProductDTO : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
        public BrandDTO Brand { get; set; }
        public CategoryDTO Category { get; set; }
    }
}
