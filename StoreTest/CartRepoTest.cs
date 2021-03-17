using Xunit;
using Microsoft.EntityFrameworkCore;
using StoreDL;
using StoreModels;
using System.Linq;
using System;
using System.Collections.Generic;

namespace StoreTest
{
    /// <summary>
    /// Testing the Order db repo
    /// </summary>
    public class CartRepoTest
    {
        private readonly DbContextOptions<CustomerDBContext> options;
        public CartRepoTest()
        {
            //use sqlite to create an inmemory test.db
            options = new DbContextOptionsBuilder<CustomerDBContext>()
            .UseSqlite("Filename=Test.db")
            .Options;
            Seed();
        }


        [Fact]
        public void TestGetLocation()
        {
            using (var context = new CustomerDBContext(options))
            {
                //Given
                LocationRepo locationRepo = new LocationRepo(context);
                //When
                //cartid then customer id
                Location location = locationRepo.GetLocationFromId(2);
                //Then
                Assert.Equal(2, location.Id);
                context.ChangeTracker.Clear();
            }
        }

        [Fact]
        public void TestCheckLocationName()
        {
            using (var context = new CustomerDBContext(options))
            {
                //Given
                LocationRepo locationRepo = new LocationRepo(context);
                //When
                //cartid then customer id
                Location location = locationRepo.GetLocationFromId(2);
                //Then
                Assert.Equal("Store 2", location.LocationName);
            }
        }

        [Fact]
        public void TestItemId()
        {
            using (var context = new CustomerDBContext(options))
            {
                //Given
                ProductRepo productRepo = new ProductRepo(context);
                LocationRepo locationRepo = new LocationRepo(context);
                ItemRepo repo = new ItemRepo(context, productRepo, locationRepo);
                //When
                //cartid then customer id
                Item item = repo.GetItemOnId(1);
                //Then
                Assert.Equal(1, item.Id);
            }
        }

        [Fact]
        public void TestItemName()
        {
            using (var context = new CustomerDBContext(options))
            {
                //Given
                ProductRepo productRepo = new ProductRepo(context);
                LocationRepo locationRepo = new LocationRepo(context);
                ItemRepo repo = new ItemRepo(context, productRepo, locationRepo);
                //When
                //cartid then customer id
                Item item = repo.GetItemOnId(1);
                //Then
                Assert.Equal("Leash", item.Product.ProductName);
            }
        }

        [Fact]
        public void TestItemQuantity()
        {
            using (var context = new CustomerDBContext(options))
            {
                //Given
                ProductRepo productRepo = new ProductRepo(context);
                LocationRepo locationRepo = new LocationRepo(context);
                ItemRepo repo = new ItemRepo(context, productRepo, locationRepo);
                //When
                //cartid then customer id
                Item item = repo.GetItemOnId(1);
                //Then
                Assert.Equal(3, item.Quantity);
            }
        }

/*        [Fact]
        public void TestItemLocationName()
        {
            using (var context = new CustomerDBContext(options))
            {
                //Given
                ProductRepo productRepo = new ProductRepo(context);
                LocationRepo locationRepo = new LocationRepo(context);
                ItemRepo repo = new ItemRepo(context, productRepo, locationRepo);
                //When
                //cartid then customer id
                Item item = repo.GetItemOnId(1);
                //Then
                Assert.Equal("test store", item.ItemLocation.LocationName);
            }
        }*/

/*        [Fact]
        public void TestAddCart()
        {
            using (var context = new CustomerDBContext(options))
            {
                //Given
                ProductRepo productRepo = new ProductRepo(context);
                LocationRepo locationRepo = new LocationRepo(context);
                ItemRepo repo = new ItemRepo(context, productRepo, locationRepo);
                OrderRepo orderRepo = new OrderRepo(context);
                //When
                Item item = repo.GetItemOnId(1);
                Location location = locationRepo.GetLocationFromId(1);
                List<OrderHistoryModel> orders = orderRepo.GetOrder("wyspar@gmail.com");
                //Then
                Assert.Equal(1, orders.Select(x => x).Where(x => x.Id == 1).FirstOrDefault().Id);
            }
        }*/

