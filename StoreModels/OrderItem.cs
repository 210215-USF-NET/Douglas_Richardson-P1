using StoreModels;
using System.Collections.Generic;
namespace StoreModels
{
    /// <summary>
    /// This class should contain all the fields and properties that define a customer order. 
    /// Orders are created when a customer is using the shop to put products/items in their cart.
    /// Order Histories are separate
    /// </summary>
    public class OrderItem
    {
        public StoreMVCUser Customer { get; set; }
        public int Quantity { get; set; }
        public Item Item { get; set; }
        public Order Order { get; set; }
        public int Id{get;set;}
        public override string ToString(){
            return Id.ToString()+" "+Customer.FirstName;
        }
    }
}