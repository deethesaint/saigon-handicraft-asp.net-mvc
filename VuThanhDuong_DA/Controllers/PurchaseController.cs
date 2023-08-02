using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VuThanhDuong_DA.Models;

namespace VuThanhDuong_DA.Controllers
{
    public class PurchaseController : Controller
    {
        //
        // GET: /Purchase/

        public ActionResult Index(int id = 0, int deleteIndex = 0)
        {
            if (Session["currentUser"] != null)
            {
                using (var dbContext = new SHSDBDataContext())
                {
                    List<user_order> orders = dbContext.user_orders.Where(uo => uo.user_account_id == (Session["currentUser"] as user_account).user_account_id).OrderByDescending(uo => uo.order_time).ToList();
                    if (id != 0)
                    {
                        TempData["id"] = id;
                        TempData["details"] = 1;
                    }
                    return View(orders);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Details()
        {
            using (var dbContext = new SHSDBDataContext())
            {
                int id = (int)TempData["id"];
                List<user_order_product> order_products = dbContext.user_order_products.Where(op => op.user_order_id == id).ToList();
                return PartialView(order_products);
            }
        }

        public ActionResult ConfirmCancel()
        {
            return PartialView();
        }

        public ActionResult CancelOrder(int id)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                user_order deleteOrder = dbContext.user_orders.SingleOrDefault(uo => uo.user_order_id == id);
                List<user_order_product> refDeleteOrder = dbContext.user_order_products.Where(op => op.user_order_id == deleteOrder.user_order_id).ToList();
                dbContext.user_order_products.DeleteAllOnSubmit(refDeleteOrder);
                dbContext.SubmitChanges();
                dbContext.user_orders.DeleteOnSubmit(deleteOrder);
                dbContext.SubmitChanges();
            }
            return RedirectToAction("Index", "Purchase");
        }

    }
}
