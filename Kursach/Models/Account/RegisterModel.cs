using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        [StringLength(35, ErrorMessage = "Превышает 35 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        [StringLength(35, ErrorMessage = "Превышает 35 символов")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Не указана Фамилия")]
        [StringLength(35, ErrorMessage = "Превышает 35 символов")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Не указано Имя")]
        [StringLength(35, ErrorMessage = "Превышает 35 символов")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Не указано Отчество")]
        [StringLength(35, ErrorMessage = "Превышает 35 символов")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Не указаны паспортные данные")]
        [Range(1000000000, 9999999999, ErrorMessage = "Недопустимое значение")]
        public long PassportData { get; set; }

        [Required(ErrorMessage = "Не указан тип аккаунта")]
        public string AccountType { get; set; }
    }
}