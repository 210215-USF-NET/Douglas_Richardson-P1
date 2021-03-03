using StoreDL;
using StoreModels;
using System;
using Serilog;
namespace StoreBL
{   
    public class CartBL
    {
        /// <summary>
        /// The cart holds items for the customer when they are shopping, This holds the precusor to an order.         
        /// The cart is not saved to the database until there is a customer applied to it
        /// </summary>
        private CartRepo cartRepo;
        private OrderRepo orderRepo;
        public Order cartOrder;
        private IUserBL userBL;
        private int? cartID;
        
        public CartBL(CartRepo newCartRepo, OrderRepo newOrderRepo, IUserBL newUserBL){
            cartRepo = newCartRepo;
            orderRepo = newOrderRepo;
            userBL = newUserBL;
            cartOrder = new Order(); 
            // cartOrder = cartRepo.GetCartOrder(cartID);
            // if(cartOrder == null){
            //     cartOrder = new Order();
            //     cartOrder.orderItems = new Item();
                //cartOrder.orderItems = new List<Item>();
            
        }

        public void NewCartOrder(){
            cartOrder = new Order();
            cartID = 0;
        }

        public int? GetCustomerCart(Customer customer){
            if(userBL.LogUserIn == true){
                //Order newCartOrder = cartRepo.FindCustomerCartOrder(cartOrder.Customer.Id);
                cartID = cartRepo.GetCartId(customer.Id);
                userBL.CartID = cartID;
            }
            return cartID;
        }
        
        public int? NewCart(){
            return cartRepo.AddNewCart(cartOrder);
        }

        //When the customer is added to the cart/order then the cart shall be pushed to the db
        public void AddCustomer(Customer customer, int? cartID){
            if(customer != null){
                cartOrder.Customer = customer;
                cartOrder.Customer.Id = customer.Id;
                if(cartID != null && cartID != 0){
                    try{
                        cartRepo.UpdateCart(cartID,cartOrder);
                    }catch(Exception e){
                        Log.Error(e.ToString());
                    }
                    
                    // Order getCartOrder = cartRepo.GetCartOrder(cartID);
                    
                    // if(getCartOrder != null){
                        
                    // }
                    
                } 
            }
        }

        //Gives the cart a location
        public void AddLocation(Location location){
            cartOrder.Location = location;
        }

        //Adds a new item to the cart/order
        public void AddNewItemToOrder(Item item,int choice,Customer customer,IUserBL userBL,Location location){
            if(item != null){
                cartOrder.Quantity = choice;
                cartOrder.Total = item.Product.Price * choice;
                cartOrder.orderItems = item;
                cartOrder.orderItems.Product.ProductName = item.Product.ProductName;
                //Check if a cart exists then update it
                Order thisCartOrder = cartRepo.GetCartOrder(userBL.CartID);
                thisCartOrder.Quantity = choice;
                thisCartOrder.Total = item.Product.Price * choice;
                thisCartOrder.orderItems = item;
                thisCartOrder.orderItems.Product.ProductName = item.Product.ProductName;
                if(thisCartOrder != null){
                    cartRepo.UpdateCart(userBL.CartID,cartOrder);
                }
                
            }
        }

        // public int? GetCartID(){
        //     return cartRepo.GetCartId(cartOrder..customerID);
        // }

        public void RemoveItemFromOrder(Item item){
            
        }

        //Gets the entire order
        public Order GetOrder(int? cartId){
            Order getCartOrder = cartRepo.GetCartOrder(cartId);
            if(getCartOrder == null){
                return cartOrder;
            }else{
                return getCartOrder;
            }
        }

        public Order GetCartOrderWithNoCustomer(int? cartId){
            return cartRepo.GetCartOrderWithNoCustomer(cartId);
        }

        public int GetAmountOfItem(Item item){
            if(cartOrder.orderItems != null){
                return cartOrder.orderItems.Quantity;
                // foreach (Item findItem in cartOrder.orderItems)
                // {
                //     if(findItem.ItemID == item.ItemID){
                //         return findItem.Quantity;
                //     }
                // }
            }
            return 0;
        }

        //Get the price of the cart on the database and if that does not exist then return the local one
        public double GetPrice(){
            return cartOrder.Total; 
        }

        //Get the location of the cart on the database and if that does not exist then return the local one
        public Location GetLocation(){
            return cartOrder.Location;
        }

        //Empties the cart on the database and locally
        public int? EmptyCart(int? cartId){
            
            cartOrder = new Order();
            if(cartOrder.Customer == null){
                cartRepo.EmptyCartNoCustomer(cartId);
            }else{
                cartRepo.EmptyCart(cartId);
            }
            return cartId;
        }
        //Push the cart into an order repo, then clear the cart
        public void pushOrder(){
            //Get all the needed items to create an order
            Order getCartOrder = cartRepo.GetCartOrder(userBL.CartID);
            if(getCartOrder != null){
                try{
                    orderRepo.PushOrder(getCartOrder);
                    cartRepo.EmptyCart(userBL.CartID);
                }catch(Exception e){
                    Log.Error(e.ToString());
                }  
            }
        }
    }
}