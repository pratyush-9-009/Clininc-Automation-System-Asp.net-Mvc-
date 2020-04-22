using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Finalproject.Models
{
    public class DashboardModel
    {
        public int TotalOrder { get; set; }
        public int Dispatched { get; set; }
        public int Delievered { get; set; }
        public int Requested { get; set; }

        public int TotalDoctor { get; set; }

        public int TotalPateint { get; set; }
        public int TotalSupplier { get; set; }
        public int Assigned { get; set; }
        public int NotAssigned { get; set; }

        public int TotalAppoint { get; set; }
        public int Tapprove{ get; set; }
        public int Trejected { get; set; }
        public int Trequested { get; set; }
        public int TInbox { get; set; }
        public int TRead { get; set; }
        public int Tunread { get; set; }

        //pateinthome
        public int Pat_app { get; set; }
        public int Pat_approv { get; set; }
        public int Pat_request { get; set; }
        public int Pat_rejected { get; set; }
        public int Pat_total { get; set; }
        public int Pat_Delivered { get; set; }
        public int Pat_Remain { get; set; }

        
    }
}