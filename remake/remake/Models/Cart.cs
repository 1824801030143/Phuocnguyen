using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace remake.Models
{
    public class Cart
    {
        public int? IDProduct { get; set; }
        public string ProductName { get; set; }
        public int Amount { get; set; }
        public double? Price { get; set; }
        public double? Total { get; set; }
        public byte[] Image { get; set; }
        public Cart( int? id)
        {
            using(NuocHoaShopEntities db = new NuocHoaShopEntities())
            {
                this.IDProduct = id;
                Product pd = db.Products.Single(n => n.ID == id);
                this.ProductName = pd.ProductName;
                this.Image = pd.Image;
                this.Price = pd.Price;
                this.Amount = 1;
                this.Total = Price * Amount;
            }
        }

        public Cart(int id,int sl)
        {
            using (NuocHoaShopEntities db = new NuocHoaShopEntities())
            {
                this.IDProduct = id;
                Product pd = db.Products.Single(n => n.ID == id);
                this.ProductName = pd.ProductName;
                this.Image = pd.Image;
                this.Price = pd.Price;
                this.Amount = sl;
                this.Total = Price * Amount;
            }
        }     
    }
}