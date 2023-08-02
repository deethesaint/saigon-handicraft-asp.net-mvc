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
            return View();
        }

        public ActionResult New()
        {
            using (var dbContext = new SHSDBDataContext())
            {
                List<product> products = dbContext.products.OrderByDescending(p => p.product_id).ToList();
                return PartialView(products.GetRange(0, 12));
            }
        }

        public ActionResult MostPositive()
        {
            using (var dbContext = new SHSDBDataContext())
            {
                List<product> products = dbContext.products.OrderByDescending(p => p.product_review_positive).ToList();
                return PartialView(products.GetRange(0, 12));
            }
        }

        public ActionResult MostBought()
        {
            using (var dbContext = new SHSDBDataContext())
            {
                List<product> products = dbContext.products.OrderByDescending(p => p.product_bought_count).ToList();
                return PartialView(products.GetRange(0, 12));
            }
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
