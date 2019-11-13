using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entitys;
using WebStore.Domain.Filters;

namespace WebStore.Areas.Admin.Models
{
    public class AdminProductViewModel
    {
        public ProductFilter filter;
        public IEnumerable<Product> ProductList;
        public IEnumerable<SelectListItem> BrandList;
        public IEnumerable<SelectListItem> CategoryList;
    }
}
