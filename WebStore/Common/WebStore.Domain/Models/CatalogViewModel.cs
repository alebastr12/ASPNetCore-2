﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Domain.Models
{
    public class CatalogViewModel
    {

        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
