using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.EntitysDTO.Identity
{
    public class PasswordHashDTO : UserDTO
    {
        public string Hash { get; set; }
    }
}
