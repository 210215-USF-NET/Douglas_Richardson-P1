using StoreModels;
using StoreDL;
using Entity = StoreDL.Entities;
using Mapper = StoreDL.Mappers;
using System.Collections.Generic;
namespace StoreBL
{
    /// <summary>
    /// Handles the creation of products, and a default item
    /// </summary>
    public class ProductBL
    {
        private ProductRepo productRepo;
        private Entity.P0DatabaseContext context;
        public ProductBL(ProductRepo newProductRepo, Entity.P0DatabaseContext context){
            productRepo = newProductRepo;
            this.context = context;
        }
        public void AddProduct(Product Product){
            
            productRepo.AddNewProduct(Product);
            //Turn product into an item, default zero quantity and no location, manager has to set these
            ItemBL newItemBL = new ItemBL(new ItemRepo(context, new Mapper.ItemMapper()));
            Item newItem = new Item();
            newItem.Product = Product;
            newItem.Quantity = 0;
            newItem.ItemLocation = null;
            //when the itme is added to the db, it creates a new product and a new item in the database
            newItemBL.AddItemToRepo(newItem);
        }
        //Give the Products to the user
        public List<Product> GetProducts(){
            return productRepo.GetProducts();
        }
    }
}