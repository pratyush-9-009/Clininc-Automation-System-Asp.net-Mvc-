using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Finalproject.Models;
using System.Web.Security;

namespace Finalproject.Controllers
{   
    
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        {

            return View();

        }
        [HttpPost]
        public ActionResult PostLogin(MemberModel model)
        {
            using (ProjectEntities1 db = new ProjectEntities1())
            {
                var checkuser = (from a in db.MemberLogins
                                 join r in db.RoleDetails on a.RoleId equals r.RoleId
                                 where a.EmailId == model.EmailId && a.Password==model.Password
                                 select new { a.MemberId,a.EmailId, r.RoleName }).FirstOrDefault();

                if (checkuser != null)
                {
                    FormsAuthentication.SetAuthCookie(checkuser.EmailId, false);
                    var athicket = new FormsAuthenticationTicket(1, checkuser.EmailId, DateTime.Now, DateTime.Now.AddMinutes(5)
                        , false, checkuser.RoleName);
                    string encrypttxt = FormsAuthentication.Encrypt(athicket);
                    var athcookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypttxt);
                    HttpContext.Response.Cookies.Add(athcookie);

                    Session["MemberId"] = checkuser.MemberId;
                    switch (checkuser.RoleName)
                    {
                        case "Pateint":
                            return RedirectToAction("PateintHome", "Pateint");
                        
                        case "Doctor":
                            return RedirectToAction("DoctorHome", "Doctor");

                        case "Supplier":
                            return RedirectToAction("SupplierHome", "Supplier");
                        default:
                            break;
                    }
                }
                else 
                {
                    ViewBag.Message = "Invalid credentials!";

                }
            }
            return View("Login");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Register()
        {

            CommonData model = new CommonData();

            MemberModel m = new MemberModel();
            m.lstnew = model.RoleNName();

            return View(m);
        }
        [HttpPost]
        public ActionResult PostRegister(MemberModel model)
        {
            MemberModel m = new MemberModel();
            if (!ModelState.IsValid)
            {
                CommonData mc = new CommonData(); m.lstnew = mc.RoleNName();
                return View("Register",m);
            }
            using (ProjectEntities1 im = new ProjectEntities1())
            {
                var gdata = im.MemberLogins.FirstOrDefault(a=>a.EmailId==model.EmailId);
                if (gdata != null)
                {
                    ViewBag.Message = "Email Id Already used! ";
                }
                else
                {
                    im.InsertMember(model.EmailId, model.Password, model.RoleId);
                    ViewBag.Message = "Registered Succesfully!";
                }
            }
            CommonData mcm = new CommonData();
           
            m.lstnew = mcm.RoleNName();
           
            return View("Register", m);
        }
    }
    }
