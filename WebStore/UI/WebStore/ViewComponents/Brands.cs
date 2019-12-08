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
        public async Task<IViewComponentResult> InvokeAsync(string BrandId)
        {
            
            return View(new BrandCompleteViewModel
            {
                Brands = GetBrands(),
                CurrentBrandId = int.TryParse(BrandId, out var Id)?Id:(int?)null
            });
        }

        private IEnumerable<BrandViewModel> GetBrands()
        {
            return _productService.GetBrands().Select(b => new BrandViewModel
            {
                Id = b.Id,
                Name = b.Name,
                Order = b.Order,
                ProductCount = _productService.GetProducts(new ProductFilter { BrandId = b.Id, CategoryId = null }).Products.Count()
            }).OrderBy(b => b.Order).ToList();
            
        }
    }
}
