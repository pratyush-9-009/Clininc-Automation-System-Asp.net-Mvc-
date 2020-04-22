using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finalproject.Models;
using System.ComponentModel.DataAnnotations;

namespace Finalproject.Models
{
    public class MemberModel
    {
        public int MemberId { get; set; }
        [Required(ErrorMessage = "Enter your Email")]
        [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$", ErrorMessage = "Invalid Email Format")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Password Required")]
        [RegularExpression("^(?=.*\\d).{4,8}$", ErrorMessage = "Password must be between 4 and 8 digits and includes at least one digit.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Enter Your Specialization")]
        public int RoleId { get; set; }
        public List<SelectListItem> lstnew { get; set; }
    }
}