using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finalproject.Models;

namespace Finalproject.Controllers
{  
    [MyCustomAuthorize(Roles = "Pateint")]
    [CustomValidate]
    [DOBvalidate]
    public class PateintController : Controller
    {

        // GET: Pateint
        public ActionResult PateintHome()
        {
            DashboardModel model = new DashboardModel();
            int memid = Convert.ToInt32(Session["MemberId"]);
            using (ProjectEntities1 im = new ProjectEntities1())
            {
                //total order calculation
                int torder = 0, tdel = 0, nrem = 0;
                var gdata1 = im.Patients.FirstOrDefault(a => a.MemberId == memid);
                if (gdata1 != null)
                {
                    var gdata = im.PateintOrderDetails.ToList().Where(a => a.PateintId == gdata1.PatientId);
                    foreach (var item in gdata)
                    {
                        if (item.OrderStatus == "Delievered") { tdel++; }
                        else { nrem++; }
                        torder++;
                    }

                    model.Pat_total = torder; model.Pat_Delivered = tdel; model.Pat_Remain = nrem;
                    //total appointment
                    int tapp = 0, tapprov = 0, trej = 0, treq = 0;
                    var gapp = im.DoctorAppointments.ToList().Where(a => a.PatientId == gdata1.PatientId);
                    foreach (var item in gapp)
                    {
                        if (item.AppointmentStatus == "Approved") { tapprov++; }
                        else if (item.AppointmentStatus == "Requested") { treq++; }
                        else if (item.AppointmentStatus == "Rejected") { trej++; }

                        tapp++;
                    }
                    model.Pat_app = tapp; model.Pat_approv = tapprov; model.Pat_rejected = trej;
                    model.Pat_request = treq;
                }
                else
                {
                    model.Pat_total = 0; model.Pat_Delivered = 0; model.Pat_Remain = 0;
                    model.Pat_app = 0; model.Pat_approv =0; model.Pat_rejected =0;
                    model.Pat_request = 0;
                }
                    return View(model);
                
                
                
            }
        }

        public ActionResult EditPateint()
        {
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                PateintViewModel model = new PateintViewModel();
                int memid = Convert.ToInt32(Session["MemberId"]);
                var gdata = db.Patients.FirstOrDefault(a => a.MemberId == memid);
                MemberModel m = new MemberModel();
                if (gdata != null)
                {
                    model.FirstName = gdata.FirstName;
                    model.LastName = gdata.LastName;
                    model.DateOfBirth = gdata.DateOfBirth;
                    model.Gender = gdata.Gender;
                    model.Address = gdata.Address;
                    return View(model);
                }
                else
                {
                    return View("EditPateint");
                }
            }
        }
        [HttpPost]
        public ActionResult PostEditPateint(PateintViewModel model)
        {
            if (!ModelState.IsValid)
            {
                
                return View("EditPateint");
            }
            using (ProjectEntities1 db=new ProjectEntities1())
            {
                int memid = Convert.ToInt32(Session["MemberId"]);
                var gdata = db.Patients.FirstOrDefault(a=>a.MemberId==memid);
                MemberModel m = new MemberModel();
               if (gdata != null)
                {
                    db.UpdatePateint(memid,model.FirstName,model.LastName,model.DateOfBirth
                        ,model.Gender,model.Address);
                    ViewBag.message = " Profile Updated!";
                    return View("EditPateint");
                }
                else
                {
                    db.InsertPateint(memid, model.FirstName, model.LastName, model.DateOfBirth, 
                        model.Gender, model.Address);
                    ViewBag.message = "Your Profile Saved!";
                    return View("EditPateint");
               }
            }
                
        }

        public ActionResult PateintPass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostPateintPass(PasswordChangeModel model)
        {
            if (!ModelState.IsValid)
            {

                return View("PateintPass");
            }
            MemberModel model1 = new MemberModel();
            int memid = Convert.ToInt32(Session["MemberId"]);
            using (ProjectEntities1 db=new ProjectEntities1())
            {
                var getdata = db.MemberLogins.FirstOrDefault(a => a.MemberId == memid);
                if (model.OldPassword == getdata.Password)
                {
                    if (model.OldPassword == model.NewPassword) { ViewBag.message = "Enter New Password different from Old Password!"; }
                    else
                    {
                        db.UpdatePassword(memid, model.NewPassword);
                        ViewBag.message = "Password Updated !";
                    }
                }
                else
                {
                    ViewBag.message = "Old password do not Match !";
                }
            }
                return View("PateintPass");
        }

