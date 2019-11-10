using WebStore.Interfaces.Services;
using WebStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Filters;

namespace WebStore.ViewComponents
{
    public class Brands:ViewComponent
    {
        private readonly IProductService _productService;

        public Brands(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var brands = GetBrands();
            return View(brands);
        }

        private object GetBrands()
        {
            return _productService.GetBrands().Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name,
                Order = b.Order,
                ProductCount = b.Products?.Count() ?? 0
                //ProductCount = _productService.GetProducts(new ProductFilter { BrandId = b.Id, CategoryId = null }).Count()
            }).OrderBy(b => b.Order).ToList();
            
        }
    }
}
