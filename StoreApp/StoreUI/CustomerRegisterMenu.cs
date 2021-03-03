using System;
using StoreModels;
using StoreBL;
using Serilog;
using Entity = StoreDL.Entities;
namespace StoreUI
{

    /// <summary>
    /// Allows the user to register to be a customer or a manager
    /// </summary>
    public class CustomerRegisterMenu : IMenu
    {
        bool active = true; //whether the menu is active in the console

      
        private ICustomerBL customerBL;
        private IManagerBL managerBL;
        private IUserBL userBL;
        private Entity.P0DatabaseContext context;
        private CartBL cartBL;
        public CustomerRegisterMenu(ICustomerBL newCustomerBL, IManagerBL newManagerBL, IUserBL newUserBL, Entity.P0DatabaseContext context, CartBL cartBL){
            customerBL = newCustomerBL;
            managerBL = newManagerBL;
            userBL = newUserBL;
            this.context = context;
            this.cartBL = cartBL;
        }

        public void End(){
            active = false;
            MenuFactory menuFactory = new MenuFactory(userBL,context,cartBL);
            menuFactory.Start();   
        }
        public void End(Manager manager){
            active = false;
            managerBL.AddNewManager(manager);
            userBL.LogUserIn = true;
            userBL.IsUserManager = true;
            MenuFactory menuFactory = new MenuFactory(userBL,context,cartBL);
            menuFactory.Start(manager);   
        }
        public void End(Customer customer){
            active = false;
            customerBL.AddNewCustomer(customer);

            Customer tempCustomer = customerBL.FindCustomerOnEmail(customer.EmailAddress);
            customer.Id = tempCustomer.Id;
            userBL.LogUserIn = true;
            userBL.IsUserManager = false;
            if(userBL.CartID == null || userBL.CartID == 0){
                userBL.CartID = cartBL.GetCustomerCart(customer);
                if(userBL.CartID == 0){
                    userBL.CartID = cartBL.NewCart();
                    cartBL.AddCustomer(customer,userBL.CartID);
                }
            }else{
                cartBL.AddCustomer(customer,userBL.CartID);
            }
            MenuFactory menuFactory = new MenuFactory(userBL,context,cartBL);
            menuFactory.Start(customer);   
        }
        public void Start(){
            //For customer or manager, like the fields on a webpage, before its sent to the server
            Customer newCustomer = new Customer();
            Console.WriteLine("New Customer Menu. ");
            Console.WriteLine("Type !stop to exit out of this menu.");

            do{
                //let the user know
                if(newCustomer.FirstName == null){
                    Console.WriteLine("Please type your first name. ");
                }else if(newCustomer.LastName == null){
                    Console.WriteLine("Please type your last name. ");
                }else if(newCustomer.EmailAddress == null){
                    Console.WriteLine("Please type a valid email address. ");
                }else{
                    Console.WriteLine("Do you want to be a manager? Yes or No");
                }

                //Get userinput
                try{
                    string userInput = Console.ReadLine();
                    if(userInput.Equals("!stop")){ 
                        End();     
                    }
                    if(newCustomer.FirstName == null){
                        newCustomer.FirstName = userInput;
                    }else if(newCustomer.LastName == null){
                        newCustomer.LastName = userInput;
                    }else if(newCustomer.EmailAddress == null){
                        customerBL.CheckIfEmailExists(userInput);
                        newCustomer.EmailAddress = userInput;
                    }else{
                        //Finish making customer
                        userInput = userInput.ToLower();
                        if(userInput.Equals("no") || userInput.Equals("n")){
                            Console.WriteLine("Welcome "+newCustomer.FirstName+"!");
                            End(newCustomer);
                        }else if(userInput.Equals("yes") || userInput.Equals("y")){
                            Manager newManager = new Manager();
                            newManager.FirstName = newCustomer.FirstName;
                            newManager.LastName = newCustomer.LastName;
                            newManager.EmailAddress = newCustomer.EmailAddress;
                            newCustomer = null;
                            Console.WriteLine("Welcome aboard "+newManager.FirstName+" as a new manager!");
                            End(newManager);
                        }
                        
                    }//End of if !stop
                }catch(EmailExistsException){
                    Console.WriteLine("The given email address is already in use. ");
                }catch(Exception e){
                    Log.Error(e.ToString());
                }
           }while(active);
        }//End of start


    }
}