        public ActionResult SendMsgPateint()
        {
            CommonData model = new CommonData();
            InboxModel m = new InboxModel();
            m.doclst2 = model.DoctorApp();
            return View(m);
           
        }
        [HttpPost]
        public ActionResult PostSendMsgPateint(InboxModel inboxModel)
        {
            CommonData model = new CommonData();
            if (!ModelState.IsValid)
            {
                inboxModel.doclst2 = model.DoctorApp();
                return View("SendMsgPateint", model);
            }
            int memid = Convert.ToInt32(Session["MemberId"]);
            MemberModel memberModel = new MemberModel();
           
            inboxModel.doclst2 = model.DoctorApp();
            using (ProjectEntities1 db=new ProjectEntities1())
            {
                
                var getdata = db.Doctors.FirstOrDefault(a=>a.DoctorId==inboxModel.DoctorId);
                int mem = getdata.MemberId;
                
                var getdata2 = db.MemberLogins.FirstOrDefault(a => a.MemberId == mem);
                string ToEmail = getdata2.EmailId;

                /*var getdata3 = db.Patients.FirstOrDefault(a => a.MemberId == memid);
                int mem2 = getdata3.MemberId;*/
                var getdata4 = db.MemberLogins.FirstOrDefault(a => a.MemberId == memid);
                inboxModel.MessageDate = DateTime.Now;
                inboxModel.FromEmailId =getdata4.EmailId;
                int replyid = 0;
                string isRead = "false";
                db.InsertMessagePat(inboxModel.FromEmailId, ToEmail, inboxModel.Subject,inboxModel.MessageDetail,
                    inboxModel.MessageDate,replyid,isRead);
            }
            ViewBag.message = "Message Sent !";
            return View("SendMsgPateint",inboxModel);
        }

        public ActionResult ViewMessage()
        {
            using (ProjectEntities1 im = new ProjectEntities1())
            {
                DoctorModel docmodel = new DoctorModel();

                int memid = Convert.ToInt32(Session["MemberId"]);
                var gdata2 = im.MemberLogins.FirstOrDefault(a => a.MemberId == memid);
                //var getdoc = im.Doctors.FirstOrDefault(a=>a.);
                    
                var gdata = im.Inboxes.ToList().Where(a => a.ToEmailId == gdata2.EmailId);
                //gdata.FirstOrDefault
                List<InboxModel> lst = new List<InboxModel>();
                foreach (var item in gdata)
                {
                    string docmail = item.FromEmailId;
                    var getdoc = im.MemberLogins.FirstOrDefault(a=>a.EmailId == docmail);

                    var doc_get = im.Doctors.FirstOrDefault(a=>a.MemberId==getdoc.MemberId);
                    string docname = doc_get.FirstName +" "+ doc_get.LastName;
                    lst.Add(new InboxModel
                    {
                        DoctorName = docname ,
                        Subject = item.Subject,
                        MessageDetail = item.MessageDetail,
                        MessageDate = Convert.ToDateTime(item.MessageDate),
                        IsRead=item.IsRead
                        
                    });
                }
                InboxModel inboxViewModel = new InboxModel();
                inboxViewModel.doctorlist = lst;

                return View(inboxViewModel);
            }
           
        }
        public ActionResult ViewMessageSentPateint()
        {
            int memid = Convert.ToInt32(Session["MemberId"]);
            using (ProjectEntities1 im = new ProjectEntities1())
            {
                var gdata2 = im.MemberLogins.FirstOrDefault(a => a.MemberId == memid);

                var gdata = im.Inboxes.ToList().Where(a => a.FromEmailId == gdata2.EmailId);
                //var gdata3 = im.Inboxes.ToList().Where(a => a.ToEmailId == gdata2.EmailId);
                //int 
                //var getPateint=im.Patients.FirstOrDefault
                List<InboxModel> lst = new List<InboxModel>();
                foreach (var item in gdata)
                {
                    string docmail = item.ToEmailId;
                    var getdoc = im.MemberLogins.FirstOrDefault(a => a.EmailId == docmail);

                    var doc_get = im.Doctors.FirstOrDefault(a => a.MemberId == getdoc.MemberId);
                    string docname = doc_get.FirstName +" "+ doc_get.LastName;
                    lst.Add(new InboxModel
                    {
                        DoctorName=docname,
                        MessageId = item.MessageId,
                        Subject = item.Subject,
                        MessageDetail = item.MessageDetail,
                        MessageDate = item.MessageDate

                    });
                }
                InboxModel inboxViewModel = new InboxModel();
                inboxViewModel.doctorlist = lst;

                return View(inboxViewModel);
            }

        }



