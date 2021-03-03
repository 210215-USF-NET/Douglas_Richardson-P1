namespace StoreModels
{
    //This class should contain all necessary fields to define a product.
    public class Product
    {
        private string productName;
        private double price;
        private Category category;
        public string ProductName { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; }
        public int? Id{get;set;}
    }
}