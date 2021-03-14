using StoreModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace StoreMVC.Models
{
    public class CartModel
    {

        [DisplayName("Item Quantity")]
        public int Quantity { get; set; }
        [DisplayName("Location Name")]
        public string LocationName { get; set; }
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [DisplayName("Product Price")]
        public double Price { get; set; }
        [DisplayName("Product Category")]
        public Category Category { get; set; }

        public int ItemId { get; set; }
        public int LocationId { get; set; }
        public string CustomerId { get; set; }
    }
}
