using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Finalproject.Models
{
    public class PateintViewModel
    {

        public int PatientId { get; set; }
        [Required(ErrorMessage = "Enter Your First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter your Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Enter Your Date Of Birth")]
        [DOBvalidate(ErrorMessage = "Invalid Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
        [Required(ErrorMessage = "Enter Your Gender")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Enter Your Address")]
        public string Address { get; set; }
        public int MemberId { get; set; }
    }
}