using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finalproject.Models;
using System.Web.Security;

namespace Finalproject.Controllers
{
    [DOBvalidate]
    
    public class AdminController : Controller
    {
        /*. Login
2. Dashboard
3. Change Password
4. Edit Profile
5. Add and Edit Drug Details
6. View and Delete Drugs
7. View Order Details*/
        // GET: Admin
        public ActionResult AdminHome()
        {
            DashboardModel model = new DashboardModel();
            using (ProjectEntities1 db=new ProjectEntities1())
            {
                var getdoc = db.MemberLogins; int Tdoc=0,Tpat=0, Tsup = 0;
                foreach (var item in getdoc)
                {
                    if (item.RoleId == 1) { Tdoc++; }
                   else if (item.RoleId == 2) { Tpat++; }
                    else if (item.RoleId == 3) { Tsup++; }
                }
                model.TotalDoctor = Tdoc; model.TotalSupplier = Tsup;
                model.TotalPateint = Tpat;
                var totalOr = db.PateintOrderDetails.ToList().Count;
                model.TotalOrder = totalOr;

                var getstatus = db.PateintOrderDetails;
                int Tdis = 0, TDel = 0, Treq = 0;
                foreach (var item in getstatus)
                {
                    if (item.OrderStatus == "Dispatched") { Tdis++; }
                    else if (item.OrderStatus == "Delievered") { TDel++; }
                    else if (item.OrderStatus == "Requested") { Treq++; }
                }
                model.Dispatched = Tdis; model.Delievered = TDel; model.Requested = Treq;
                int assgn = 0, nassgn = 0;
                var getstat = db.PateintOrderDetails.ToList();
                foreach(var item in getstat)
                {
                    if (item.OrderStatus == "Requested") { nassgn++; }
                    else { assgn++; }
                }model.Assigned = assgn; model.NotAssigned = nassgn;

            }
                return View(model);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostLogin(AdminLoginModel model)
        {
            using (ProjectEntities1 db=new ProjectEntities1())
            {
                var getdata = db.Admins.SingleOrDefault(a=>a.Email==model.Email);
                db.InsertAdmin(model.FirstName,model.LastName,model.Gender,model.Address
                    ,model.Email,model.Password);
                ViewBag.Message = "Inserted";
            }
                return View("Login");
        }

        public ActionResult Login2()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostLogin2(AdminLoginModel model)
        {
           
                using (ProjectEntities1 db = new ProjectEntities1())
            {
                string role = "Admin";

                var checkuser = (from a in db.Admins
                                 where a.Email == model.Email && a.Password == model.Password
                                 select new { a.Email,a.AdminId}).FirstOrDefault();

                if (checkuser != null)
                {
                    FormsAuthentication.SetAuthCookie(checkuser.Email, false);
                    var athicket = new FormsAuthenticationTicket(1, checkuser.Email, DateTime.Now, DateTime.Now.AddMinutes(10)
                        , false, role);
                    string encrypttxt = FormsAuthentication.Encrypt(athicket);
                    var athcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypttxt);
                    HttpContext.Response.Cookies.Add(athcookie);

                    Session["AdminId"] = checkuser.AdminId;
                    switch (role)
                    {
                        case "Pateint":
                            return RedirectToAction("PateintHome", "Pateint");

                        case "Doctor":
                            return RedirectToAction("DoctorHome", "Doctor");

                        case "Supplier":
                            return RedirectToAction("SupplierHome", "Supplier");

                        case "Admin":
                            return RedirectToAction("AdminHome", "Admin");
                        default:
                            break;
                    }
                }
                /*else if (model.Email == null) {
                    ViewBag.Message = "Enter Email";
                    return View("Login2");
                }
                else if (model.Password == null)
                {
                    ViewBag.Message = "Enter Password";
                    return View("Login2");
                }*/
                else if (model.Password == null&& model.Email == null)
                {
                    ViewBag.Message = "Fields Can't be left blank";
                    return View("Login2");
                }
                else
                {
                    ViewBag.Message = "Invalid Credentials";
                    return View("Login2");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    
        public ActionResult EditAdmin()
        {
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                AdminLoginModel model = new AdminLoginModel();
                var gdata = db.Admins.FirstOrDefault(a => a.AdminId == 2);
                model.FirstName = gdata.FirstName;
                model.LastName = gdata.LastName;
                model.Gender = gdata.Gender;
                model.Address = gdata.Address;
                model.Email = gdata.Email;
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult PostEditAdmin(AdminLoginModel model)
        {
            if (!ModelState.IsValid)
                return View("EditAdmin");
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                db.UpdateAdmin(model.FirstName,model.LastName,model.Gender,model.Address
                    ,model.Email);
                ViewBag.Message = "Updated";
                return View("EditAdmin");
            }
        }

        public ActionResult ViewDrugsAdmin()
        {
            DrugViewModel dm = new DrugViewModel();
            List<DrugViewModel> lst = new List<DrugViewModel>();
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                var getdata = db.Drugs.ToList().Where(a=>a.IsDeleted==false);


                foreach (var item in getdata)
                {
                    lst.Add(new DrugViewModel
                    {
                        DrugId = item.DrugId,
                        DrugName = item.DrugName,
                        ManufactureDate = Convert.ToDateTime(item.ManufactureDate),
                        ExpiredDate = Convert.ToDateTime(item.ExpiredDate),
                        UsedFor = item.UsedFor,

                        SideEffects = item.SideEffects,
                        TotalQuantityAvailable = item.TotalQuantityAvailable,
                        IsDeleted=item.IsDeleted


                    });
                }

                dm.lstDrug = lst;
                return View(dm);

            }

        }

        public ActionResult ViewOrder()
        {

            CommonData model = new CommonData();
            AdminOrderModel m = new AdminOrderModel();
            m.suplist = model.SupplierNName();
            List<AdminOrderModel> lst = new List<AdminOrderModel>();
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                var getdata = db.PateintOrderDetails.ToList();


                foreach (var item in getdata)
                {
                    string supname = null;
                    if (item.OrderStatus != "Requested")
                    {
                        int orderid = item.OrderId;
                        
                        var nname = (from d in db.DrugDeliveries
                                     join s in db.Suppliers on d.SupplierId
         equals s.SupplierId
                                     where d.OrderId == orderid
                                     select new { s.FirstName, s.LastName }).FirstOrDefault();
                        supname = nname.FirstName + " " + nname.LastName;
                    }
                    lst.Add(new AdminOrderModel
                    {
                        SupplierName=supname,
                        OrderId = item.OrderId,
                        PatientId = item.PateintId,
                        Drugname=item.Drug.DrugName,
                        PateintName=item.Patient.FirstName+ item.Patient.LastName,
                        DrugId = item.DrugId,
                        OrderNumber = item.OrderNumber,
                        Quantity = item.Quantity,
                        OrderedDate = Convert.ToDateTime(item.OrderedDate),
                        OrderStatus = item.OrderStatus
                        

                    });
                }
                //InboxModel inboxViewModel = new InboxModel();
                m.lstAdmin = lst;
                return View(m);
            }

        }
        [HttpPost]
        public ActionResult PostorderAdmin(AdminOrderModel model)
        {
            using (ProjectEntities1 db=new ProjectEntities1())
            {
                if (model.SupplierId > 0)
                {
                    string assign = "Assigned";
                    db.InsertDrugDelivery(model.OrderId, model.SupplierId);
                    db.UpdateAssign(model.OrderId,assign);
                    return Json("Assigned");
                }
                else { return Json("Error"); }
            }
                
        }
        public ActionResult AddDrugsAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult PostAddDrugsAdmin(DrugViewModel model)
        {
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                /* if (ModelState.IsValid)
                 {*/

                if (!ModelState.IsValid) { return View("AddDrugsAdmin"); }

                    Boolean isdeleted = false;
                    var getdata = db.Drugs.FirstOrDefault(a => a.DrugId == model.DrugId);
                    if (getdata != null)
                    {
                        db.UpadateDrug(model.DrugId, model.DrugName, model.ManufactureDate, model.ExpiredDate, model.UsedFor,
                            model.SideEffects, model.TotalQuantityAvailable, isdeleted);
                        ViewBag.Message = "Updated";
                    }
                    else
                    {

                        db.InsertDrug(model.DrugName, model.ManufactureDate, model.ExpiredDate, model.UsedFor,
                                model.SideEffects, model.TotalQuantityAvailable, isdeleted);
                        ViewBag.Message = "Inserted";
                    }
                    return View("AddDrugsAdmin");
                }
              /*  else
                {
                    return View("AddDrugsAdmin");
                }*/


             
        }
        
