using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Interfaces.Services;
using WebStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Filters;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductService _productService;

        public CatalogController(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult ProductDetails(int id)
        {
            var product = _productService.GetProductById(id);
            if (product is null)
                return NotFound();
            return View(new ProductViewModel 
            {
                Id = product.Id,
                ImageUrl = product.ImageUrl,
                Name = product.Name,
                Order = product.Order,
                Price = product.Price,
                BrandName = product.Brand?.Name ?? string.Empty
            });
        }
        public IActionResult Shop(int? CategoryId, int? BrandId)
        {
            var products = _productService.GetProducts(
                new ProductFilter { BrandId = BrandId, CategoryId = CategoryId });

            // сконвертируем в CatalogViewModel
            var model = new CatalogViewModel()
            {
                BrandId = BrandId,
                CategoryId = CategoryId,
                Products = products.Products.Select(p => new ProductViewModel()
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    Name = p.Name,
                    Order = p.Order,
                    Price = p.Price,
                    BrandName=p.Brand.Name
                }).OrderBy(p => p.Order).ToList()
            };

            return View(model);
        }
    }
}