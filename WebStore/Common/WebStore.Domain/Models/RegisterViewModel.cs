using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Domain.Models
{
    public class RegisterViewModel
    {
        [Required, Display(Prompt = "Имя пользователя"), MaxLength(256, ErrorMessage = "Максимальная длина 256 символов")]
        [RegularExpression(@"([A-Za-z][A-Za-z0-9_]{2,255})", ErrorMessage = "Неверный формат имени пользователя")]
        [Remote("IsNameFree", "Account")]
        public string UserName { get; set; }
        [Required,DataType(DataType.EmailAddress), Display(Prompt = "Адрес электронной почты")]
        public string Email { get; set; }
        [RegularExpression(@"(^\+\d{1,2})?((\(\d{3}\))|(\-?\d{3}\-)|(\d{3}))((\d{3}\-\d{4})|(\d{3}\-\d\d\  
-\d\d)|(\d{7})|(\d{3}\-\d\-\d{3}))", ErrorMessage ="Неверный формат номера телефона.")]
        [DataType(DataType.PhoneNumber), Display(Prompt = "Номер телефона")]
        public string PhoneNumber { get; set; }
        [Required,DataType(DataType.Password), Display(Prompt = "Пароль")]
        public string Password { get; set; }
        [Required, DataType(DataType.Password), Compare(nameof(Password)), Display(Prompt = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }
    }
}
