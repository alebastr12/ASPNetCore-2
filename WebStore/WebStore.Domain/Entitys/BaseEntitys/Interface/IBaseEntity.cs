using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain
{
    public interface IBaseEntity
    {
        int Id { get; set; }
    }
}
