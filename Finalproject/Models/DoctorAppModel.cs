using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finalproject.Models;
using System.ComponentModel.DataAnnotations;

namespace Finalproject.Models
{
    public class DoctorAppModel
    {
        public int AppointmentId { get; set; }
        public Nullable<int> PatientId { get; set; }
        public Nullable<int> DoctorId { get; set; }
        [Required(ErrorMessage = "Enter Message Subject")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Enter Description")]
        public string Description { get; set; }
        public string PateintName { get; set; }
        [Required(ErrorMessage = "Enter Appointment Date")]
        [CustomValidate(ErrorMessage = "Invalid Date")]
        public Nullable<System.DateTime> AppointmentDate { get; set; }
        public string AppointmentStatus { get; set; }
        public string DoctorName { get; set; }
        public List<SelectListItem> doclst { get; set; }
        public List<DoctorAppModel> lstDoc { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Patient Patient { get; set; }
    }
}