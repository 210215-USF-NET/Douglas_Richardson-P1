using StoreModels;
using System.Collections.Generic;
namespace StoreModels
{
    /// <summary>
    /// This class should contain all the fields and properties that define a customer order. 
    /// Orders are created when a customer is using the shop to put products/items in their cart.
    /// Order Histories are separate
    /// </summary>
    public class Order
    {
        //customerid in customer
        private Customer customer;
        private Location location;
        private double total;

        private bool inCart;
        public Customer Customer { get; set; }
        public double Total { get; set; }
        public bool InCart { get; set; }
        public Location Location { get; set; }
        public int? Id{get;set;}
        public int Quantity { get; set; }
        public Item orderItems{get;set;}
        //public List<Item> orderItems{get;set;}
        public override string ToString(){
            return Id.ToString()+" "+Location.LocationName+" "+Customer.FirstName;
        }
    }
}