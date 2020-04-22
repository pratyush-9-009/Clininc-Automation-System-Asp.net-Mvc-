using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Finalproject.Models
{
    public class SupplierViewModel
    {
        public int SupplierId { get; set; }
        [Required(ErrorMessage = "Enter your First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter your Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Enter your Company Name")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Enter your Comapny Address")]
        public string CompanyAddress { get; set; }

        public int MemberId { get; set; }
    }
}