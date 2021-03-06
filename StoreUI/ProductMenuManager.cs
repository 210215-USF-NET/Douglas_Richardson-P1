using System;
using System.Collections.Generic;
using StoreModels;
using StoreBL;
using Entity = StoreDL.Entities;
using Serilog;
namespace StoreUI
{
    /// <summary>
    /// The manager product menu, allows the manager to create new products, items and locations.
    /// </summary>
    public class ProductMenuManager : IMenu
    {
        //active is for the product menu
        bool active = true;
        private Manager manager;
        private ProductBL productBL;
        private ItemBL itemBL;
        private IUserBL ourUserBL;
        private LocationBL locationBL;
        private CartBL cartBL;
        private Entity.P0DatabaseContext context;
        public ProductMenuManager(ProductBL newProductBL, ItemBL newItemBL, IUserBL userBL, LocationBL newLocationBL, CartBL newCartBL, Entity.P0DatabaseContext context){
            productBL = newProductBL;
            itemBL = newItemBL;
            ourUserBL = userBL;
            locationBL = newLocationBL;
            cartBL = newCartBL;
            this.context = context;
        }

        public void End()
        {
            active = false;
            MenuFactory menuFactory = new MenuFactory(ourUserBL,context,cartBL);
            menuFactory.Start();   
        }
        public void End(Manager manager)
        {
            active = false;
            MenuFactory menuFactory = new MenuFactory(ourUserBL,context,cartBL);
            menuFactory.Start(manager);   
        }
        public void Start(){}


        //Primary starting point for the manager user
        public void Start(Manager manager)
        {
            this.manager = manager;
            active = true;
            do{
                Console.WriteLine("Product Manager Menu");
                Console.WriteLine("[1] Add New Product");
                Console.WriteLine("[2] Add New Location");
                Console.WriteLine("[3] Change Item Quantity");
                Console.WriteLine("[4] Change Item Location");
                Console.WriteLine("[5] Back to Previous Menu");
                ManagerView();
            }while(active);
        }//start

        //The logic behind the manager choices
        private void ManagerView(){
            string userInput = Console.ReadLine();
            switch(userInput){
                //New product
                case "1":
                    NewProductMenu();
                    break;
                //Add New Location
                case "2":
                    AddNewLocation();
                    break;
                //Change Item Quantity  
                case "3":
                    ListItemsMenuManager(true,false);
                    break;
                //Change Item Location
                case "4":
                    ListItemsMenuManager(false,true);
                    break;
                case "5":
                    End(manager);
                    break;
                default:
                    Console.WriteLine("Invalid Input, please pick and choose a number. ");
                    break;
            }
        }
     
        private void NewProductMenu(){
            Product newProduct = new Product();
            newProduct.Category = Category.Nothing;
            Console.WriteLine("New Product Menu. ");
            Console.WriteLine("Type !stop to exit out of this menu.");
            do{
                if(newProduct.ProductName == null){
                    Console.WriteLine("Please type in a product name.");
                }else if(newProduct.Price == 0.0d){
                    Console.WriteLine("Please type in a price. ");
                }else if(newProduct.Category == Category.Nothing){
                    Console.WriteLine("Please pick in a valid category. ");
                    Console.WriteLine("[1] Food");
                    Console.WriteLine("[2] Collars");
                    Console.WriteLine("[3] Leashes");
                    Console.WriteLine("[4] Clothes");
                    Console.WriteLine("[5] Beds");
                    Console.WriteLine("[6] Accessories");
                }
                try{
                    //give the user input, check if they want to stop
                    string userInput = Console.ReadLine();
                    if(userInput.Equals("!stop")){ 
                        End(manager);
                    }else{
                        if(newProduct.ProductName == null){
                            newProduct.ProductName = userInput;
                        }else if(newProduct.Price == 0.0d){
                            double result;
                            Double.TryParse(userInput, out result);
                            newProduct.Price = result;
                        }else if(newProduct.Category == Category.Nothing){
                            switch(userInput){
                                case "1":
                                    newProduct.Category = Category.Food;
                                    active = false;
                                    break;
                                case "2":
                                    newProduct.Category = Category.Collars;
                                    active = false;
                                    break;
                                case "3":
                                    newProduct.Category = Category.Leashes;
                                    active = false;
                                    break;
                                case "4":
                                    newProduct.Category = Category.Clothes;
                                    active = false;
                                    break;
                                case "5":
                                    newProduct.Category = Category.Beds;
                                    active = false;
                                    break;
                                case "6":
                                    newProduct.Category = Category.Accessories;
                                    active = false;
                                    break;
                            }
                            productBL.AddProduct(newProduct);
                            Start(manager);
                        }
                    }
                }catch(Exception e){
                    Console.WriteLine(e.ToString());
                }
            }while(active);
        }//End of NewProductMenu