        public ActionResult AdminChangePass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostAdminPassword(PasswordChangeModel model)
        {
            if (!ModelState.IsValid)
                return View("AdminChangePass");
            int memid = Convert.ToInt32(Session["AdminId"]);
            using (ProjectEntities1 db = new ProjectEntities1())
            {

                var getdata = db.Admins.FirstOrDefault(a => a.AdminId == memid);
                if (model.OldPassword == getdata.Password)
                {
                    if (model.OldPassword == model.NewPassword) { ViewBag.message = "Enter New Password different from Old Password!"; }
                    else
                    {
                        db.UpdateAdminPassword(memid, model.NewPassword);
                        ViewBag.message = "Password Updated!";
                    }
                }
                
                else
                {
                    ViewBag.message = "Old password didn't match!";
                }
            }
            return View("AdminChangePass");
        }

            public ActionResult EditDrug(int id)
        {
            DrugViewModel model = new DrugViewModel();
            using (ProjectEntities1 db=new ProjectEntities1())
            {
                var getdata = db.Drugs.FirstOrDefault(a=>a.DrugId==id);
                if(getdata!=null)
                {
                    model.DrugId = getdata.DrugId;
                    model.DrugName = getdata.DrugName;
                    model.ManufactureDate = getdata.ManufactureDate;
                    model.ExpiredDate = getdata.ExpiredDate;model.UsedFor = getdata.UsedFor;
                    model.TotalQuantityAvailable = getdata.TotalQuantityAvailable;
                    model.SideEffects = getdata.SideEffects;
                    return View("AddDrugsAdmin",model);
                }
                else
                {
                    return View("ViewDrugsAdmin");
                }
            }
                
        }

            public ActionResult DeleteDrug(int id)
        {
            DrugViewModel model = new DrugViewModel();
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                var getdata = db.Drugs.FirstOrDefault(a=>a.DrugId==id);
                if(getdata!=null)
                {
                    db.Updateisdelete(id,true);
                    }
            }
                return RedirectToAction("ViewDrugsAdmin");
        }
    }
}