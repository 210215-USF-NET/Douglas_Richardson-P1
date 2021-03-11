using System;
using Model = StoreModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;

namespace StoreDL
{
    /// <summary>
    /// The item repo handles the changing of the location and quantity for items
    /// </summary>
    public class ItemRepo
    {
        private CustomerDBContext context;
        private ProductRepo productRepo;
        private LocationRepo locationRepo;
        public ItemRepo(CustomerDBContext context, ProductRepo productRepo, LocationRepo locationRepo){
            this.context = context;
            this.productRepo = productRepo;
            this.locationRepo = locationRepo;
        }
        public void AddNewItem(Model.Item Item)
        {
            context.Entry(Item).State = EntityState.Added;
            context.Items.Add(Item);
            context.SaveChanges();
            context.Entry(Item).State = EntityState.Detached;
        }

        public List<Model.Item> GetItems()
        {
            var result = context.Items
                .Include("ItemLocation")
                .AsNoTracking()
                .Include("Product")
                .AsNoTracking()
                .Select(item => item)
                .ToList();
/*            foreach (var item in result)
            {
                Model.Item newItem = new Model.Item();
                Model.Location newLocation = new Model.Location();
                Model.Product newProduct = new Model.Product();
                newItem.ItemLocation = newLocation;
                newItem.Product = newProduct;
                newItem.Id = item.Id;
                newItem.Quantity = item.Quantity;
                newItem.ItemLocation.Id = item.ItemLocation.Id;
                newItem.Product.Id = item.Product.Id;
*//*                newItem.ItemLocation.Address = item.Address;
                newItem.ItemLocation.LocationName = item.LocationName;
                newItem.Product.Price = item.Price;
                newItem.Product.Category = item.Category;
                newItem.Product.ProductName = item.ProductName;*//*
                gotItems.Add(newItem);
                Debug.WriteLine("item id " + item.Id);
            }*/
            
            return result;
        }

        public void DeleteItem(int Id)
        {
            Model.Item findItem = GetItemOnId(Id);
            context.Entry(findItem).State = EntityState.Deleted;
            Model.Product foundProduct = productRepo.GetProductById(findItem.Product.Id);
            context.Entry(foundProduct).State = EntityState.Deleted;
            context.Items.Remove(findItem);
            context.Products.Remove(foundProduct);
            context.SaveChanges();
            context.ChangeTracker.Clear();
            context.Entry(findItem).State = EntityState.Detached;
            context.Entry(foundProduct).State = EntityState.Detached;
        }

        public void UpdateItem(Model.Item itemToBeUpdated){
            Model.Item findItem = GetItemOnId(itemToBeUpdated.Id);
            context.Entry(findItem).State = EntityState.Modified;
            if (itemToBeUpdated.ItemLocation.Id != 0 && itemToBeUpdated.ItemLocation != null)
            {
                Model.Location findLocation = locationRepo.GetLocationFromId(itemToBeUpdated.ItemLocation.Id);
                findItem.ItemLocation = findLocation;
                findItem.ItemLocation.Id = findLocation.Id;
            }
            Model.Product foundProduct = productRepo.GetProductById(findItem.Product.Id);
            context.Entry(foundProduct).State = EntityState.Modified;
            foundProduct.Price = itemToBeUpdated.Product.Price;
            foundProduct.Category = itemToBeUpdated.Product.Category;
            foundProduct.ProductName = itemToBeUpdated.Product.ProductName;

            findItem.Quantity = itemToBeUpdated.Quantity;
            context.SaveChanges();
            context.ChangeTracker.Clear();
            context.Entry(findItem).State = EntityState.Detached;
            context.Entry(foundProduct).State = EntityState.Detached;
        }

        public Model.Item GetItemOnId(int Id)
        {
            var result = context.Items
                .Include("ItemLocation")
                .AsNoTracking()
                .Include("Product")
                .AsNoTracking()
                .Select(item => item)
                .Where(item => item.Id == Id)
                .FirstOrDefault();
            return result;
        }
    }//class
}
