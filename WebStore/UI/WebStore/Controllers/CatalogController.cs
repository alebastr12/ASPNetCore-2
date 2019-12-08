using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Interfaces.Services;
using WebStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Filters;
using Microsoft.Extensions.Configuration;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductService _productService;
        private readonly IConfiguration config;

        public CatalogController(IProductService productService, IConfiguration config)
        {
            _productService = productService;
            this.config = config;
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
        public IActionResult Shop(int? CategoryId, int? BrandId, int Page = 1)
        {
            var page_size = int.Parse(config["PageSize"]);
            var products = _productService.GetProducts(
                new ProductFilter { BrandId = BrandId, CategoryId = CategoryId , Page=Page, PageSize=page_size});

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
                }).OrderBy(p => p.Order).ToList(),
                PageViewModel=new PageViewModel
                {
                    PageNumber = Page,
                    PageSize = page_size,
                    TotalItems = products.TotalCount
                }
            };

            return View(model);
        }
        public IActionResult GetFilteredProducts(int? CategoryId, int? BrandId, int Page = 1)
        {
            var products = GetProducts(CategoryId, BrandId, Page);
            return PartialView("Partial/_ProductItems", products);
        }
        private IEnumerable<ProductViewModel> GetProducts(int? CategoryId, int? BrandId, int Page)
        {
            var products_dto = _productService.GetProducts(new ProductFilter
            {
                BrandId = BrandId,
                CategoryId = CategoryId,
                Page = Page,
                PageSize = int.Parse(config["PageSize"])
            });
            return products_dto.Products.Select(p => new ProductViewModel()
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Order = p.Order,
                Price = p.Price,
                BrandName = p.Brand.Name
            });
        }
    }
}