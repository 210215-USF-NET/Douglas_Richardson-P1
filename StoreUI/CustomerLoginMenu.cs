using System;
using StoreModels;
using StoreBL;
using System.ComponentModel.DataAnnotations;
using Entity = StoreDL.Entities;
using Serilog;
namespace StoreUI
{
    /// <summary>
    /// Allows the user to login
    /// </summary>
    public class CustomerLoginMenu : IMenu
    {
        Boolean active = true; //whether the menu is active in the console
        private ICustomerBL customerBL;
        private IManagerBL managerBL;
        private IUserBL userBL;
        private Entity.P0DatabaseContext context;
        private CartBL cartBL;
        public CustomerLoginMenu(ICustomerBL newCustomerBL, IManagerBL newManagerBL, IUserBL newUserBL, Entity.P0DatabaseContext context, CartBL newCartBL){
            customerBL = newCustomerBL;
            managerBL = newManagerBL;
            userBL = newUserBL;
            this.context = context;
            cartBL = newCartBL;
        }
        public void CreateElement()
        {
            Customer newCustomer = new Customer();
        }

        public void End(Customer customer)
        {
            active = false;
            userBL.LogUserIn = true;
            userBL.IsUserManager = false;
            Customer tempCustomer = customerBL.FindCustomerOnEmail(customer.EmailAddress);
            customer.Id = tempCustomer.Id;
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
        public void End(Manager manager)
        {
            active = false;
            userBL.LogUserIn = true;
            userBL.IsUserManager = true;
            MenuFactory menuFactory = new MenuFactory(userBL,context,cartBL);
            menuFactory.Start(manager);   
        }
        public void End()
        {
            active = false;
            Console.WriteLine("Exiting Customer Menu. ");
            MenuFactory menuFactory = new MenuFactory(userBL,context,cartBL);
            menuFactory.Start(); 
        }
        public void Start()
        {
            Console.WriteLine("Type in your email to login. ");
            Console.WriteLine("Type !stop to exit out of this menu.");
            //Check managers then customers
            do{
                try{
                    string userInput = Console.ReadLine();
                    if(userInput.Equals("!stop")){ 
                        End();     
                    }
                    if((new EmailAddressAttribute().IsValid(userInput))){
                        Manager thisManager = managerBL.FindManagerOnEmail(userInput);
                        if(thisManager != null){
                            End(thisManager);
                        }
                        Customer thisCustomer = customerBL.FindCustomerOnEmail(userInput);
                        if(thisCustomer != null){
                            End(thisCustomer);
                        }
                        if(thisCustomer == null && thisManager == null){
                            Console.WriteLine("This account does not exist. ");
                        }
                    }else{
                        Console.WriteLine("Please type in a valid email. ");
                    }
                }catch(Exception e){
                    Log.Error(e.ToString());
                }
            }while(active);
        }//start

    }//class
}