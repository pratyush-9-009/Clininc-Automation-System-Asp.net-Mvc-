using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finalproject.Models;

namespace Finalproject.Controllers
{
    [MyCustomAuthorize(Roles = "Supplier")]
    public class SupplierController : Controller
    {
        // GET: Supplier
      
        public ActionResult SupplierHome()
        {
            int memid = Convert.ToInt32(Session["MemberId"]);
            DashboardModel model = new DashboardModel();   
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                var getmem = db.MemberLogins.FirstOrDefault(a=>a.MemberId==memid);
                if (getmem != null) { 
                var getdata = (from p in db.PateintOrderDetails join d in db.DrugDeliveries on p.OrderId
                               equals d.OrderId join s in db.Suppliers on d.SupplierId equals s.SupplierId
                               where s.MemberId==memid
                               select new
                               {p.OrderId,p.PateintId,p.OrderNumber,p.OrderStatus,p.OrderedDate,p.Quantity,
                               p.DrugId,d.DeliveredDate,
                                   M = p.Patient.FirstName+" "+ p.Patient.LastName,
                                   Drug_Name=p.Drug.DrugName
                                   }).ToList();
                // var nxtdata = getdata.SingleOrDefault(a=>a.MemberId==memid);
                 int count = getdata.Count;
                model.TotalOrder = count;
                int dis_count=0, deli_count=0, Req_count=0;
               foreach(var item in getdata)
                {
                    if (item.OrderStatus == "Dispatched")
                        dis_count++;
                    else if (item.OrderStatus == "Delievered")
                        deli_count++;
                    else if (item.OrderStatus == "Requested"|| item.OrderStatus == "Assigned")
                        Req_count++;
                }
                model.Dispatched = dis_count;model.Delievered = deli_count;model.Requested = Req_count;
                        return View(model);
                    }
                else
                {
                    model.Dispatched = 0; model.Delievered = 0; model.Requested = 0; model.TotalOrder=0;
                    return View(model);
                }
        }
            }

        public ActionResult EditSupplier()
        {
           
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                
                //PateintViewModel model = new PateintViewModel();
                int memid = Convert.ToInt32(Session["MemberId"]);
                var gdata = db.Suppliers.FirstOrDefault(a => a.MemberId == memid);
                MemberModel m = new MemberModel();
                SupplierViewModel model = new SupplierViewModel();

                if (gdata != null)
                {
                    model.FirstName = gdata.FirstName;
                    model.LastName = gdata.LastName;
                    model.CompanyName = gdata.CompanyName;
                    model.CompanyAddress = gdata.CompanyAddress;
                    
                    return View(model);
                }
                else
                {
                    //model.doctorlst = cmodel.DoctorSpecs();
                    return View("EditSupplier");
                }
            }

        }
        [HttpPost]
        public ActionResult PostEditSupplier(SupplierViewModel model)
        {
            if (!ModelState.IsValid)
                return View("EditSupplier");
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                int memid = Convert.ToInt32(Session["MemberId"]);
                var gdata = db.Suppliers.FirstOrDefault(a => a.MemberId == memid);
                MemberModel m = new MemberModel();
                if (gdata != null)
                {
                    db.UpdateSupplier(memid, model.FirstName, model.LastName, model.CompanyName
                        , model.CompanyAddress);
                    ViewBag.message = "Updated";
                    return View("EditSupplier");
                }
                else
                {
                    db.InsertSupplier(memid, model.FirstName, model.LastName, model.CompanyName
                        , model.CompanyAddress);
                    ViewBag.message = "Inserted";
                    return View("EditSupplier");
                }
            }
          
        }

        public ActionResult ChangePassSupplier()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostSupplierPass(PasswordChangeModel model)
        {
            if (!ModelState.IsValid)
                return View("ChangePassSupplier");
            MemberModel model1 = new MemberModel();
            int memid = Convert.ToInt32(Session["MemberId"]);
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                var getdata = db.MemberLogins.FirstOrDefault(a => a.MemberId == memid);
                if (model.OldPassword == getdata.Password)
                {
                    if (model.OldPassword == model.NewPassword) { ViewBag.message = "Enter New Password different from Old Password!"; }
                    else
                    {
                        db.UpdatePassword(memid, model.NewPassword);
                        ViewBag.message = "Password Updated!";
                    }
                }
                else
                {
                    ViewBag.message = "Old password do not Match";
                }
            }
            return View("ChangePassSupplier");
          
        }
        public ActionResult ViewOrderSup()
        {
            DashboardModel mm = new DashboardModel();
            int memid = Convert.ToInt32(Session["MemberId"]);
            CommonData model = new CommonData();
            AdminOrderModel m = new AdminOrderModel();
            m.suplist = model.SupplierNName();
            List<AdminOrderModel> lst = new List<AdminOrderModel>();
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                var getdata = (from p in db.PateintOrderDetails join d in db.DrugDeliveries on p.OrderId
                               equals d.OrderId join s in db.Suppliers on d.SupplierId equals s.SupplierId
                               where s.MemberId==memid
                               select new
                               {p.OrderId,p.PateintId,p.OrderNumber,p.OrderStatus,p.OrderedDate,p.Quantity,
                               p.DrugId,d.DeliveredDate,
                                   M = p.Patient.FirstName+" "+ p.Patient.LastName,
                                   Drug_Name=p.Drug.DrugName
                                   }).ToList();
                // var nxtdata = getdata.SingleOrDefault(a=>a.MemberId==memid);
                 int count = getdata.Count;
                mm.TotalOrder = count;
                
               
                foreach (var item in getdata)
                {
                    lst.Add(new AdminOrderModel
                    {
                        OrderId = item.OrderId,
                        PatientId = item.PateintId,
                       PateintName=item.M,
                       Drugname=item.Drug_Name,
                        DrugId = item.DrugId,
                        OrderNumber = item.OrderNumber,
                        Quantity = item.Quantity,
                        DelieveredDate=Convert.ToDateTime(item.DeliveredDate),
                        OrderedDate = item.OrderedDate,
                        OrderStatus = item.OrderStatus,
                        


                    });
                }
                //InboxModel inboxViewModel = new InboxModel();
                m.lstAdmin = lst;
                return View(m);
            }
        }
        [HttpPost]
        public ActionResult PostViewSup(AdminOrderModel model)
        {
            using (ProjectEntities1 db=new ProjectEntities1())
            {   //Delievered
                string status = model.OrderStatus;
                if(status== "Dispatched")
                {
                    
                    db.UpdateOrStatus(model.OrderId, model.OrderStatus);
                    return Json("Updated");
                }
                else
                {
                    db.UpdateOrStatus(model.OrderId,model.OrderStatus);
                    db.Updatedeleivery(model.OrderId,DateTime.Now);
                    //model.DelieveredDate = DateTime.Now;
                    return Json("Updated");
                }
                
            }
        }
    }
}