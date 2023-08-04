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
                List<product> products = new List<product>();
                switch (displayMode)
                {
                    case FilterMode.DEFAULT: products = dbContext.products.ToList(); break;
                    case FilterMode.ATOZ: products = dbContext.products.OrderBy(p => p.product_name).ToList(); break;
                    case FilterMode.ZTOA: products = dbContext.products.OrderByDescending(p => p.product_name).ToList(); break;
                    case FilterMode.ONETONINE: products = dbContext.products.OrderBy(p => p.product_price).ToList(); break;
                    case FilterMode.NINETOONE: products = dbContext.products.OrderByDescending(p => p.product_price).ToList(); break;
                }
                return View(products);
            }
        }

        public ActionResult Category(int id = 0, int displayMode = 0)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                ViewBag.id = id;
                ViewBag.categoryTitle = dbContext.product_categories.SingleOrDefault(pc => pc.product_category_id == id).product_category_name;
                ViewBag.categoryDescription = dbContext.product_categories.SingleOrDefault(pc => pc.product_category_id == id).product_category_description;
                List<product> products = new List<product>();
                switch (displayMode)
                {
                    case FilterMode.DEFAULT: products = dbContext.products.Where(p => p.product_category_id == id).ToList(); break;
                    case FilterMode.ATOZ: products = dbContext.products.Where(p => p.product_category_id == id).OrderBy(p => p.product_name).ToList(); break;
                    case FilterMode.ZTOA: products = dbContext.products.Where(p => p.product_category_id == id).OrderByDescending(p => p.product_name).ToList(); break;
                    case FilterMode.ONETONINE: products = dbContext.products.Where(p => p.product_category_id == id).OrderBy(p => p.product_price).ToList(); break;
                    case FilterMode.NINETOONE: products = dbContext.products.Where(p => p.product_category_id == id).OrderByDescending(p => p.product_price).ToList(); break;
                }
                return View(products);
            }
        }

        public ActionResult Search(FormCollection c, string searchTerm = "", int displayMode = 0)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                string s;
                if (Request["searchtx"] != null)
                    ViewBag.search = Request["searchtx"];
                else
                    ViewBag.search = searchTerm;
                s = ViewBag.search;
                List<product> products = dbContext.products.Where(p => p.product_name.Contains(s) == true).ToList();
                switch (displayMode)
                {
                    case FilterMode.DEFAULT: products = dbContext.products.Where(p => p.product_name.Contains(s) == true).ToList(); break;
                    case FilterMode.ATOZ: products = dbContext.products.Where(p => p.product_name.Contains(s) == true).OrderBy(p => p.product_name).ToList(); break;
                    case FilterMode.ZTOA: products = dbContext.products.Where(p => p.product_name.Contains(s) == true).OrderByDescending(p => p.product_name).ToList(); break;
                    case FilterMode.ONETONINE: products = dbContext.products.Where(p => p.product_name.Contains(s) == true).OrderBy(p => p.product_price).ToList(); break;
                    case FilterMode.NINETOONE: products = dbContext.products.Where(p => p.product_name.Contains(s) == true).OrderByDescending(p => p.product_price).ToList(); break;
                }
                ViewBag.searchAmount = products.Count;
                return View(products);
            }
        }

        public ActionResult Details(int id = 0)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                product prd = dbContext.products.SingleOrDefault(p => p.product_id == id);
                List<product_review> reviews = prd.product_reviews.OrderByDescending(r => r.product_review_id).ToList();
                ViewBag.reviews = reviews;
                ViewBag.images = prd.product_images.ToList();
                ViewBag.category = prd.product_category.product_category_name;
                List<product> otherProducts = dbContext.products.Where(p => p.product_id != prd.product_id && p.product_category_id == prd.product_category_id).ToList();
                if (otherProducts.Count <= 4)
                {
                    ViewBag.otherProducts = otherProducts.GetRange(0, otherProducts.Count);
                }
                else
                {
                    ViewBag.otherProducts = otherProducts.GetRange(0, 4);
                }
                return View(prd);
            }
        }

        [HttpPost]        
        public ActionResult PostReview(FormCollection c)
        {
            if (Session["currentUser"] != null)
            {
                user_account currentUser = Session["currentUser"] as user_account;
                using (var dbContext = new SHSDBDataContext())
                {
                    product_review review = new product_review();
                    review.review_owner = currentUser.user_lastname + " " + currentUser.user_firstname;
                    review.product_id = int.Parse(Request["product_id"]);
                    review.user_account_id = currentUser.user_account_id;
                    review.product_review_content = Request["review_content"];
                    dbContext.product_reviews.InsertOnSubmit(review);
                    dbContext.SubmitChanges();
                }
            }
            return RedirectToAction("Details", "Product", new { id = int.Parse(Request["product_id"]) });
        }
    }
}
