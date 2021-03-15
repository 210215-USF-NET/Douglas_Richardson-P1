using System;
using StoreModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace StoreModels
{
    public class ItemModel
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
    }
}
