using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entitys;

namespace WebStore.Areas.Admin.Models
{
    public class EditProductViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Наименование")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Порядок")]
        public int Order { get; set; }
        [Display(Name = "Изображение"),DataType(DataType.Upload)]
        public IFormFile ImageUrl { get; set; }
        [Required]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }
        [Required]
        [Display(Name = "Категория")]
        public  int CategoryId { get; set; }
        [Required]
        [Display(Name ="Брэнд")]
        public  int BrandId { get; set; }
    }
}
