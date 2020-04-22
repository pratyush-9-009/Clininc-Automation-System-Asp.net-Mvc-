using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finalproject.Models;
using System.ComponentModel.DataAnnotations;
namespace Finalproject.Models
{
    public class DrugViewModel
    {
        public int ? DrugId { get; set; }
        [Required(ErrorMessage = "Enter Drug Name")]
        public string DrugName { get; set; }

        [Required(ErrorMessage = "Enter Manufactured Date")]
        [DOBvalidate(ErrorMessage = "Invalid Manufactured Date")]
        public Nullable<System.DateTime> ManufactureDate { get; set; }

        [Required(ErrorMessage = "Enter Expired Date")]
        [CustomValidate(ErrorMessage = "Invalid Expired Date")]
        public Nullable<System.DateTime> ExpiredDate { get; set; }

        [Required(ErrorMessage = "Enter It's Use")]
        public string UsedFor { get; set; }
        [Required(ErrorMessage = "Enter It's Side Effects")]
        public string SideEffects { get; set; }
        [Required(ErrorMessage = "Enter Total Quantity Available")]
        public Nullable<int> TotalQuantityAvailable { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public List<DrugViewModel> lstDrug { get; set; }
    }
}