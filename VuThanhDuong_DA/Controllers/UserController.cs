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
            TempData["LoginFailed"] = 1;
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
        public ActionResult RegisterHandler(user_account user)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                if (dbContext.user_accounts.Any(u => u.user_username == user.user_username))
                {
                    TempData["RegisterFailed"] = 1;
                    TempData["why"] = "Đã tồn tại tên đăng nhập. Vui lòng chọn tên đăng nhập khác!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    try
                    {
                        user.user_member_tier = "Đồng";
                        user.user_point = 0;
                        dbContext.user_accounts.InsertOnSubmit(user);
                        dbContext.SubmitChanges();
                        TempData["RegisterSucceed"] = 1;
                    }
                    catch (Exception e)
                    {
                        TempData["RegisterFailed"] = 1;
                        TempData["why"] = "Lỗi không xác định\nError Information: " + e.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LoginFailed()
        {
            return PartialView();
        }

        public ActionResult RegisterFailed()
        {
            return PartialView();
        }

        public ActionResult RegisterSuccessfully()
        {
            return PartialView();
        }

        public ActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(user_account ua)
        {
            return View();
        }
    }
}
