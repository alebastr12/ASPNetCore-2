using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entitys;

namespace WebStore.Domain.EntitysDTO.Identity
{
    public abstract class UserDTO
    {
        public User User { get; set; }
    }
}
