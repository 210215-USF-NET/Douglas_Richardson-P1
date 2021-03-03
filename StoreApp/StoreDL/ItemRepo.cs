using System;
using Model = StoreModels;
using Entity = StoreDL.Entities;
using Mapper = StoreDL.Mappers;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
namespace StoreDL
{
    /// <summary>
    /// The item repo handles the changing of the location and quantity for items
    /// </summary>
    public class ItemRepo
    {
        private Entity.P0DatabaseContext context;
        private Mapper.ItemMapper mapper; 
        public ItemRepo(Entity.P0DatabaseContext context, Mapper.ItemMapper mapper){
            this.mapper = mapper;
            this.context = context;
        }
        public void AddNewItem(Model.Item Item)
        {
            Entity.Item newItem = mapper.ParseItem(Item);
            //context.Entry(newItem).State = EntityState.Added;
            context.Items.Add(newItem);
            context.SaveChanges();
            context.Entry(newItem).State = EntityState.Detached;
        }

        public List<Model.Item> GetItems()
        {
           return context.Items.Include("Product").AsNoTracking().Include("Location").AsNoTracking().Select(x => mapper.ParseItem(x)).ToList();
        }

        public void ChangeItemLocation(Model.Item item){
            Entity.Item findItem = context.Items.Find(item.ItemID);
            context.Entry(findItem).State = EntityState.Modified;
            Entity.LocationTable findLocation = context.LocationTables.Find(item.ItemLocation.LocationID);
            findItem.LocationId = findLocation.Id;
            context.SaveChanges();
            context.ChangeTracker.Clear();
            context.Entry(findItem).State = EntityState.Detached;
        }

        public void ChangeItemQuantity(Model.Item item){
            Entity.Item findItem = context.Items.Find(item.ItemID);
            context.Entry(findItem).State = EntityState.Modified;
            //context.Entry(findItem).CurrentValues.SetValues(mapper.ParseItem(item));
            findItem.Quantity = item.Quantity;
            //Entity.Product findProduct = context.Products.Find(item.Product.Id);
            //findItem.Quantity = item.Quantity;
            context.SaveChanges();
            context.ChangeTracker.Clear();
            context.Entry(findItem).State = EntityState.Detached;
        }
    }//class
}
