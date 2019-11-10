using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class OrderViewModel
    {
        [Required,DataType(DataType.PhoneNumber)]
        [Display(Prompt = "Телефонный номер")]
        public string Phone { get; set; }
        [Required]
        [Display(Prompt = "Аддресс для доставки")]
        public string Address { get; set; }
    }
}
