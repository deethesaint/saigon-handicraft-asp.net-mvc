using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VuThanhDuong_DA.Models;

namespace VuThanhDuong_DA.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/

        public ActionResult Index()
        {
            ViewBag.sum = GetCart().Sum(pc => (pc.Price - pc.Discount) * pc.Amount);
            ViewBag.amount = GetCart().Sum(pc => pc.Amount);
            return View(GetCart());
        }

        public ActionResult OrderList()
        {
            return View();
        }

        public List<InCartProduct> GetCart()
        {
            if (Session["cart"] == null)
                return new List<InCartProduct>();
            return Session["cart"] as List<InCartProduct>;
        }

        public ActionResult AddProductIntoCart(int id)
        {
            List<InCartProduct> products = GetCart();
            InCartProduct ICPrd = products.Find(prd => prd.Id == id);
            if (ICPrd == null)
            {
                products.Add(new InCartProduct(id));
            }
            else
            {
                ICPrd.Amount++;
            }
            Session["cart"] = products;
            return RedirectToAction("Details", "Product", new {ID = id});
        }

        [HttpPost]
        public ActionResult ChangeProductAmount(FormCollection c)
        {
            List<InCartProduct> products = GetCart();
            foreach(InCartProduct icp in products)
            {
                icp.Amount = int.Parse(Request[(icp.Id).ToString()]);
            }
            Session["cart"] = products;
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult DeleteProduct(int id)
        {
            List<InCartProduct> products = GetCart();
            products.RemoveAll(product => product.Id == id);
            Session["cart"] = products;
            return RedirectToAction("Index", "Cart");
        }

        public ActionResult RequestOrder()
        {
            ViewBag.sum = GetCart().Sum(pc => (pc.Price - pc.Discount) * pc.Amount);
            ViewBag.amount = GetCart().Sum(pc => pc.Amount);
            return View(GetCart());
        }

        [HttpPost]
        public ActionResult SubmitOrder(FormCollection c)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                if (GetCart().Count != 0)
                {
                    List<InCartProduct> products = GetCart();
                    user_order uo = new user_order();
                    if (Session["currentUser"] != null)
                    {
                        uo.user_account_id = (Session["currentUser"] as user_account).user_account_id;
                    }
                    uo.order_time = DateTime.Now;
                    uo.is_processed = false;
                    uo.is_delivered = false;
                    uo.user_order_buyer_name = Request["Order_owner_name"];
                    uo.user_order_address = Request["Order_owner_address"];
                    uo.user_order_email = Request["Order_owner_email"];
                    uo.user_order_phonenumber = Request["Order_owner_phone"];
                    uo.order_total_value = GetCart().Sum(pc => (pc.Price - pc.Discount) * pc.Amount);
                    dbContext.user_orders.InsertOnSubmit(uo);
                    dbContext.SubmitChanges();
                    foreach (InCartProduct product in products)
                    {
                        user_order_product uop = new user_order_product();
                        uop.product_id = product.Id;
                        uop.user_order_id = uo.user_order_id;
                        uop.product_name = product.Name;
                        uop.order_product_amount = product.Amount;
                        dbContext.user_order_products.InsertOnSubmit(uop);
                    }
                    dbContext.SubmitChanges();
                    Session.Remove("cart");
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
