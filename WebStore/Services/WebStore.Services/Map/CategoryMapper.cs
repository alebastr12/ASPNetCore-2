using System;
using System.Collections.Generic;
using System.Text;
using WebStore.Domain.Entitys;
using WebStore.Domain.EntitysDTO;

namespace WebStore.Services.Map
{
    public static class CategoryMapper
    {
        public static CategoryDTO ToDTO(this Category category)
        {
            if (category is null)
                return null;
            return new CategoryDTO
            {
                Id=category.Id,
                Name=category.Name,
                Order=category.Order,
                ParentCategory=category.ParentCategory.ToDTO(),
            };
        }
        public static Category FromDTO(this CategoryDTO category)
        {
            if (category is null)
                return null;
            return new Category
            {
                Id = category.Id,
                Order = category.Order,
                Name = category.Name,
                ParentCategory = category.ParentCategory.FromDTO(),
                ParentId=category.ParentCategory?.Id
            };
        }
    }
}