        //This method lists all of the items for the manager, and allows them to choose an item to change the item's location or quantity
        private void ListItemsMenuManager(bool isChangingItemQuantity, bool isChangingItemLocation){
            if(isChangingItemQuantity){
                Console.WriteLine("Change Item Quantity Menu");
            }else{
                Console.WriteLine("Change Item Location Menu");
            }
            Console.WriteLine("Type !stop to exit out of this menu.");
            //Print all the items, this lets the user know which ones to choose
            List<Item> thisItemList = new List<Item>();
            try{
                thisItemList = itemBL.GetItems();
            }catch(Exception e){
                Console.WriteLine(e.ToString());
            }
            
            if(thisItemList != null){
                if(thisItemList.Count != 0){
                    foreach (Item item in thisItemList){
                        Console.WriteLine("["+item.ItemID+"] "+item.Product.ProductName+" amount: "+item.Quantity);
                    }
                }else{
                    Console.WriteLine("There are no items.");
                    Start(manager);
                    active = false;
                }
            }else{
                Console.WriteLine("There are no items.");
                Start(manager);
                active = false;
            }
            while(active){
                try{
                    //give the user input, check if they want to stop
                    string userInput = Console.ReadLine();
                    if(userInput.Equals("!stop")){
                        End(manager);    
                    }else{
                        //change string input to an int, then find it in the list
                        int choice = Int32.Parse(userInput);
                        bool nothingFound = true;
                        foreach (Item item in thisItemList){
                            if(choice == item.ItemID){
                                nothingFound = false;
                                if(isChangingItemQuantity){
                                    GiveItemQuantityMenu(item);  
                                    Start(manager); 
                                    break;
                                }else{
                                    GiveItemLocationMenu(item);
                                    Start(manager);
                                    break;       
                                }    
                            }
                        }
                        if(nothingFound){
                            throw new InvalidItemIdException("Not a valid item id.");
                        }        
                    }
                }catch(FormatException){
                    Console.WriteLine("Please type in a number. ");
                }catch(InvalidItemIdException){
                    Console.WriteLine("Not a valid item id. ");
                }catch(Exception e){
                    Console.WriteLine(e.ToString());
                    continue;
                }
            }
        }//ListItemsMenuManager

        //Lets the user give an item an amount
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
                }catch(NumberCannotBeNegative){
                    Console.WriteLine("The number cannot be negative. ");
                }catch(Exception e){
                    Log.Error(e.ToString());
                }
            }while(active);
        }

        //Lets the user assign a location to an item
        private void GiveItemLocationMenu(Item item){
            Console.WriteLine("Choose a new location for an item");
            List<Location> thisLocationList = new List<Location>();
            try{
                thisLocationList = locationBL.GetLocations();
            }catch(Exception e){
                Console.WriteLine(e.ToString());
            }
            
            if(thisLocationList != null){
                if(thisLocationList.Count != 0){
                    foreach (Location location in thisLocationList)
                    {
                        Console.WriteLine("["+location.LocationID+"] "+location.LocationName);
                    }
                }
            }else{
                Console.WriteLine("There are no locations.");
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
            }
        }

        //Creates a new location and sends it to the BL
        private void AddNewLocation(){
            Console.WriteLine("Please enter a new location");
            Location location = new Location();
            do{
                if(location.LocationName == null){
                    Console.WriteLine("Type in the name for the location.");
                }else if(location.Address == null){
                    Console.WriteLine("Type in the address for the location. ");
                }
                try{
                    string userInput = Console.ReadLine();
                    if(location.LocationName == null){
                        location.LocationName = userInput;
                    }else if(location.Address == null){
                        location.Address = userInput;
                        locationBL.AddNewLocation(location);
                        active = false;
                        Start(manager);
                    }
                }catch(FormatException){
                    Console.WriteLine("Please type in a number. ");
                }catch(Exception e){
                    Log.Error(e.ToString());
                }
            }while(active);
        }//AddNewLocation 

    }//class
}