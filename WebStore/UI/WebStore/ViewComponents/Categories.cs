using WebStore.Interfaces.Services;
using WebStore.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.ViewComponents
{
    public class Categories:ViewComponent
    {
        private readonly IProductService _productService;

        public Categories(IProductService iproductservice)
        {
            _productService = iproductservice;
        }
        public async Task<IViewComponentResult> InvokeAsync(string CategoryId)
        {
            var current_section_id = int.TryParse(CategoryId, out var Id) ? Id : (int?)null;
            var Categories = GetCategories(current_section_id, out int? ParrentId);
            return View(new CategoryCompleteViewModel
            {
                Categories = Categories,
                CurrentParrentCategory=ParrentId,
                CurrentcategoryId= current_section_id
            });
           
        }
        private List<CategoryViewModel> GetCategories(int? CurrentId, out int? ParrentId)
        {
            ParrentId = null;
            var categories = _productService.GetCategories();
            // получим и заполним родительские категории
            var parentSections = categories.Where(p => (p.ParentCategory is null)).ToArray();
            var parentCategories = new List<CategoryViewModel>();
            foreach (var parentCategory in parentSections)
            {
                parentCategories.Add(new CategoryViewModel()
                {
                    Id = parentCategory.Id,
                    Name = parentCategory.Name,
                    Order = parentCategory.Order,
                    ParentCategory = null
                });
            }
            // получим и заполним дочерние категории
            foreach (var CategoryViewModel in parentCategories)
            {
                var childCategories = categories.Where(c => c.ParentCategory?.Id == CategoryViewModel.Id);
                foreach (var childCategory in childCategories)
                {
                    if (childCategory.Id == CurrentId)
                        ParrentId = CategoryViewModel.Id;
                    CategoryViewModel.ChildCategories.Add(new CategoryViewModel()
                    {
                        Id = childCategory.Id,
                        Name = childCategory.Name,
                        Order = childCategory.Order,
                        ParentCategory = CategoryViewModel
                    });
                }
                CategoryViewModel.ChildCategories = CategoryViewModel.ChildCategories.OrderBy(c => c.Order).ToList();
            }
            parentCategories = parentCategories.OrderBy(c => c.Order).ToList();
            return parentCategories;
        }
    }
}

