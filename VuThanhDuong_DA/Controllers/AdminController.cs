using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using VuThanhDuong_DA.Models;

namespace VuThanhDuong_DA.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }

        public ActionResult Sidebar()
        {
            return PartialView();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(admin_account ac)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                admin_account current_ac = dbContext.admin_accounts.SingleOrDefault(aci => aci.admin_username == ac.admin_username && aci.admin_password == ac.admin_password);
                if (current_ac == null)
                {
                    TempData["admin_failed"] = 1;
                    return View();
                }
                else
                {
                    Session["admin"] = current_ac;
                    return RedirectToAction("Index", "Admin");
                }
            }
        }

        public ActionResult LoginFailed()
        {
            return PartialView();
        }

        public ActionResult Logout()
        {
            Session.Remove("admin");
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(admin_account aa)
        {
            return View();
        }

        public ActionResult CategoryList()
        {
            using (var dbContext = new SHSDBDataContext())
            {
                List<product_category> categories = dbContext.product_categories.ToList();
                List<int> counts = new List<int>();
                foreach (product_category category in categories)
                {
                    counts.Add(category.products.Count);
                }
                ViewBag.countlist = counts;
                return View(categories);
            }
        }

        public ActionResult CategoryCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CategoryCreate(FormCollection c)
        {
            product_category category = new product_category();
            category.product_category_name = Request["category_name"];
            category.product_category_description = Request["category_description"];
            using (var dbContext = new SHSDBDataContext())
            {
                dbContext.product_categories.InsertOnSubmit(category);
                dbContext.SubmitChanges();
            }
            TempData["categoryCreateSucceed"] = 1;
            return View();
        }

        public ActionResult CategoryEdit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CategoryEdit(int id, FormCollection c)
        {
            return View();
        }

        [HttpPost]
        public ActionResult CategoryDelete(int id)
        {
            return View();
        }

        public ActionResult ProductList(int id = 0)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                List<product> products = dbContext.products.ToList();
                return View(products);
            }
        }

        public ActionResult ProductCreate()
        {
            using (var dbContext = new SHSDBDataContext())
            {
                List<product_category> categories = dbContext.product_categories.ToList();
                return View(categories);
            }
        }

        [HttpPost]
        public ActionResult ProductCreate(FormCollection c, HttpPostedFileBase product_thumbnail, HttpPostedFileBase img_1, HttpPostedFileBase img_2, HttpPostedFileBase img_3, HttpPostedFileBase img_4)
        {
            List<product_category> categories = new List<product_category>();
            using (var dbContext = new SHSDBDataContext())
            {
                product product = new product();
                product.product_name = Request["product_name"];
                product.product_category_id = int.Parse(Request["category_id"]);
                product.product_description = Request["product_description"];
                product.product_price = decimal.Parse(Request["product_price"]);
                product.product_discount = decimal.Parse(Request["product_discount"]);
                product.product_bought_count = 0;
                product.product_review_negative = 0;
                product.product_review_positive = 0;
                product.product_inventory = int.Parse(Request["product_inventory"]);
                if (product_thumbnail != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + Path.GetFileName(product_thumbnail.FileName);
                    string savePath = Path.Combine(Server.MapPath("~/Images/") + fileName);
                    product_thumbnail.SaveAs(savePath);
                    product.product_thumbnail_image = fileName;
                }
                dbContext.products.InsertOnSubmit(product);
                dbContext.SubmitChanges();
                if (img_1 != null)
                {
                    string fileName = product.product_id + "2" + Path.GetFileName(img_1.FileName);
                    string savePath = Path.Combine(Server.MapPath("~/Images/") + fileName);
                    img_1.SaveAs(savePath);
                    product_image image = new product_image();
                    image.product_id = product.product_id;
                    image.product_image_file_name = fileName;
                    dbContext.product_images.InsertOnSubmit(image);
                    dbContext.SubmitChanges();
                }
                if (img_2 != null)
                {
                    string fileName = product.product_id + "3" + Path.GetFileName(img_2.FileName);
                    string savePath = Path.Combine(Server.MapPath("~/Images/") + fileName);
                    img_2.SaveAs(savePath);
                    product_image image = new product_image();
                    image.product_id = product.product_id;
                    image.product_image_file_name = fileName;
                    dbContext.product_images.InsertOnSubmit(image);
                    dbContext.SubmitChanges();
                }
                if (img_3 != null)
                {
                    string fileName = product.product_id + "4" + Path.GetFileName(img_3.FileName);
                    string savePath = Path.Combine(Server.MapPath("~/Images/") + fileName);
                    img_3.SaveAs(savePath);
                    product_image image = new product_image();
                    image.product_id = product.product_id;
                    image.product_image_file_name = fileName;
                    dbContext.product_images.InsertOnSubmit(image);
                    dbContext.SubmitChanges();
                }
                if (img_4 != null)
                {
                    string fileName = product.product_id + "5" + Path.GetFileName(img_4.FileName);
                    string savePath = Path.Combine(Server.MapPath("~/Images/") + fileName);
                    img_4.SaveAs(savePath);
                    product_image image = new product_image();
                    image.product_id = product.product_id;
                    image.product_image_file_name = fileName;
                    dbContext.product_images.InsertOnSubmit(image);
                    dbContext.SubmitChanges();
                }
                categories = dbContext.product_categories.ToList();
            }
            TempData["productCreateSucceed"] = 1;
            return View(categories);
        }

        public ActionResult ProductEdit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProductEdit(int id, FormCollection c)
        {
            TempData["editSucceed"] = 1;
            return View();
        }

        [HttpPost]
        public ActionResult ProductDelete(int id)
        {
            return View();
        }

        public ActionResult UserList()
        {
            return View();
        }

        public ActionResult UserCreate()
        {
            return View();
        }

        public ActionResult UserEdit()
        {
            return View();
        }

        public ActionResult UserDelete()
        {
            return View();
        }

        public ActionResult OrderProcess()
        {
            return View();
        }

        public ActionResult OrderSucceed()
        {
            return View();
        }
    }
}
