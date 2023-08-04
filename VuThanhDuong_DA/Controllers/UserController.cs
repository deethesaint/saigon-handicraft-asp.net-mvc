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
            if (Session["currentUser"] != null)
                return View(Session["currentUser"] as user_account);
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Edit(user_account ua)
        {
            if (Session["currentUser"] != null)
            {
                using (var dbContext = new SHSDBDataContext())
                {
                    user_account changedAccount = dbContext.user_accounts.SingleOrDefault(u => u.user_account_id == ua.user_account_id);
                    changedAccount.user_firstname = ua.user_firstname;
                    changedAccount.user_lastname = ua.user_lastname;
                    changedAccount.user_address = ua.user_address;
                    changedAccount.user_phonenumber = ua.user_phonenumber;
                    changedAccount.user_gender = ua.user_gender;
                    changedAccount.user_email = ua.user_email;
                    dbContext.SubmitChanges();
                    Session["currentUser"] = changedAccount;
                    ViewBag.editSucceed = "Thay đổi thông tin thành công!";
                }
            }
            return View(Session["currentUser"] as user_account);
        }

        public ActionResult ChangePassword()
        {
            if (Session["currentUser"] != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ChangePassword(FormCollection c)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                user_account ua = dbContext.user_accounts.SingleOrDefault(d => d.user_account_id == (Session["currentUser"] as user_account).user_account_id);
                if (Request["currentPassword"] != ua.user_password)
                {
                    ViewBag.wrong = "Mật khẩu hiện tại không chính xác!";
                    return ChangePassword();
                }
                else if (Request["newPassword"] != Request["rePassword"])
                {
                    ViewBag.wrong = "Nhập lại mật khẩu không khớp!";
                    return ChangePassword();
                }
                else
                {
                    ua.user_password = Request["newPassword"];
                    dbContext.SubmitChanges();
                    ViewBag.changeSucceed = "Thay đổi mật khẩu thành công!";
                }
            }
            return View();
        }
    }
}
