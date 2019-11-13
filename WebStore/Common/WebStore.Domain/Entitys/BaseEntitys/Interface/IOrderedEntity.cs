using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.Entitys.BaseEntitys.Interface
{
    public interface IOrderedEntity
    {
        int Order { get; set; }
    }
}
