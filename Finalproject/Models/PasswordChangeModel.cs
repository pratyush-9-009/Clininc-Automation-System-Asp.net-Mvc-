using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Finalproject.Models
{
    public class PasswordChangeModel
    {
        [Required(ErrorMessage ="Enter Old Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Enter New Password")]
        [RegularExpression("^(?=.*\\d).{4,8}$",ErrorMessage = "Password must be between 4 and 8 digits and includes at least one digit.")]
        public string NewPassword { get; set; }


        [Compare("NewPassword",ErrorMessage ="Password didn't match")]
        [Required(ErrorMessage = "Enter Confirm Password")]
        public string ConfirmPassword { get; set; }

    }
}