using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Domain.Models
{
    public class EmployeeView
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="Имя является обязательным полем.")]
        [DisplayName("Имя")]
        [StringLength(maximumLength:100,MinimumLength =2,ErrorMessage ="Имя должно содержать не менее 2-х символов и не более 100 символов")]
        public string FirstName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Фамилия является обязательным полем.")]
        [DisplayName("Фамилия")]
        [StringLength(maximumLength: 100, MinimumLength = 2, ErrorMessage = "Фамилия должна содержать не менее 2-х символов и не более 100 символов")]
        public string SurName { get; set; }
        [DisplayName("Отчество")]
        public string Patronymic { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Должность является обязательным полем.")]
        [DisplayName("Должность")]
        [StringLength(maximumLength: 100, MinimumLength = 2, ErrorMessage = "Должность должна не менее 2-х символов и не более 100 символов")]
        public string Post { get; set; }
        [DisplayName("Дата рождения")]
        public DateTime DateOfBirth { get; set; }
    }
}
