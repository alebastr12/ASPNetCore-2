using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entitys.BaseEntitys;
using WebStore.Domain.Entitys.BaseEntitys.Interface;

namespace WebStore.Domain.EntitysDTO
{
    public class BrandDTO : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        //public IEnumerable<ProductDTO> Products { get; set; }
    }
}
