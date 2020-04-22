using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Finalproject.Controllers
{
    public class CustomErrorController : Controller
    {
        // GET: CustomError
       
            public ActionResult PageNotFound()
            {
                return View();
            }
            public ActionResult unauthorized()
            {
                return View();
            }
       
       
    }
}