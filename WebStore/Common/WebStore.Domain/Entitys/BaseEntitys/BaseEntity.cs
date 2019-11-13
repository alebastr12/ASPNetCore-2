using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entitys.BaseEntitys.Interface;

namespace WebStore.Domain.Entitys.BaseEntitys
{
    public class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
    }
}
