using System;
using System.Collections.Generic;
using StoreModels;
using StoreBL;
using Entity = StoreDL.Entities;
using Serilog;
namespace StoreUI
{
    /// <summary>
    /// The customer product menu, shows the locations and items that the customer can choose from
    /// </summary>
    public class ProductMenu : IMenu
    {
        //active is for the product menu
        bool active = true;
        private ProductBL productBL;
        private ItemBL itemBL;
        private IUserBL ourUserBL;
        private LocationBL locationBL;
        private CartBL cartBL;
        private Entity.P0DatabaseContext context;
        private Customer customer;
        public ProductMenu(ProductBL newProductBL, ItemBL newItemBL, IUserBL userBL, LocationBL newLocationBL, CartBL newCartBL, Entity.P0DatabaseContext context){
            productBL = newProductBL;
            itemBL = newItemBL;
            ourUserBL = userBL;
            locationBL = newLocationBL;
            cartBL = newCartBL;
            this.context = context;
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
        public void Start(){}

        //Customer product menu start
        public void Start(Customer customer)
        {
            this.customer = customer;
            active = true;
            do{
                Location location = cartBL.GetLocation();
                if(location == null){
                    CustomerView(this.customer);
                    active = false;
                }else{
                    Console.WriteLine("Your current store location is: "+location.LocationName);
                    Console.WriteLine("Do you want to change your store location?");
                    Console.WriteLine("Type yes, no or !stop ");
                    try{
                        string userInput = Console.ReadLine();
                        if(userInput.Equals("yes") || userInput.Equals("y")){
                            CustomerView(this.customer);
                            active = false;
                        }else if(userInput.Equals("no") || userInput.Equals("n")){
                            ListItemsMenuCustomer(this.customer,location);
                            active=false;
                        }else if(userInput.Equals("!stop")){
                            End(this.customer);
                            active=false;
                        }else{
                            Console.WriteLine("Please choose yes or no");
                        }
                    }catch(Exception e){
                        Log.Error(e.ToString());
                    }
                }
                
                
            }while(active);
        }//start customer

        //Choose the location
        private void CustomerView(Customer customer){
            //Get the store locations first, let the customer choose
            List<Location> thisLocationList = locationBL.GetLocations();
            
            if(thisLocationList != null){
                if(thisLocationList.Count != 0 ){
                    Console.WriteLine("Type in a number from the list to choose an store.");
                    foreach (Location location in thisLocationList){
                        if(location.LocationName != null){
                            Console.WriteLine("["+location.LocationID+"] "+location.LocationName);
                        }        
                    }
                }
            }else{
                Console.WriteLine("There are no store locations.");
                End(customer);
                active = false;
            }
            while(active){ 
                try{
                    string userInput = Console.ReadLine();
                    if(userInput.Equals("!stop")){ 
                        End(customer); 
                        break;   
                    }else{
                        //change string input to an int, then find it in the list
                        int choice = Int32.Parse(userInput);
                        bool nothingFound = true;
                        foreach (Location location in thisLocationList){
                            if(choice == location.LocationID){
                                cartBL.AddLocation(location);
                                cartBL.AddCustomer(customer,ourUserBL.CartID);
                                nothingFound = false;
                                ListItemsMenuCustomer(customer,location);
                                active = false;
                                break;
                            }
                        }
                        if(nothingFound){
                            throw new InvalidItemIdException("Not a valid location id.");
                        }
                    }
                }catch(FormatException){
                    Console.WriteLine("Please type in a number. ");
                }catch(InvalidItemIdException){
                    Console.WriteLine("Not a valid location id.");
                }catch(Exception e){
                    Log.Error(e.ToString());
                }
            }
        }

        //Opens the store, gets the items and displays them
        private void ListItemsMenuCustomer(Customer customer, Location location){
            Console.WriteLine();
            Console.WriteLine(location.LocationName+"'s Menu");
            bool choosingItem = true;
            int choice = 0;
            while(active){
                try{
                    //give the user input, check if they want to stop
                    string userInput = "";
                    if(choosingItem){
                        List<Item> thisItemList = itemBL.GetItems();
                        bool noItemsInStore = true;
                        
                        //List all the items in the store
                        foreach (Item item in thisItemList){
                            if(item.ItemLocation != null){
                                if(item.ItemLocation.LocationID == location.LocationID && item.Quantity > 0){
                                    Console.WriteLine("["+item.ItemID+"] "+item.Product.ProductName+" Price: "+item.Product.Price+" Quantity: "+item.Quantity);
                                    noItemsInStore = false;
                                }
                            }
                        }
                        
                        if(noItemsInStore){
                            Console.WriteLine("There are no items.");
                            Start(customer);
                            active = false;
                        }else{
                            Console.WriteLine("Type in a number from the list to choose an item.");
                            Console.WriteLine("Type !stop to exit out of this menu.");
                        }
                        userInput = Console.ReadLine();
                    }                    
                    if(userInput.Equals("!stop")){ 
                        End(customer);
                        active = false;
                    }else{
                        //change string input to an int, then find it in the list
                        if(choosingItem && active){
                            choice = Int32.Parse(userInput);
                        }
                        if(active){
                            bool nothingFound = true;
                            //Get all the items, check if the item is in the location
                            List<Item> thisItemList = itemBL.GetItems();
                            foreach (Item item in thisItemList){
                                if(item.ItemLocation != null){
                                    if(choice == item.ItemID && item.ItemLocation.LocationID == location.LocationID && item.Quantity > 0){
                                        choosingItem = false;
                                        int AmountInCart = 0;//cartBL.GetAmountOfItem(item);
                                        if((item.Quantity-AmountInCart)==0){
                                            Console.WriteLine("You have all of these items in your cart. ");
                                            choosingItem = true;
                                            nothingFound = false;
                                        }else{
                                            Console.WriteLine("There is "+(item.Quantity));
                                            Console.WriteLine("How many of "+item.Product.ProductName+" do you want?");
                                            Console.WriteLine("Please type in a number from 1 to "+(item.Quantity-AmountInCart));
                                            //Let the user they already have some in the cart
                                            if(AmountInCart>0){
                                                Console.WriteLine("You have "+AmountInCart+" in your cart already.");
                                            }
                                            userInput = Console.ReadLine();
                                            choice = Int32.Parse(userInput);
                                            //Check if the choice is between one and the item quantity
                                            if(choice>=1 && choice<=(item.Quantity-AmountInCart)){
                                                if(customer == null && ourUserBL.CartID == null){
                                                    ourUserBL.CartID = cartBL.NewCart();
                                                }else if(ourUserBL.CartID == 0 || ourUserBL.CartID == null){
                                                    ourUserBL.CartID = cartBL.NewCart();
                                                }
                                                cartBL.AddNewItemToOrder(item,choice,customer,ourUserBL,location);
                                                choosingItem = true;
                                                Console.WriteLine("Item added to cart. ");
                                                Console.WriteLine();
                                                choice = 0;
                                            }else{
                                                nothingFound = false;
                                            }
                                            nothingFound = false;
                                        }
                                    }
                                }
                            }
                            if(nothingFound){
                                throw new InvalidItemIdException("Not a valid item id.");
                            }
                        }
                    }
                }catch(FormatException){
                    Console.WriteLine("Please type in a number. ");
                }catch(InvalidItemIdException){
                    Console.WriteLine("Not a valid item id.");
                    ListItemsMenuCustomer(customer, location);
                    break;
                }catch(Exception e){
                    Log.Error(e.ToString());
                    break;
                }
            }
        }//ListItemsMenu
        
        private void GiveItemQuantityMenu(Item item){
            Console.WriteLine("Please enter the quantity for "+item.Product.ProductName);
            do{
                try{
                    string userInput = Console.ReadLine();
                    int choice = Int32.Parse(userInput);
                    item.Quantity = choice;
                    itemBL.ChangeItemQuantity(item);
                    active = false;
                }catch(FormatException){
                    Console.WriteLine("Please type in a number. ");
                }catch(Exception e){
                    Log.Error(e.ToString());
                }
            }while(active);
        }

        private void GiveItemLocationMenu(Item item){
            Console.WriteLine("Choose a new location for an item");
            List<Location> thisLocationList = new List<Location>();
            try{
                thisLocationList = locationBL.GetLocations();
            }catch(Exception e){
                Log.Error(e.ToString());
            }
            
            if(thisLocationList.Count != 0){
                foreach (Location location in thisLocationList)
                {
                    Console.WriteLine("["+location.LocationID+"] "+location.LocationName);
                }
            }else{
                Console.WriteLine("There are no locations.");
                active = false;
            }
            do{
                try{
                    string userInput = Console.ReadLine();
                    int choice = Int32.Parse(userInput);
                    thisLocationList = locationBL.GetLocations();
                    bool nothingFound = true;
                    foreach (Location location in thisLocationList){             
                        if(choice == location.LocationID){
                            item.ItemLocation = location;
                            itemBL.ChangeItemLocation(item);
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
            }while(active);
        }

    }//class
}