using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Domain.Models
{
    public class RegisterViewModel
    {
        [Required, Display(Prompt = "Имя пользователя")]
        public string UserName { get; set; }
        [Required,DataType(DataType.EmailAddress), Display(Prompt = "Адрес электронной почты")]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber), Display(Prompt = "Номер телефона")]
        public string PhoneNumber { get; set; }
        [Required,DataType(DataType.Password), Display(Prompt = "Пароль")]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare(nameof(Password)), Display(Prompt = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }
    }
}
