using System;
using StoreBL;
using StoreDL;
using Entity = StoreDL.Entities;
using Mapper = StoreDL.Mappers;
using StoreModels;
using Serilog;
namespace StoreUI
{
    /// <summary>
    /// The main menu, sends the user to different locations in the program
    /// </summary>
    public class MenuFactory : IMenu
    {
        Boolean active = true;
        Manager manager;
        //main userBL
        private IUserBL ourUserBL;
        private Entity.P0DatabaseContext context;
        private CartBL cartBL;
        public MenuFactory(IUserBL newUserBL, Entity.P0DatabaseContext context, CartBL cartBL){
            ourUserBL = newUserBL;
            this.context = context;
            this.cartBL = cartBL;
        }

        public void End(){
            active = false;
        }

        public void Start(){
            do{
                Console.WriteLine("Choose from one of the following by entering a number. ");
                Console.WriteLine("[1] Login in");
                Console.WriteLine("[2] Register");
                Console.WriteLine("[3] View Cart");
                Console.WriteLine("[4] View Products");
                Console.WriteLine("[5] Exit Store");
                CustomerNotLoggedIn();
            }while(active);
        }//End of start()
        
        public void Start(Manager manager){
            this.manager = manager;
            active = true;
            do{
                if(ourUserBL.LogUserIn){
                    Console.WriteLine("Hello "+manager.LastName);
                    Console.WriteLine("Choose from one of the following by entering a number. ");
                    Console.WriteLine("[1] View Product Menu");
                    Console.WriteLine("[2] View Order Histories from Locations");
                    Console.WriteLine("[3] Search a Customer");
                    Console.WriteLine("[4] Logout");
                    ManagerIsLoggedIn(manager);
                }
            }while(active);
        }

        public void Start(Customer customer){
            active = true;
            do{
                if(ourUserBL.LogUserIn){
                    Console.WriteLine("Hello "+customer.FirstName);
                    Console.WriteLine("Choose from one of the following by entering a number. ");
                    Console.WriteLine("[1] View Products ");
                    Console.WriteLine("[2] View Cart");
                    Console.WriteLine("[3] View Order History");
                    Console.WriteLine("[4] Logout");
                    CustomerIsLoggedIn(customer);
                }else{
                    Start();
                }
            }while(active);

        }

        //This handles when the customer is not logged/registered in to the store
        private void CustomerNotLoggedIn(){
            string userInput = Console.ReadLine();
            switch (userInput){
                //Login
                case "1":
                    End();
                    CustomerLoginMenu customerLoginMenu = new CustomerLoginMenu(
                        new CustomerBL(new CustomerRepo(context, new CustomerMapper())), 
                        new ManagerBL(new ManagerRepo(context, new Mapper.ManagerMapper())), 
                        ourUserBL,
                        context,
                        cartBL);
                    customerLoginMenu.Start();
                    break;
                //Register
                case "2":
                    End();
                    CustomerRegisterMenu customerRegisterMenu = new CustomerRegisterMenu(
                        new CustomerBL(new CustomerRepo(context, new CustomerMapper())), 
                        new ManagerBL(new ManagerRepo(context, new Mapper.ManagerMapper())), 
                        ourUserBL,
                        context,
                        cartBL);
                    customerRegisterMenu.Start();
                    break;
                //View Cart
                case "3":
                    End();
                    CartMenu cartMenu = new CartMenu(null,
                    cartBL,
                    ourUserBL,
                    context);
                    cartMenu.Start();
                    break;
                //View Products
                case "4": 
                    ProductMenu productMenu = new ProductMenu(
                        new ProductBL(new ProductRepo(context, new Mapper.ProductMapper()),context),
                        new ItemBL(new ItemRepo(context, new Mapper.ItemMapper())),
                        ourUserBL,
                        new LocationBL(new LocationRepo(context, new Mapper.LocationMapper())),
                        cartBL,
                        context);
                    productMenu.Start(null);
                    End();
                    break;
                //Exit store
                case "5":
                    Console.WriteLine("Thank you for visiting our store! Please come back later. ");
                    End();
                    break;      
                default:
                    Console.WriteLine("Invalid Input, please choose and type a number. ");
                    break;

            }
        }

