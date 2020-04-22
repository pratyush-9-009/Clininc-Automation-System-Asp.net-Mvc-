using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finalproject.Models;
using System.ComponentModel.DataAnnotations;
namespace Finalproject.Models
{
    public class InboxModel
    {
        public int MessageId { get; set; }
        public string FromEmailId { get; set; }
        public string ToEmailId { get; set; }
        [Required(ErrorMessage = "Enter the Subject")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "Enter Message Detail")]
        public string MessageDetail { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string PateintName { get; set; }
        public List<SelectListItem> doclst2 { get; set; }
        public List<InboxModel> doctorlist { get; set; }
        public Nullable<System.DateTime> MessageDate { get; set; }
        public int ReplyId { get; set; }
        public string IsRead { get; set; }
    }
}