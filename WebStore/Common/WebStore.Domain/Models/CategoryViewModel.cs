using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entitys.BaseEntitys.Interface;

namespace WebStore.Domain.Models
{
    public class CategoryViewModel : INamedEntity, IOrderedEntity
    {
        public CategoryViewModel()
        {
            ChildCategories = new List<CategoryViewModel>();
        }
        public int Order { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        /// <summary>
        /// Список дочерних категорий
        /// </summary>
        public List<CategoryViewModel> ChildCategories { get; set; }

        /// <summary>
        /// Родительская категория
        /// </summary>
        public CategoryViewModel ParentCategory { get; set; }
    }
}
