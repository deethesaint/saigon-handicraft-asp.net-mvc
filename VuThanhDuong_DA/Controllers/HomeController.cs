using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace VuThanhDuong_DA.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Breadcrumb()
        {
            return PartialView();
        }

        public ActionResult Sidebar()
        {
            return PartialView();
        }

        public ActionResult PaginationHandler()
        {
            return View();
        }

    }
}
