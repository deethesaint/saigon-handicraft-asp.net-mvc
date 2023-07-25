using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VuThanhDuong_DA.Models;

namespace VuThanhDuong_DA.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult LoginModal()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult LoginHandler(user_account user)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                user_account user_account = dbContext.user_accounts.FirstOrDefault(u => u.user_username == user.user_username && u.user_password == user.user_password);
                if (user_account != null)
                {
                    Session["currentUser"] = user_account;
                    return RedirectToAction("Index", "Home");
                }    
            }
            Session["failed"] = 1;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            Session.Remove("currentUser");
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOutModal()
        {
            return PartialView();
        }

        public ActionResult RegisterModal()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult RegisterHandler()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LoginFailed()
        {
            return PartialView();
        }
    }
}
