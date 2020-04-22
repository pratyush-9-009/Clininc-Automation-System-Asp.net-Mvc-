using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finalproject.Models;
using System.ComponentModel.DataAnnotations;

namespace Finalproject.Models
{
    public class DoctorModel
    {
        public int ? DoctorId { get; set; }
        [Required(ErrorMessage = "Enter your First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter your Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Enter your Total Experience")]
        public Nullable<int> TotalExperience { get; set; }
        public int SpecializedId { get; set; }
        [Required(ErrorMessage = "Enter your Specialization")]
        public string Specialization { get; set; }
        [Required(ErrorMessage = "Enter your First Name")]
        public string Gender { get; set; }
        public List<SelectListItem> doctorlst { get; set; }
        public int MemberId { get; set; }

    }
}