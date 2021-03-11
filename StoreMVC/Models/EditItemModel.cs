using System;
using StoreModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace StoreMVC.Models
{
    public class EditItemModel
    {
        [DisplayName("Item Quantity")]
        [Required]
        [Range(0,Int32.MaxValue, ErrorMessage = "Quantity must be positive or zero")]
        public int Quantity { get; set; }
        [DisplayName("Product Name")]
        [Required]
        public string ProductName { get; set; }
        [DisplayName("Product Price")]
        [Required]
        [Range(.5, Double.MaxValue, ErrorMessage = "Price must be greater than 0.50")]
        public double Price { get; set; }
        [DisplayName("Product Category")]
        [Required]
        [Range(2, 7, ErrorMessage = "Category must be between 2 and 7")]
        public Category Category { get; set; }
        [DisplayName("Location Name")]
        public string LocationName { get; set; }
        public int ItemId { get; set; }
        public int LocationId { get; set; }
        public int ProductId { get; set; }
    }
}
