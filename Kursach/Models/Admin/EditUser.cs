﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Kursach.Models.Admin
{
    public class EditUser
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Не указана роль")]
        [Range(1, 3, ErrorMessage = "Недопустимое значение")]
        public int RoleId { get; set; }

        [Required(ErrorMessage = "Не указан логин")]
        [StringLength(35, ErrorMessage = "Превышает 35 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(35, ErrorMessage = "Превышает 35 символов")]
        public string Password { get; set; }

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
        public int PassportData { get; set; }


        public int? MemberTicketId { get; set; }

        [Remote(action: "CheckDate", controller: "Admin", ErrorMessage = "Недопустимая дата")]
        public DateTime? ConclusionDate { get; set; }
        public SelectList? MemberTickets { get; set; }

    }
}
