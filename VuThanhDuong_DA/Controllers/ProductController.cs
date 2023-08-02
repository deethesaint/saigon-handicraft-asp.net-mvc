using MvcSiteMapProvider.Linq;
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

        public ActionResult All(int displayMode = 0)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                return View(dbContext.products.ToList());
            }
        }

        public ActionResult Category(int id = 0, int displayMode = 0)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                ViewBag.categoryTitle = dbContext.product_categories.SingleOrDefault(pc => pc.product_category_id == id).product_category_name;
                ViewBag.categoryDescription = dbContext.product_categories.SingleOrDefault(pc => pc.product_category_id == id).product_category_description;
                return View(dbContext.products.Where(p => p.product_category_id == id).ToList());
            }
        }

        public ActionResult Details(int id = 0)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                product prd = dbContext.products.SingleOrDefault(p => p.product_id == id);
                ViewBag.images = prd.product_images.ToList();
                ViewBag.category = prd.product_category.product_category_name;
                return View(prd);
            }
        }

        public ActionResult Search(int displayMode = 0)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(FormCollection c)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                ViewBag.search = Request["searchtx"];
                List<product> products = dbContext.products.Where(p => p.product_name.Contains(Request["searchtx"].ToString()) == true).ToList();
                ViewBag.searchAmount = products.Count;
                return View(products);
            }
        }
    }
}
