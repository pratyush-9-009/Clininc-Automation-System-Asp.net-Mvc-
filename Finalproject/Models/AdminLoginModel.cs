using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Finalproject.Models
{
    public class AdminLoginModel
    {
        public int AdminId { get; set; }
        [Required(ErrorMessage = "Enter your First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter your Last Name")]
        public string LastName { get; set; }

        public string Gender { get; set; }
        [Required(ErrorMessage = "Enter your Address")]
        public string Address { get; set; }
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid Email Format")]
        [Required(ErrorMessage = "Enter your Email")]
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual Admin Admin1 { get; set; }
        public virtual Admin Admin2 { get; set; }
    }
}