        //When the customer is logged in to the store
        private void CustomerIsLoggedIn(Customer customer){
            string userInput = Console.ReadLine();
            switch (userInput){
                //View Products
                case "1":
                    End();
                    ProductMenu productMenu = new ProductMenu(
                        new ProductBL(new ProductRepo(context, new Mapper.ProductMapper()),context),
                        new ItemBL(new ItemRepo(context, new Mapper.ItemMapper())),
                        ourUserBL,
                        new LocationBL(new LocationRepo(context, new Mapper.LocationMapper())),
                        cartBL,
                        context);
                    productMenu.Start(customer);
                    break;
                //View Cart
                case "2":
                    End();
                    CartMenu cartMenu = new CartMenu(customer,
                    cartBL,
                    ourUserBL,
                    context);
                    cartMenu.Start();
                    break;
                //View Order History
                case "3":
                    End();
                    OrderHistoryMenu orderHistoryMenu = new OrderHistoryMenu(customer,cartBL,ourUserBL,context,
                    new OrderBL(new OrderRepo(context, new Mapper.OrderMapper()),ourUserBL));
                    orderHistoryMenu.Start();
                    break;
                //Logout
                case "4": 
                    Console.WriteLine("Have a good day! "+customer.FirstName);
                    ourUserBL.IsUserManager = false;
                    ourUserBL.LogUserIn = false;
                    customer = null;
                    ourUserBL.CartID = 0;
                    cartBL.NewCartOrder();
                    Start();
                    break; 
                default:
                    Console.WriteLine("Invalid Input, please choose and type a number. ");
                    break;

            }
        }

        //When the manager is logged in to the store
        private void ManagerIsLoggedIn(Manager manager){
            string userInput = Console.ReadLine();
            switch (userInput){
                //View Products
                case "1":
                    End();
                    ProductMenuManager productMenu = new ProductMenuManager(
                        new ProductBL(new ProductRepo(context, new Mapper.ProductMapper()),context),
                        new ItemBL(new ItemRepo(context, new Mapper.ItemMapper())),
                        ourUserBL,
                        new LocationBL(new LocationRepo(context, new Mapper.LocationMapper())),
                        cartBL,
                        context
                        );
                    productMenu.Start(manager);
                    break;
                //View Order Histories from location
                case "2":
                    End();
                    OrderHistoryLocationMenu OrderHistoryLocationMenu = new OrderHistoryLocationMenu(
                        manager,
                        cartBL,
                        ourUserBL,
                        new LocationBL(new LocationRepo(context, new Mapper.LocationMapper())),
                        context,
                        new OrderBL(new OrderRepo(context, new Mapper.OrderMapper()),ourUserBL)
                    );
                    OrderHistoryLocationMenu.Start();
                    break;
               
                //Search customer
                case "3":
                    SearchForACustomer();
                    break;
                     //Log Out
                case "4":
                    Console.WriteLine("Good work! Come back tomorrow. ");
                    ourUserBL.IsUserManager = false;
                    ourUserBL.LogUserIn = false;
                    manager = null;
                    ourUserBL.CartID = 0;
                    cartBL.NewCartOrder();
                    Start();
                    break;        
                default:
                    Console.WriteLine("Invalid Input, please pick and choose a number. ");
                    break;

            }
        }

        //Searches for a customer by their last name on the database
        private void SearchForACustomer(){ 
            while(active){
                try{
                    Console.WriteLine("Please type in a customer's last name");
                    string userInput = Console.ReadLine();
                    if(userInput != null && userInput != ""){
                        if(userInput.Equals("!stop")){
                            Start(manager);
                            active = false;
                        }
                        CustomerBL newCustomerBL = new CustomerBL(new CustomerRepo(context, new CustomerMapper()));
                        Customer foundCustomer = newCustomerBL.FindCustomerOnLastName(userInput);
                        if(foundCustomer != null){
                            Console.WriteLine();
                            Console.WriteLine("Their customer id is: "+foundCustomer.Id);
                            Console.WriteLine("Their first name is: "+foundCustomer.FirstName);
                            Console.WriteLine("Their last name is: "+foundCustomer.LastName);
                            Console.WriteLine("Their email address is: "+foundCustomer.EmailAddress);   
                            Console.WriteLine();    
                            Start(manager);
                            active = false;
                        }else{
                            Console.WriteLine("There is no customer by that last name.");
                            Start(manager);
                            active = false;
                        }   
                    }
                }catch(Exception e){
                    Log.Error(e.ToString());
                }
            }
        }
    }//End of class
}