        public ActionResult DoctorAppointment()
        {
            CommonData model = new CommonData();
            DoctorAppModel m = new DoctorAppModel();
            m.doclst = model.DoctorApp();
            return View(m);
        }
        [HttpPost]
        public ActionResult PostDoctorAppointment(DoctorAppModel da)
        {
            CommonData model = new CommonData();
            if (!ModelState.IsValid)
            {
                da.doclst = model.DoctorApp();
                return View("DoctorAppointment", da);
            }
            int memid = Convert.ToInt32(Session["MemberId"]);
            using (ProjectEntities1 im = new ProjectEntities1())
            {
                var getdata = im.Patients.FirstOrDefault(a => a.MemberId == memid);

                da.AppointmentStatus = "Requested";
                int id = Convert.ToInt32(getdata.PatientId);
                im.InsertDoctorApp(id,da.DoctorId,da.Subject,da.Description,da.AppointmentDate,da.AppointmentStatus);
            }
            CommonData mcm = new CommonData();

            da.doclst = mcm.DoctorApp();
            ViewBag.message = "Appointment Requested !";
            return View("DoctorAppointment", da);
        }

       
        public ActionResult ViewAppointment()
        {
            int memid = Convert.ToInt32(Session["MemberId"]);
            using (ProjectEntities1 im = new ProjectEntities1())
            {
                var pdata = im.Patients.FirstOrDefault(a=>a.MemberId==memid);
                var gdata = im.DoctorAppointments.ToList().Where(a=>a.PatientId==pdata.PatientId);
                List<DoctorAppModel> lst = new List<DoctorAppModel>();
                foreach (var item in gdata)
                {


                    lst.Add(new DoctorAppModel
                    {
                        DoctorName = item.Doctor.FirstName+" "+item.Doctor.LastName,
                        Subject = item.Subject,
                        Description = item.Description,
                        AppointmentDate = item.AppointmentDate,
                        AppointmentStatus=item.AppointmentStatus
                    });
                }
                DoctorAppModel docViewModel = new DoctorAppModel();
                docViewModel.lstDoc = lst;

                return View(docViewModel);
            }
         
        }

        
        public ActionResult RaiseOrder()
        {
            CommonData model = new CommonData();

            PateintOrder m = new PateintOrder();
            m.drglst = model.DruGdetail();

            return View(m);
           
        }
        [HttpPost]
        public ActionResult PostRaiseOrder(PateintOrder m)
        {
            if (!ModelState.IsValid)
            {
                CommonData mc = new CommonData();
                m.drglst = mc.DruGdetail();
                return View("RaiseOrder",m);
            }
            int memid = Convert.ToInt32(Session["MemberId"]);
            using (ProjectEntities1 im = new ProjectEntities1())
            {
                var getdata = im.Patients.FirstOrDefault(a=>a.MemberId==memid);
                m.OrderedDate = DateTime.Now;
                m.OrderStatus = "Requested";
                string year = (DateTime.Now.Year).ToString();
                string month = (DateTime.Now.Month).ToString();
                string day = (DateTime.Now.Day).ToString();
                string hour = (DateTime.Now.Hour).ToString();
                string min= (DateTime.Now.Minute).ToString();
                string mili= (DateTime.Now.Second).ToString();
                m.OrderNumber = year + month + day + hour+min+mili;
                int id = getdata.PatientId;
                im.InsertPateintOrder(id,m.DrugId,m.OrderNumber,m.Quantity,m.OrderedDate,m.OrderStatus);

            }
            CommonData mcm = new CommonData();
            
            m.drglst = mcm.DruGdetail();
            ViewBag.message = "Medicines Requested";
            return View("RaiseOrder", m);
            
        }
        public ActionResult ViewOrder()
        {
            int memid = Convert.ToInt32(Session["MemberId"]);
            using (ProjectEntities1 im = new ProjectEntities1())
            {
                var gdata1 = im.Patients.FirstOrDefault(a=>a.MemberId==memid);
                var gdata = im.PateintOrderDetails.ToList().Where(a=>a.PateintId==gdata1.PatientId);
                List<PateintOrder> lst = new List<PateintOrder>();
                foreach (var item in gdata)
                {


                    lst.Add(new PateintOrder
                    {
                        DrugName=item.Drug.DrugName, Quantity=item.Quantity,OrderStatus=item.OrderStatus,
                        OrderedDate=item.OrderedDate
                        
                    }

                        );
                }
                PateintOrder pateintViewModel = new PateintOrder();
                pateintViewModel.lstPateint = lst;

                return View(pateintViewModel);
            }
        
        }

    }
}