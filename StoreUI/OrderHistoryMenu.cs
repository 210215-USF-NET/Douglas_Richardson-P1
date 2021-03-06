using System;
using StoreBL;
using StoreDL;
using Entity = StoreDL.Entities;
using Mapper = StoreDL.Mappers;
using StoreModels;
using System.Collections.Generic;
namespace StoreUI
{
    /// <summary>
    /// This displays the order history of the customer sorted by price.
    /// </summary>
    public class OrderHistoryMenu : IMenu
    {
        
        bool active = true;
        private IUserBL ourUserBL;
        private CartBL cartBL;
        private Entity.P0DatabaseContext context;
        private Customer customer;
        private OrderBL orderBL;
        public OrderHistoryMenu(Customer customer, CartBL newCartBL, IUserBL userBL , Entity.P0DatabaseContext context, OrderBL orderBL){
            ourUserBL = userBL;
            cartBL = newCartBL;
            this.context = context;
            this.orderBL = orderBL;
            this.customer = customer;
        }
        public void End(){}
        public void End(Customer customer)
        {
            active = false;
            MenuFactory menuFactory = new MenuFactory(ourUserBL,context,cartBL);
            if(customer != null){
                menuFactory.Start(customer);  
            }else{
                menuFactory.Start();  
            }  
        }
        public void Start()
        {
            active = true;
            Console.WriteLine("Here is your order history. ");
            Console.WriteLine("Orders are sorted by most expensive to least expensive");
            //This gets the order list and sorts it by the total
            List<Order> theseOrders = orderBL.GetCustomerOrders(customer.Id);
            theseOrders.Sort((x, y) => y.Total.CompareTo(x.Total));
            //Print out the orders
            int counter = 0;
            foreach (var order in theseOrders)
            {
                counter++;
                Console.WriteLine("Order "+counter+": at "+order.Location.LocationName+" with "+order.Quantity+" "+order.orderItems.Product.ProductName+"(s) price: "+order.Total);
            }
            Console.WriteLine();
            End(customer);
            
            // while(active){  
            //     try{
            //         Console.WriteLine("Type !stop to go back");
            //         string userInput = Console.ReadLine();
            //         if(userInput.Equals("!stop")){
                        
            //             active = false;
            //         }
            //     }catch(Exception){

            //     }
            // }
        }
    }
}