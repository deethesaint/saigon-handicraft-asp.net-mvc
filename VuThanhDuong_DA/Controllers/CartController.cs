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
            return View(GetCart());
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
    }
}
