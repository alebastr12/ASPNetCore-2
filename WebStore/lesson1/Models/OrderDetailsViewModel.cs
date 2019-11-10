using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class OrderDetailsViewModel
    {
        public CartViewModel Cart { get; set; }
        public OrderViewModel Order { get; set; }
    }
}
