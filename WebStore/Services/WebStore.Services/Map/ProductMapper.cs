using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entitys;
using WebStore.Domain.EntitysDTO;

namespace WebStore.Services.Map
{
    public static class ProductMapper
    {
        public static ProductDTO ToDTO(this Product product)
        {
            if (product is null)
                return null;
            return new ProductDTO
            {
                Id=product.Id,
                Name=product.Name,
                Order=product.Order,
                Brand=product.Brand.ToDTO(),
                Category=product.Category.ToDTO(),
                ImageUrl=product.ImageUrl,
                Price=product.Price
            };
        }
        public static Product FromDTO(this ProductDTO product)
        {
            if (product is null)
                return null;
            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                Order = product.Order,
                Brand = product.Brand.FromDTO(),
                Category = product.Category.FromDTO(),
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                BrandId=product.Brand?.Id ?? 0,
                CategoryId=product.Category?.Id??0
            };
        }
    }
}
