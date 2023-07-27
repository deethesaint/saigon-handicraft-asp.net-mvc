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
            return View();
        }

        public ActionResult Login()
        {
            return View();
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
            return View();
        }

        public ActionResult CategoryCreate()
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
    }
}
