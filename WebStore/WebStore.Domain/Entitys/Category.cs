using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WebStore.Domain.Entitys.BaseEntitys;
using WebStore.Domain.Entitys.BaseEntitys.Interface;

namespace WebStore.Domain.Entitys
{
    public class Category : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }
        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
