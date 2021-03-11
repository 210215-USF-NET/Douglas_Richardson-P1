using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreDL;
using StoreModels;
namespace StoreDL
{
    public class CustomerDBContext : IdentityDbContext<StoreMVCUser>
    {
        public CustomerDBContext(DbContextOptions<CustomerDBContext> options) : base(options){

        }
        public CustomerDBContext(){

        }

        public DbSet<Location> Locations {get;set;}
        public DbSet<ManagerString> ManagerStrings { get;set;}
        public DbSet<Item> Items { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
       
    }
}