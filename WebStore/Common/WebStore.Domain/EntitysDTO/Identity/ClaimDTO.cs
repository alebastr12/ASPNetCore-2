using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace WebStore.Domain.EntitysDTO.Identity
{
    public abstract class ClaimDTO : UserDTO
    {
        public IEnumerable<Claim> Claims { get; set; }
    }
}
