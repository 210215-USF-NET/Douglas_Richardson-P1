using StoreModels;
using StoreDL;
using System.Collections.Generic;
namespace StoreBL
{
    /// <summary>
    /// Handles the creation of products, and a default item
    /// </summary>
    public class ProductBL
    {
        private ProductRepo productRepo;
        private ItemRepo itemRepo;
        public ProductBL(ProductRepo newProductRepo, ItemRepo itemRepo)
        {
            productRepo = newProductRepo;
            this.itemRepo = itemRepo;
        }
        public int AddProduct(Product Product){      
            return productRepo.AddNewProduct(Product);
        }
        //Give the Products to the user
        public List<Product> GetProducts(){
            return productRepo.GetProducts();
        }

        public Product GetProductById(int Id)
        {
            return productRepo.GetProductById(Id);
        }
    }
}