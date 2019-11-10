using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.Entitys.BaseEntitys.Interface
{
    public interface INamedEntity:IBaseEntity
    {
        string Name { get; set; }
    }
}
