using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Domain.Models
{
    public class LoginViewModel
    {
        [Required, Display(Prompt = "Имя пользователя")]
        public string UserName { get; set; }
        [Required, DataType(DataType.Password), Display(Prompt = "Пароль")]
        public string Password { get; set; }
        [Display(Name ="Запомнить меня на сайте")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
