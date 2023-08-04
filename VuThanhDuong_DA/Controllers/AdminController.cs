using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            //using (var dbContext = new SHSDBDataContext())
            //    Session["admin"] = dbContext.admin_accounts.SingleOrDefault(aci => aci.admin_username == "admin_test" && aci.admin_password == "09122002");
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

        public ActionResult CategoryEdit(int id)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                return View(dbContext.product_categories.SingleOrDefault(c => c.product_category_id == id));
            }
        }

        [HttpPost]
        public ActionResult CategoryEdit(FormCollection c)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                product_category pc = dbContext.product_categories.SingleOrDefault(pct => pct.product_category_id == int.Parse(Request["category_id"]));
                pc.product_category_name = Request["category_name"];
                pc.product_category_description = Request["category_description"];
                dbContext.SubmitChanges();
                TempData["categoryEditSucceed"] = 1;
                return View(pc);
            }
        }

        public ActionResult CategoryDelete(int id)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                product_category pc = dbContext.product_categories.SingleOrDefault(pct => pct.product_category_id == id);
                List<product> products = pc.products.ToList();
                foreach (product item in products)
                {
                    List<product_image> images = item.product_images.ToList();
                    foreach (product_image image in images)
                    {
                        string fileName = image.product_image_file_name;
                        string savePath = Path.Combine(Server.MapPath("~/Images/") + fileName);
                        FileInfo f = new FileInfo(savePath);
                        if (f.Exists)
                        {
                            f.Delete();
                        }
                    }
                    dbContext.product_images.DeleteAllOnSubmit(images);
                    dbContext.SubmitChanges();
                }
                dbContext.products.DeleteAllOnSubmit(products);
                dbContext.SubmitChanges();
                dbContext.product_categories.DeleteOnSubmit(pc);
                dbContext.SubmitChanges();
            }
            return RedirectToAction("CategoryList", "Admin");
        }

        public ActionResult ProductList(int id = 0)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                List<product> products = new List<product>();
                if (id == 0)
                {
                    products = dbContext.products.ToList();
                }
                else
                {
                    products = dbContext.products.Where(p => p.product_category_id == id).ToList();
                }
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

        public ActionResult ProductEdit(int id)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                product prd = dbContext.products.SingleOrDefault(p => p.product_id == id);
                ViewBag.categories = dbContext.product_categories.ToList();
                ViewBag.images = prd.product_images.ToList();
                return View(prd);
            }
        }

        [HttpPost]
        public ActionResult ProductEdit(product prd, FormCollection c, HttpPostedFileBase product_thumbnail, HttpPostedFileBase img_1, HttpPostedFileBase img_2, HttpPostedFileBase img_3, HttpPostedFileBase img_4)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                product product = dbContext.products.SingleOrDefault(p => p.product_id == prd.product_id);
                product.product_name = prd.product_name;
                product.product_category_id = int.Parse(Request["category_id"]);
                product.product_description = prd.product_description;
                product.product_price = prd.product_price;
                product.product_discount = prd.product_discount;
                product.product_inventory = prd.product_inventory;
                List<product_image> images = product.product_images.ToList();
                if (product_thumbnail != null)
                {
                    string cfileName = product.product_thumbnail_image;
                    string csavePath = Path.Combine(Server.MapPath("~/Images/") + cfileName);
                    FileInfo f = new FileInfo(csavePath);
                    if (f.Exists)
                    {
                        f.Delete();
                    }
                    string fileName = Guid.NewGuid().ToString("N") + Path.GetFileName(product_thumbnail.FileName);
                    string savePath = Path.Combine(Server.MapPath("~/Images/") + fileName);
                    product_thumbnail.SaveAs(savePath);
                    product.product_thumbnail_image = fileName;
                }
                dbContext.SubmitChanges();
                if (img_1 != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + Path.GetFileName(img_1.FileName);
                    string savePath = Path.Combine(Server.MapPath("~/Images/") + fileName);
                    img_1.SaveAs(savePath);
                    int imgId = 0;
                    if (0 < images.Count)
                    {
                        imgId = images[0].product_image_id;
                        string cfileName = images[0].product_image_file_name;
                        string csavePath = Path.Combine(Server.MapPath("~/Images/") + cfileName);
                        FileInfo f = new FileInfo(csavePath);
                        if (f.Exists)
                        {
                            f.Delete();
                        }
                    }
                    product_image image = dbContext.product_images.SingleOrDefault(i => i.product_image_id == imgId);
                    if (image == null)
                    {
                        image = new product_image();
                        dbContext.product_images.InsertOnSubmit(image);
                    }
                    image.product_id = product.product_id;
                    image.product_image_file_name = fileName;
                    dbContext.SubmitChanges();
                }
                if (img_2 != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + Path.GetFileName(img_2.FileName);
                    string savePath = Path.Combine(Server.MapPath("~/Images/") + fileName);
                    img_2.SaveAs(savePath);
                    int imgId = 0;
                    if (1 < images.Count)
                    {
                        imgId = images[1].product_image_id;
                        string cfileName = images[1].product_image_file_name;
                        string csavePath = Path.Combine(Server.MapPath("~/Images/") + cfileName);
                        FileInfo f = new FileInfo(csavePath);
                        if (f.Exists)
                        {
                            f.Delete();
                        }
                    }
                    product_image image = dbContext.product_images.SingleOrDefault(i => i.product_image_id == imgId);
                    if (image == null)
                    {
                        image = new product_image();
                        dbContext.product_images.InsertOnSubmit(image);
                    }
                    image.product_id = product.product_id;
                    image.product_image_file_name = fileName;
                    dbContext.SubmitChanges();
                }
                if (img_3 != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + Path.GetFileName(img_3.FileName);
                    string savePath = Path.Combine(Server.MapPath("~/Images/") + fileName);
                    img_3.SaveAs(savePath);
                    int imgId = 0;
                    if (2 < images.Count)
                    {
                        imgId = images[2].product_image_id;
                        string cfileName = images[2].product_image_file_name;
                        string csavePath = Path.Combine(Server.MapPath("~/Images/") + cfileName);
                        FileInfo f = new FileInfo(csavePath);
                        if (f.Exists)
                        {
                            f.Delete();
                        }
                    }
                    product_image image = dbContext.product_images.SingleOrDefault(i => i.product_image_id == imgId);
                    if (image == null)
                    {
                        image = new product_image();
                        dbContext.product_images.InsertOnSubmit(image);
                    }
                    image.product_id = product.product_id;
                    image.product_image_file_name = fileName;
                    dbContext.SubmitChanges();
                }
                if (img_4 != null)
                {
                    string fileName = Guid.NewGuid().ToString("N") + Path.GetFileName(img_4.FileName);
                    string savePath = Path.Combine(Server.MapPath("~/Images/") + fileName);
                    img_4.SaveAs(savePath);
                    int imgId = 0;
                    if (3 < images.Count)
                    {
                        imgId = images[3].product_image_id;
                        string cfileName = images[3].product_image_file_name;
                        string csavePath = Path.Combine(Server.MapPath("~/Images/") + cfileName);
                        FileInfo f = new FileInfo(csavePath);
                        if (f.Exists)
                        {
                            f.Delete();
                        }
                    }
                    product_image image = dbContext.product_images.SingleOrDefault(i => i.product_image_id == imgId);
                    if (image == null)
                    {
                        image = new product_image();
                        dbContext.product_images.InsertOnSubmit(image);
                    }
                    image.product_id = product.product_id;
                    image.product_image_file_name = fileName;
                    dbContext.SubmitChanges();
                }

                ViewBag.categories = dbContext.product_categories.ToList();
                ViewBag.images = product.product_images.ToList();
                TempData["productEditSucceed"] = 1;
                return View(product);
            }
        }

        public ActionResult ProductDelete(int id)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                product prd = dbContext.products.SingleOrDefault(p => p.product_id == id);
                List<product_image> images = prd.product_images.ToList();
                foreach (product_image image in images)
                {
                    string fileName = image.product_image_file_name;
                    string savePath = Path.Combine(Server.MapPath("~/Images/") + fileName);
                    FileInfo f = new FileInfo(savePath);
                    if (f.Exists)
                    {
                        f.Delete();
                    }
                }
                dbContext.product_images.DeleteAllOnSubmit(images);
                dbContext.SubmitChanges();
                dbContext.products.DeleteOnSubmit(prd);
                dbContext.SubmitChanges();
                List<product> products = dbContext.products.ToList();
                return View("ProductList", products);
            }
        }

        public ActionResult UserList()
        {
            using (var dbContext = new SHSDBDataContext())
            {
                return View(dbContext.user_accounts.ToList());
            }
        }

        public ActionResult UserEdit(int id)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                return View(dbContext.user_accounts.SingleOrDefault(u => u.user_account_id == id));
            }
        }

        [HttpPost]
        public ActionResult UserEdit(user_account ua)
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
                changedAccount.user_password = ua.user_password;
                dbContext.SubmitChanges();
                ViewBag.editSucceed = "Thay đổi thông tin thành công!";
                return View(changedAccount);
            }
        }

        public ActionResult UserDelete(int id)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                user_account deletedUser = dbContext.user_accounts.SingleOrDefault(u => u.user_account_id == id);
                List<user_order> orders = deletedUser.user_orders.ToList();
                foreach (user_order order in orders)
                {
                    List<user_order_product> order_Products = order.user_order_products.ToList();
                    dbContext.user_order_products.DeleteAllOnSubmit(order_Products);
                }
                dbContext.SubmitChanges();
                dbContext.user_orders.DeleteAllOnSubmit(orders);
                dbContext.SubmitChanges();
                dbContext.user_accounts.DeleteOnSubmit(deletedUser);
                dbContext.SubmitChanges();
            }
            return RedirectToAction("UserList", "Admin");
        }

        public ActionResult OrderProcess(int id = 0)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                List<user_order> orders = dbContext.user_orders.Where(uo => uo.is_processed == false).OrderByDescending(uo => uo.order_time).ToList();
                if (id != 0)
                {
                    TempData["id"] = id;
                    TempData["details"] = 1;
                    return RedirectToAction("OrderProcess", "Admin", new { id = 0, orders });
                }
                return View(orders);
            }
        }

        public ActionResult OrderDetails()
        {
            using (var dbContext = new SHSDBDataContext())
            {
                int id = (int)TempData["id"];
                List<user_order_product> order_products = dbContext.user_order_products.Where(op => op.user_order_id == id).ToList();
                return PartialView(order_products);
            }
        }

        public ActionResult OrderProcessConfirm(int id)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                user_order processOrder = dbContext.user_orders.SingleOrDefault(uo => uo.user_order_id == id);
                processOrder.is_processed = true;
                dbContext.SubmitChanges();
                List<user_order> orders = dbContext.user_orders.Where(uo => uo.is_processed == false).OrderByDescending(uo => uo.order_time).ToList();
                return RedirectToAction("OrderProcess", "Admin", new { id = 0, orders });
            }
        }

        public ActionResult OrderSucceed(int id = 0)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                List<user_order> orders = dbContext.user_orders.Where(uo => uo.is_processed == true && uo.is_delivered == false).OrderByDescending(uo => uo.order_time).ToList();
                if (id != 0)
                {
                    TempData["id"] = id;
                    TempData["details"] = 1;
                    return RedirectToAction("OrderSucceed", "Admin", new { id = 0, orders });
                }
                return View(orders);
            }
        }

        public ActionResult OrderSucceedConfirm(int id)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                user_order processOrder = dbContext.user_orders.SingleOrDefault(uo => uo.user_order_id == id);
                processOrder.is_delivered = true;
                dbContext.SubmitChanges();
                if (processOrder.user_account_id != null)
                {
                    user_account user = dbContext.user_accounts.SingleOrDefault(ua => ua.user_account_id == processOrder.user_account_id);
                    int bonus_point = (int)processOrder.order_total_value / 100000;
                    user.user_point = bonus_point;
                    if (user.user_point >= 2000)
                    {
                        user.user_member_tier = UserRanking.DIAMOND;
                    }
                    else if (user.user_point >= 750)
                    {
                        user.user_member_tier = UserRanking.PLATINUM;
                    }
                    else if (user.user_point >= 250)
                    {
                        user.user_member_tier = UserRanking.GOLD;
                    }
                    else if (user.user_point >= 100)
                    {
                        user.user_member_tier = UserRanking.SILVER;
                    }
                    else
                    {
                        user.user_member_tier = UserRanking.BRONZE;
                    }
                    dbContext.SubmitChanges();
                }
                List<user_order> orders = dbContext.user_orders.Where(uo => uo.is_processed == false).OrderByDescending(uo => uo.order_time).ToList();
                return RedirectToAction("OrderSucceed", "Admin", new { id = 0, orders });
            }
        }

        public ActionResult OrderDeleteFromProcess(int id)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                user_order deleteOrder = dbContext.user_orders.SingleOrDefault(uo => uo.user_order_id == id);
                List<user_order_product> refDeleteOrder = dbContext.user_order_products.Where(op => op.user_order_id == deleteOrder.user_order_id).ToList();
                foreach (user_order_product refProduct in refDeleteOrder)
                {
                    product prd = dbContext.products.SingleOrDefault(p => p.product_id == refProduct.product_id);
                    prd.product_inventory += refProduct.order_product_amount;
                    dbContext.SubmitChanges();
                }
                dbContext.user_order_products.DeleteAllOnSubmit(refDeleteOrder);
                dbContext.SubmitChanges();
                dbContext.user_orders.DeleteOnSubmit(deleteOrder);
                dbContext.SubmitChanges();
            }
            return RedirectToAction("OrderProcess", "Admin");
        }

        public ActionResult OrderDeleteFromSucceed(int id)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                user_order deleteOrder = dbContext.user_orders.SingleOrDefault(uo => uo.user_order_id == id);
                List<user_order_product> refDeleteOrder = dbContext.user_order_products.Where(op => op.user_order_id == deleteOrder.user_order_id).ToList();
                foreach (user_order_product refProduct in refDeleteOrder)
                {
                    product prd = dbContext.products.SingleOrDefault(p => p.product_id == refProduct.product_id);
                    prd.product_inventory += refProduct.order_product_amount;
                    dbContext.SubmitChanges();
                }
                dbContext.user_order_products.DeleteAllOnSubmit(refDeleteOrder);
                dbContext.SubmitChanges();
                dbContext.user_orders.DeleteOnSubmit(deleteOrder);
                dbContext.SubmitChanges();
            }
            return RedirectToAction("OrderSucceed", "Admin");
        }
    }
}