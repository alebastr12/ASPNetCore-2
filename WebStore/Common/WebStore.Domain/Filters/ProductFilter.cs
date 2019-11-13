using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebStore.Domain.Filters
{
    public class ProductFilter
    {
        [Display(Name ="Имя")]
        public string Name { get; set; }
        [Display(Name = "Категория")]
        public int? CategoryId { get; set; }
        [Display(Name = "Брэнд")]
        public int? BrandId { get; set; }
        public List<int> Ids { get; set; }
    }
}
