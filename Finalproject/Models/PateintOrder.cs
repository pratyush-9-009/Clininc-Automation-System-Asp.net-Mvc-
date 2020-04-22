using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finalproject.Models;
using System.ComponentModel.DataAnnotations;

namespace Finalproject.Models
{
    public class PateintOrder
    {
        public int OrderId { get; set; }
        public Nullable<int> PatientId { get; set; }
        [Required(ErrorMessage = "Enter The Drug")]
        public Nullable<int> DrugId { get; set; }
        public string OrderNumber { get; set; }
        [Required(ErrorMessage = "Enter Quantity")]
        public Nullable<int> Quantity { get; set; }
        public Nullable<System.DateTime> OrderedDate { get; set; }
        public string OrderStatus { get; set; }
        public string DrugName { get; set; }
        public List<SelectListItem> drglst { get; set; }
        public List<PateintOrder> lstPateint { get; set; }
        public virtual Drug Drug { get; set; }

        public virtual Patient Patient { get; set; }
    }
}