        /*        [Fact]
                public void TestGetCart()
                {
                    using (var context = new CustomerDBContext(options))
                    {
                        //Given
                        ProductRepo productRepo = new ProductRepo(context);
                        LocationRepo locationRepo = new LocationRepo(context);
                        ItemRepo repo = new ItemRepo(context, productRepo, locationRepo);
                        CartRepo cartRepo = new CartRepo(context, repo);
                        //When

                        Tuple<Cart, int> tuple = cartRepo.GetCartFromItemId(2, "1");
                        //Then
                        Assert.Equal(2, tuple.Item1.Id);
                    }
                }*/

        /*        [Fact]
                public void TestMaxQuantity()
                {
                    using (var context = new CustomerDBContext(options))
                    {
                        //Given
                        ProductRepo productRepo = new ProductRepo(context);
                        LocationRepo locationRepo = new LocationRepo(context);
                        ItemRepo repo = new ItemRepo(context, productRepo, locationRepo);
                        CartRepo cartRepo = new CartRepo(context, repo);
                        //When
                        Item item = repo.GetItemOnId(1);
                        Location location = locationRepo.GetLocationFromId(1);

                        Tuple<Cart, int> tuple = cartRepo.GetCartFromItemId(2, "1");
                        //Then
                        Assert.Equal(3, tuple.Item2);
                    }
                }*/

        private void Seed()
        {
            using (var context = new CustomerDBContext(options))
            {
                //This makes sure that the state of the db gets recreated every time to maintain the modularity of the tests.
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Users.AddRange
                (
                    new StoreMVCUser
                    {
                        Id = "1",
                        FirstName = "Douglas",
                        LastName = "Richardson",
                        Email = "wyspar@gmail.com"
                    }
        
                );
                context.Locations.AddRange(
                    new Location
                    {
                        Id = 2,
                        LocationName = "Store 2",
                        Address = "321 store way"
                    }

                );
                context.Products.AddRange(
                    new Product
                    {
                        Id = 2,
                        ProductName = "Bed",
                        Price = 6,
                        Category = Category.Beds
                    }
                );
                context.Items.AddRange(
                    new Item
                    {
                        Id = 3,
                        Quantity = 3,
                        Product = new Product
                        {
                            Id = 1,
                            ProductName = "Dog Bowl",
                            Price = 3,
                            Category = Category.Accessories
                        },
                        ItemLocation = new Location
                        {
                            Id = 3,
                            LocationName = "test store",
                            Address = "123 way"
                        }                        
                    }
                );
                context.Carts.AddRange(
                    new Cart { 
                        Id = 1,
                        CustomerId = "1",
                        Quantity = 3,
                        Location = new Location
                        {
                            Id = 1,
                            LocationName = "test store",
                            Address = "123 way"
                        },
                        Item = new Item
                        {
                            Id = 1,
                            Quantity = 3,
                            Product = new Product
                            {
                                Id = 3,
                                ProductName = "Leash",
                                Price = 3,
                                Category = Category.Leashes
                            },
                            ItemLocation = new Location
                            {
                                Id = 5,
                                LocationName = "test store",
                                Address = "123 way"
                            }
                        }
                    }
                );
                context.Orders.AddRange(
                    new Order
                    {
                        Customer = new StoreMVCUser
                        {
                            Id = "2",
                            FirstName = "Reese",
                            LastName = "Can",
                            Email = "ahh@gmail.com"
                        },
                        Id = 1,
                        Location = new Location
                        {
                            Id = 8,
                            LocationName = "test store",
                            Address = "123 way"
                        },
                        Total = 4.99
                    }
                );
                context.SaveChanges();
            }
        }

    }
}