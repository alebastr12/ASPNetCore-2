using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.EntitysDTO.Identity
{
    public class SetLockoutDTO : UserDTO
    {
        public DateTimeOffset? LockoutEnd { get; set; }
    }
}
