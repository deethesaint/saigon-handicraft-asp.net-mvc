using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using MvcSiteMapProvider;
using VuThanhDuong_DA.Models;

namespace VuThanhDuong_DA.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return RedirectToAction("All", "Product");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Instruction()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Sidebar()
        {
            using (var dbContext = new SHSDBDataContext())
            {
                return PartialView(dbContext.product_categories.ToList());
            }
        }

        public ActionResult Dropdown()
        {
            using (var dbContext = new SHSDBDataContext())
            {
                return PartialView(dbContext.product_categories.ToList());
            }
        }

    }
}
