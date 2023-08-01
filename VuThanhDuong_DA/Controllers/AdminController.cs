using System;
using System.Collections.Generic;
using System.Linq;
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

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(admin_account aa)
        {
            return View();
        }

        public ActionResult CategoryList(string search_str = "")
        {
            using (var dbContext = new SHSDBDataContext())
            {
                return View(dbContext.product_categories.ToList());
            }
        }

        public ActionResult CategoryCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CategoryCreate(product_category pc)
        {
            return View();
        }

        public ActionResult CategoryEdit()
        {
            return View();
        }

        public ActionResult CategoryDelete()
        {
            return View();
        }

        public ActionResult ProductList()
        {
            return View();
        }

        public ActionResult ProductCreate()
        {
            return View();
        }

        public ActionResult ProductEdit()
        {
            return View();
        }

        public ActionResult ProductDelete()
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
