using System;
using StoreModels;
using StoreBL;
using Entity = StoreDL.Entities;
using Serilog;
namespace StoreUI
{
    /// <summary>
    /// Contains the UI for the cart
    /// </summary>
    public class CartMenu : IMenu
    {
        Customer customer;
        private CartBL cartBL;
        private IUserBL ourUserBL;
        private Entity.P0DatabaseContext context;
        private bool active = true;
        public CartMenu(Customer customer, CartBL cart, IUserBL userBL, Entity.P0DatabaseContext context){
            if(customer != null){
                this.customer = customer;
            }
            cartBL = cart;
            ourUserBL = userBL;
            this.context = context;
        }
        public void End(Customer customer)
        {
            active = false;
            MenuFactory menuFactory = new MenuFactory(ourUserBL,context,cartBL);
            menuFactory.Start(customer); 
        }

        public void End(){}

        public void Start()
        {   
            Order thisCartOrder = new Order();
            
            
            
            

            // Console.WriteLine(thisCartOrder.Customer.FirstName);
            // Console.WriteLine(thisCartOrder.orderItems.Quantity);
            
            while(active){
                try{
                    if(customer != null){
                        if(ourUserBL.CartID == null){
                            ourUserBL.CartID = cartBL.GetCustomerCart(customer);
                        }
                        //Console.WriteLine("ourUserBL.cartid with customer: "+ourUserBL.CartID);
                        thisCartOrder = cartBL.GetOrder(ourUserBL.CartID);
                    }else{
                        //Console.WriteLine("ourUserBL.cartid: "+ourUserBL.CartID);
                        thisCartOrder = cartBL.GetCartOrderWithNoCustomer(ourUserBL.CartID);
                    }
                    
                }catch(Exception e){
                    Log.Error(e.ToString());
                }

                if(thisCartOrder.Location != null){
                    Console.WriteLine("Your store location is: "+thisCartOrder.Location.LocationName);
                }else{
                    Console.WriteLine("You do not have a store selected.");
                }
                if(thisCartOrder.Quantity > 0){
                Console.WriteLine("This is what is in your cart");
                Console.WriteLine("You have "+thisCartOrder.Quantity+" of "+thisCartOrder.orderItems.Product.ProductName);
                }else{
                    Console.WriteLine("Your cart is empty. ");
                }
                // foreach (Item item in thisCartOrder.orderItems)
                // {
                //     Console.WriteLine("You have "+item.Quantity+" of "+item.Product.ProductName);
                // }
                Console.WriteLine("Here is your total. $"+thisCartOrder.Total);
                Console.WriteLine("[1] Submit your order");
                Console.WriteLine("[2] Clear your cart");
                Console.WriteLine("[3] Go back to the main menu. ");
                try{
                    string userInput = Console.ReadLine();
                    switch(userInput){
                        case "1":
                            if(this.customer != null){
                                if(thisCartOrder.Quantity > 0){
                                    Console.WriteLine("Order is submitted.");
                                    cartBL.pushOrder();
                                }else{
                                    Console.WriteLine("Your cart is empty, cannot submit order. ");
                                }     
                            }else{
                                Console.WriteLine("Please login or register to submit your order. ");
                            }
                            End(customer);
                            break;
                        case "2":
                            Console.WriteLine("Cart has been emptied");
                            ourUserBL.CartID = cartBL.EmptyCart(ourUserBL.CartID);
                            //Console.WriteLine("Cart :"+ourUserBL.CartID);
                            //End(customer);
                            break;
                        case "3":
                            End(customer);
                            break;
                        default:
                            Console.WriteLine("Please pick and choose a number;");
                            break;
                    }
                }catch(Exception e){
                    Log.Error(e.ToString());
                }
            }//while
        }//start
    }
}