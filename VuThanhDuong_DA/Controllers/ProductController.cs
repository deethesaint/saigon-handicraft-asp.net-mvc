using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VuThanhDuong_DA.Models;

namespace VuThanhDuong_DA.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Index()
        {
            return RedirectToAction("All", "Product");
        }

        public ActionResult All()
        {
            using (var dbContext = new SHSDBDataContext())
            {
                return View(dbContext.products.ToList());
            }
        }

        public ActionResult Category(int id = 0)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                return View(dbContext.products.Where(p => p.product_category_id == id).ToList());
            }
        }

        public ActionResult Details(int id = 0)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                product prd = dbContext.products.SingleOrDefault(p => p.product_id == id);
                ViewBag.images = prd.product_images.ToList();
                return View(prd);
            }
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(FormCollection c)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                return View(dbContext.products.Where(p => p.product_name.Contains(Request["searchtx"].ToString()) == true).ToList());
            }
        }
    }
}
