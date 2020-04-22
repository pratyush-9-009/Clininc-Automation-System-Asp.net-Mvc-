using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finalproject.Models;

namespace Finalproject.Models
{
    public class AdminOrderModel
    {
        public int OrderId { get; set; }
        public Nullable<int> PatientId { get; set; }
        public Nullable<int> DrugId { get; set; }
        public string OrderNumber { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<System.DateTime> OrderedDate { get; set; }
        public DateTime ? DelieveredDate { get; set; }
        public string OrderStatus { get; set; }
        public int ? SupplierId { get; set; }
        public string Drugname { get; set; }

        public string PateintName { get; set; }
        public string SupplierName { get; set; }
        public List<SelectListItem> suplist { get; set; }
        public List<AdminOrderModel> lstAdmin { get; set; }

        public int TotalOrder { get; set; }

        public virtual Drug Drug { get; set; }

        public virtual Patient Patient { get; set; }
    }
}