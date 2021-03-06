using System;
using StoreBL;
using StoreDL;
using Entity = StoreDL.Entities;
using Mapper = StoreDL.Mappers;
using StoreModels;
using System.Collections.Generic;
using Serilog;
namespace StoreUI
{
    /// <summary>
    /// Allows the user to pick a location and list the orders from that location
    /// </summary>
    public class OrderHistoryLocationMenu : IMenu
    {
        bool active = true;
        private IUserBL ourUserBL;
        private CartBL cartBL;
        private Entity.P0DatabaseContext context;
        private Manager manager;
        private LocationBL locationBL;
        private OrderBL orderBL;
        public OrderHistoryLocationMenu(Manager manager, CartBL newCartBL, IUserBL userBL , LocationBL locationBL, Entity.P0DatabaseContext context, OrderBL orderBL){
            ourUserBL = userBL;
            cartBL = newCartBL;
            this.context = context;
            this.locationBL = locationBL;
            this.manager = manager;
            this.orderBL = orderBL;
        }
        public void End(){}
        public void End(Manager manager)
        {
            active = false;
            MenuFactory menuFactory = new MenuFactory(ourUserBL,context,cartBL);
            if(manager != null){
                menuFactory.Start(manager);  
            }else{
                menuFactory.Start();  
            }  
        }
        public void Start()
        {
            active = true;
            List<Location> thisLocationList = new List<Location>();
            try{
                thisLocationList = locationBL.GetLocations();
            }catch(Exception e){
                Log.Error(e.ToString());
            }
            
            if(thisLocationList != null){
                if(thisLocationList.Count != 0){
                    Console.WriteLine("Choose a location to look at its order history");
                    foreach (Location location in thisLocationList)
                    {
                        Console.WriteLine("["+location.LocationID+"] "+location.LocationName);
                    }
                }
            }else{
                Console.WriteLine("There are no locations.");
                End(manager);
                active = false;
            }
            while(active){
                try{
                    string userInput = Console.ReadLine();
                    int choice = Int32.Parse(userInput);
                    thisLocationList = locationBL.GetLocations();
                    bool nothingFound = true;
                    foreach (Location location in thisLocationList){             
                        if(choice == location.LocationID){
                            PrintLocationOrderHistory(location);
                            nothingFound = false;
                            active = false;
                        }
                    }   
                    if(nothingFound){
                        throw new InvalidItemIdException("Not a valid location id.");
                    }
                }catch(FormatException){
                    Console.WriteLine("Please type in a number. ");
                }catch(InvalidItemIdException){
                    Console.WriteLine("Not a valid location id. ");
                }
                catch(Exception e){
                    Log.Error(e.ToString());
                }
            }
            
        }

        private void PrintLocationOrderHistory(Location location){
            Console.WriteLine("Here is the order history for "+location.LocationName);
            Console.WriteLine("Orders are sorted by most expensive to least expensive");
            //This gets the order list and sorts it by the total
            List<Order> theseOrders = orderBL.GetLocationOrders(location.LocationID);
            theseOrders.Sort((x, y) => y.Total.CompareTo(x.Total));
            //Print out the orders
            int counter = 0;
            foreach (var order in theseOrders)
            {
                if(order != null){
                    counter++;
                    Console.WriteLine("Order "+counter+": with "+order.Customer.LastName+" who had "+order.Quantity+" "+order.orderItems.Product.ProductName+"(s) price: "+order.Total);
                }
                
            }
            Console.WriteLine();
            End(manager);
        }
        
    }//Class
}