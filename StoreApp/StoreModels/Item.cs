namespace StoreModels
{

    /// <summary>
    /// This data structure models a product and its quantity. The quantity was separated from the product as it could vary from orders and locations.  
    /// </summary>
    public class Item
    {
        private int itemID;
        private Product product;
        private int quantity;
        private Location location;
        public int? ItemID { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public Location ItemLocation {get;set;}

        public override string ToString(){
            return ItemID.ToString()+" "+ItemLocation.LocationName+" "+Product.ProductName;
        }
    }
}