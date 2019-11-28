using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.EntitysDTO;
using WebStore.Domain.Models;
using WebStore.Interfaces.Services;

namespace WebStore.ViewComponents
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductService _ProductData;

        public BreadCrumbsViewComponent(IProductService ProductData) => _ProductData = ProductData;

        public IViewComponentResult Invoke(BreadCrumbType Type, int id, BreadCrumbType FromType)
        {
            switch (Type)
            {
                default: return View(Array.Empty<BreadCrumbViewModel>());

                case BreadCrumbType.Category:
                    return View(
                        new[]
                        {
                            new BreadCrumbViewModel
                            {
                                BreadCrumbType = Type,
                                Id = id.ToString(),
                                Name = _ProductData.GetCategoryById(id).Name
                            }
                        });

                case BreadCrumbType.Brand:
                    return View(
                        new[]
                        {
                            new BreadCrumbViewModel
                            {
                                BreadCrumbType = Type,
                                Id = id.ToString(),
                                Name = _ProductData.GetBrandById(id).Name
                            }
                        });

                case BreadCrumbType.Product:
                    return View(GetProductBreadCrumbs(_ProductData.GetProductById(id), FromType));
            }
        }
        private static IEnumerable<BreadCrumbViewModel> GetProductBreadCrumbs(ProductDTO Product, BreadCrumbType FromType) =>
            new[]
            {
                new BreadCrumbViewModel
                {
                    BreadCrumbType = FromType,
                    Id = FromType == BreadCrumbType.Category
                         ? Product.Category.Id.ToString()
                         : Product.Brand.Id.ToString(),
                    Name = FromType == BreadCrumbType.Category
                           ? Product.Category.Name
                           : Product.Brand.Name
                },
                new BreadCrumbViewModel
                {
                    BreadCrumbType = BreadCrumbType.Product,
                    Id = Product.Id.ToString(),
                    Name = Product.Name
                }
            };
    }
}
