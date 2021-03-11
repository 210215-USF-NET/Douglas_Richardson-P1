using System;
using Model = StoreModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Serilog;
namespace StoreDL
{
    /// <summary>
    /// Creates new products on the database
    /// </summary>
    public class ProductRepo
    {
        private CustomerDBContext context;
        public ProductRepo(CustomerDBContext context){
            this.context = context;
        }
        public int AddNewProduct(Model.Product Product)
        {
            context.Entry(Product).State = EntityState.Added;
            context.Products.Add(Product);
            context.SaveChanges();
            context.Entry(Product).State = EntityState.Detached;
            Log.Information("New Product Added. "+ Product.ProductName);
            return Product.Id;
        }

        public List<Model.Product> GetProducts()
        {
            if (context.Products.Any(x => x != null))
            {
                var result = from product in context.Products select product;
                return result.ToList();
            }
            else
            {
                return null;
            }
        }

        public Model.Product GetProductById(int Id)
        {
            var result = context.Products
                .AsNoTracking()
                .Select(item => item)
                .Where(item => item.Id == Id)
                .FirstOrDefault();
            return result;
        }

    }//class
}
