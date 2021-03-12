using StoreModels;
using System.Collections.Generic;
namespace StoreModels
{
    /// <summary>
    /// This class should contain all the fields and properties that define a customer order. 
    /// Orders are created when a customer is using the shop to put products/items in their cart.
    /// Order Histories are separate
    /// </summary>
    public class Cart
    {
        public Customer Customer { get; set; }
        public int Quantity { get; set; }
        public Location Location { get; set; }
        public Item Item { get; set; }
        public int Id{get;set;}
        public override string ToString(){
            return Id.ToString()+" "+Location.LocationName+" "+Customer.FirstName;
        }
    }
}