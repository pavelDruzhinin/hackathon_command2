using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GameShop.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public List<Order> Orders { get; set; }     
               
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] //пришлось изменить формат даты, чтобы браузер не затирал дату рождения на странице редактирования профиля
        public DateTime Birthday { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public List<GameComment> GameComments { get; set; }
    }
}