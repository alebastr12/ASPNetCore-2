using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entitys;
using WebStore.Domain.EntitysDTO;

namespace WebStore.Services.Map
{
    public static class BrandMapper
    {
        public static BrandDTO ToDTO(this Brand brand)
        {
            if (brand is null)
                return null;
            return new BrandDTO
            {
                Id=brand.Id,
                Name=brand.Name,
                Order=brand.Order
            };
        }
        public static Brand FromDTO(this BrandDTO brand)
        {
            if (brand is null)
                return null;
            return new Brand
            {
                Id=brand.Id,
                Name=brand.Name,
                Order=brand.Order
            };
        }
    }
}
