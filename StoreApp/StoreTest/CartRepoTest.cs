using Xunit;
using Microsoft.EntityFrameworkCore;
using Entity = StoreDL.Entities;
using Mapper = StoreDL.Mappers;
using StoreDL;
using Model = StoreModels;
using System.Linq;
using System;
namespace StoreTest
{
    /// <summary>
    /// Testing the Order db repo
    /// </summary>
    public class CartRepoTest
    {
        private readonly DbContextOptions<Entity.P0DatabaseContext> options;
        public CartRepoTest()
        {
            //use sqlite to create an inmemory test.db
            options = new DbContextOptionsBuilder<Entity.P0DatabaseContext>()
            .UseSqlite("Filename=Test.db")
            .Options;
            Seed();
        }
        
        [Fact]
        public void TestGetCartWithCustomer()
        {
            using (var context = new Entity.P0DatabaseContext(options)){
                //Given
                CartRepo repo = new CartRepo(context, new Mapper.CartMapper());
                //When
                //cartid then customer id
                Model.Order thisOrder = repo.GetCartWithCustomer(7,1);
                Model.Order thisOrder2 = repo.GetCartWithCustomer(1,2);
                //Then
                Assert.Equal(thisOrder.orderItems.Product.ProductName, "Dog Bowl");
                Assert.Null(thisOrder2.Id);
            }
            using (var assertContext = new Entity.P0DatabaseContext(options)){
                var result = from cart in assertContext.Carts where cart.Id == 7 select cart;
                Assert.Equal(7,result.FirstOrDefault().Id);
            }
        }

        [Fact]
        public void TestGetCartId()
        {
            using (var context = new Entity.P0DatabaseContext(options)){
                //Given
                CartRepo repo = new CartRepo(context, new Mapper.CartMapper());
                //When
                //cartid then customer id
                int? cartId = repo.GetCartId(1);
                //Then
                Assert.Equal(cartId, 7);
            }
        }

        [Fact]
        public void TestAddNewCart()
        {
            using(var context = new Entity.P0DatabaseContext(options)){
                CartRepo repo = new CartRepo(context, new Mapper.CartMapper());
                Model.Location newLocation = new Model.Location{
                    LocationID = 2, LocationName = "Store 1", Address = "123 store way"
                };
                Model.Order newOrder = new Model.Order{
                    Id = 3,
                    Customer = new Model.Customer{Id = 2, FirstName = "Tony", LastName = "Smith", EmailAddress = "heytony@gmail.com"},
                    orderItems = new Model.Item{ItemID = 2, ItemLocation = newLocation, Product = new Model.Product{Id = 4, ProductName = "Dog Food", Price = 3}, Quantity = 12},
                    Location = newLocation,
                    Quantity = 6,
                    Total = 18
                };
                int? idOfNewCart = repo.AddNewCart(newOrder);
                Model.Order foundOrder = repo.GetCartWithCustomer(idOfNewCart,2);
                Assert.Equal(18,foundOrder.Total);
            }
        }

        [Fact]
        public void TestEmptyCart()
        {
            using(var context = new Entity.P0DatabaseContext(options)){
                CartRepo repo = new CartRepo(context, new Mapper.CartMapper());

                repo.EmptyCart(7);
                Model.Order foundOrder = repo.GetCartWithCustomer(7,1);
                Assert.Equal(foundOrder.Quantity,0);
            }
        }

        //customer, total, location, id, quantity, item
        private void Seed(){
            using (var context = new Entity.P0DatabaseContext(options))
            {
                //This makes sure that the state of the db gets recreated every time to maintain the modularity of the tests.
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Customers.AddRange(
                    new Entity.Customer{
                        Id = 1,
                        FirstName = "Bob",
                        LastName = "Bobbet",
                        EmailAddress = "bobrocks@gmail.com"
                    }
                );
                context.LocationTables.AddRange(
                    new Entity.LocationTable{
                        Id = 1,
                        LocationName = "Store 1",
                        LocationAddress = "123 store way"
                    }
                    
                );
                context.Products.AddRange(
                    new Entity.Product{
                        Id = 1,
                        ProductName = "Dog Bowl",
                        Price = 3,
                        Category = 6
                    }
                    
                );
                context.Items.AddRange(
                    new Entity.Item{
                        Id = 1,
                        Quantity = 3,
                        ProductId = 1,
                        LocationId = 1
                    }
                );
                context.Carts.AddRange(
                    new Entity.Cart{
                        Id = 7,
                        CustomerId = 1,
                        ItemId = 1,
                        LocationId = 1,
                        Quantity = 3,
                        Total = 9
                    }
                );
                context.SaveChanges();
            }
        }
    }
}