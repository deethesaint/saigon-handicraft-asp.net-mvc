using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VuThanhDuong_DA.Models;

namespace VuThanhDuong_DA.Models
{
    public class InCartProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImgFileName { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public int Amount { get; set; }
        public int MaxAmount { get; set; }
        public InCartProduct(int id)
        {
            using (var dbContext = new SHSDBDataContext())
            {
                product prd = dbContext.products.SingleOrDefault(p => p.product_id == id);
                Id = prd.product_id;
                Name = prd.product_name;
                ImgFileName = prd.product_thumbnail_image;
                Price = (decimal)prd.product_price;
                Discount = (decimal)prd.product_discount;
                Amount = 1;
                MaxAmount = (int)prd.product_inventory;
            }
        }
    }
}