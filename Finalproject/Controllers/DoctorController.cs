using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finalproject.Models;

namespace Finalproject.Controllers
{
    [MyCustomAuthorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        // GET: Doctor
        public ActionResult DoctorHome()
        {
            int memid = Convert.ToInt32(Session["MemberId"]);
            DashboardModel model = new DashboardModel();
            using (ProjectEntities1 im = new ProjectEntities1())
            {
                var ndata = im.Doctors.FirstOrDefault(a => a.MemberId == memid);
                if (ndata != null)
                {
                    var gdata = im.DoctorAppointments.ToList().Where(a => a.DoctorId == ndata.DoctorId);
                    int appCount = 0, tap = 0, trej = 0, treq = 0;
                    foreach (var item in gdata)
                    {
                        if (item.AppointmentStatus == "Approved") { tap++; }
                        else if (item.AppointmentStatus == "Rejected") { trej++; }
                        else if (item.AppointmentStatus == "Requested") { treq++; }
                        appCount++;
                    }
                    model.TotalAppoint = appCount; model.Tapprove = tap; model.Trejected = trej; model.Trequested = treq;

                    // inbox messages count
                    int tbox = 0; int read = 0, uread = 0;
                    var gdata2 = im.MemberLogins.FirstOrDefault(a => a.MemberId == memid);
                    var ginbox = im.Inboxes.ToList().Where(a => a.ToEmailId == gdata2.EmailId);
                    foreach (var item in ginbox)
                    {
                        if (item.IsRead == "True") { read++; }
                        else { uread++; }
                        tbox++;
                    }
                    model.TInbox = tbox; model.TRead = read; model.Tunread = uread;
                }
                else
                {
                    model.TInbox = 0; model.TRead = 0; model.Tunread = 0;
                    model.TotalAppoint = 0; model.Tapprove = 0; model.Trejected = 0; model.Trequested = 0;
                }
                //is read and is unread
            }  
                
            
            return View(model);
        }
        public ActionResult DoctorEdit()
        {
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                //PateintViewModel model = new PateintViewModel();
                int memid = Convert.ToInt32(Session["MemberId"]);
                var gdata = db.Doctors.FirstOrDefault(a => a.MemberId == memid);
                MemberModel m = new MemberModel();
                CommonData cmodel = new CommonData();
                DoctorModel model = new DoctorModel();
               

                if (gdata != null)
                {
                    model.FirstName = gdata.FirstName;
                    model.LastName = gdata.LastName;
                    model.TotalExperience = gdata.TotalExperience;
                    model.SpecializedId = gdata.SpecializedId;
                    model.Gender = gdata.Gender;
                    model.doctorlst = cmodel.DoctorSpecs();
                    return View(model);
                }
                else
                {
                    model.doctorlst = cmodel.DoctorSpecs();
                    return View(model);
                }
            }
            
            //return View(dm);
        }
        [HttpPost]
        public ActionResult PostDoctorEdit(DoctorModel model)
        {
            CommonData cmodel = new CommonData();
            
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                
                int memid = Convert.ToInt32(Session["MemberId"]);
                var gdata = db.Doctors.FirstOrDefault(a => a.MemberId == memid);
                model.doctorlst = cmodel.DoctorSpecs();
                MemberModel m = new MemberModel();
                if (gdata != null)
                {
                    db.UpdateDoctor(memid, model.FirstName, model.LastName, model.TotalExperience
                       , model.SpecializedId, model.Gender);

                    
                    ViewBag.message = "Profile Updated !";
                    return View("DoctorEdit",model);
                }
                else
                {
                    
                        db.InsertDoctor(memid, model.FirstName, model.LastName, model.TotalExperience
                        , model.SpecializedId, model.Gender);
                        
                        ViewBag.message = "Profile Saved !";
                        return View("DoctorEdit", model);
                  /*  }
                    else
                    {
                        return View("DoctorEdit", model);
                    }*/
                }
            }
         
        }

        public ActionResult changePassword()
        {

            return View();
        }
        [HttpPost]
        public ActionResult PostDocPassword(PasswordChangeModel model)
        {
            if (!ModelState.IsValid)
                return View("changePassword");
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
                        ViewBag.message = "Password Updated";
                    }
                }
                else
                {
                    ViewBag.message = "Old password do not Match !";
                }
            }
            return View("changePassword");

        }

        public ActionResult ViewMessageDoc()
        {
            int memid = Convert.ToInt32(Session["MemberId"]);
            using (ProjectEntities1 im = new ProjectEntities1())
            {
                var gdata2 = im.MemberLogins.FirstOrDefault(a=>a.MemberId==memid);

                var gdata = im.Inboxes.ToList().Where(a=>a.ToEmailId==gdata2.EmailId);

               // var member = im.MemberLogins.ToList(); var inb = im.Inboxes.ToList();

                List<InboxModel> lst = new List<InboxModel>();
                foreach (var item in gdata)
                {
                    string fromEmail = item.FromEmailId;
                    var getemail = im.MemberLogins.FirstOrDefault(a => a.EmailId == fromEmail);
                    var getpateint = im.Patients.FirstOrDefault(a => a.MemberId == getemail.MemberId);
                    string Pateint_Name = getpateint.FirstName + getpateint.LastName;
                    lst.Add(new InboxModel
                    {
                        MessageId=item.MessageId,
                        PateintName=Pateint_Name,
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
        public ActionResult ViewMessageSent()
        {
            int memid = Convert.ToInt32(Session["MemberId"]);
            using (ProjectEntities1 im = new ProjectEntities1())
            {
                var gdata2 = im.MemberLogins.FirstOrDefault(a => a.MemberId == memid);

                var gdata = im.Inboxes.ToList().Where(a => a.FromEmailId == gdata2.EmailId);
                //int 
                //var getPateint=im.Patients.FirstOrDefault
                List<InboxModel> lst = new List<InboxModel>();
                foreach (var item in gdata)
                {
                    string fromEmail = item.ToEmailId;
                    var getemail = im.MemberLogins.FirstOrDefault(a=>a.EmailId==fromEmail);
                    var getpateint = im.Patients.FirstOrDefault(a => a.MemberId == getemail.MemberId);
                    string Pateint_Name = getpateint.FirstName + getpateint.LastName;
                    lst.Add(new InboxModel
                    {
                        MessageId = item.MessageId,
                        PateintName=Pateint_Name,
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
        public ActionResult PostViewMessageDoc(InboxModel model)
        {
            using (ProjectEntities1 im = new ProjectEntities1())
            {
                int memid = Convert.ToInt32(Session["MemberId"]);
                var gdata = im.MemberLogins.FirstOrDefault(a => a.MemberId == memid);
                string FromEmail = gdata.EmailId;
                var gdata2 = im.Inboxes.FirstOrDefault(a=>a.MessageId==model.MessageId);

                string ToEmail = gdata2.FromEmailId;
                string IsRead = "True";
                im.UpdateIsRead(gdata2.MessageId,IsRead);
                im.InsertMessagePat(FromEmail,ToEmail,model.Subject,model.MessageDetail,DateTime.Now,model.MessageId,IsRead);
                
                return Json("Inserted");
            }
        }


        public ActionResult ViewAppointment()
        {
            int memid = Convert.ToInt32(Session["MemberId"]);
            DoctorAppModel doctorAppModel = new DoctorAppModel();
            using (ProjectEntities1 im = new ProjectEntities1())
            {
                var ndata = im.Doctors.FirstOrDefault(a => a.MemberId == memid);
                
                var gdata = im.DoctorAppointments.ToList().Where(a=>a.DoctorId==ndata.DoctorId);

                List<DoctorAppModel> lst = new List<DoctorAppModel>();
                foreach (var item in gdata)
                {
                    

                    lst.Add(new DoctorAppModel
                    {
                        AppointmentId=item.AppointmentId,
                        PatientId=item.PatientId,
                        PateintName = item.Patient.FirstName+" "+item.Patient.LastName,
                        DoctorId=item.DoctorId,
                        Subject = item.Subject,
                        Description = item.Description,
                        AppointmentDate = item.AppointmentDate,
                        AppointmentStatus = item.AppointmentStatus
                    });
                }

                doctorAppModel.lstDoc = lst;

                return View(doctorAppModel);
            }
           
        }

        [HttpPost]
        public ActionResult PostViewAppointment(DoctorAppModel doctorAppModel)
        {
            using (ProjectEntities1 db=new ProjectEntities1())
            {
                db.UpdateAppStatus(doctorAppModel.AppointmentId, doctorAppModel.AppointmentStatus);
            }
                return Json("Inserted");
        }

    }
}