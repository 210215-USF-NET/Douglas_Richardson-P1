
using System;
using Model = StoreModels;
using Entity = StoreDL.Entities;
//using Mapper = StoreDL.Mappers;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
namespace StoreDL
{
    /// <summary>
    /// The customer repo, creates a customer in the database
    /// </summary>
    public class CustomerRepo : ICustomerRepo
    {
        private Entity.P0DatabaseContext context;
        private CustomerMapper mapper; 
        public CustomerRepo(Entity.P0DatabaseContext context, CustomerMapper mapper){
            this.mapper = mapper;
            this.context = context;
        }
        public void AddNewCustomer(Model.Customer customer)
        {
            Entity.Customer newCustomer = mapper.ParseCustomer(customer);
            context.Entry(newCustomer).State = EntityState.Added;
            context.Customers.Add(newCustomer);
            context.Customers.AsNoTracking();
            context.SaveChanges();
            context.Entry(newCustomer).State = EntityState.Detached;
            Log.Information("New Customer created. "+newCustomer.Id+" Email: "+newCustomer.EmailAddress);
        }

        public Model.Customer GetCustomerByLastName(string lastname)
        {
            context.Customers.AsNoTracking();
            return context.Customers.Select(x => mapper.ParseCustomer(x)).ToList().FirstOrDefault(x => x.LastName == lastname);
        }
        public Model.Customer GetCustomerByEmail(string email)
        {
            context.Customers.AsNoTracking();
            return context.Customers.Select(x => mapper.ParseCustomer(x)).ToList().FirstOrDefault(x => x.EmailAddress == email);
        }
        public List<Model.Customer> GetCustomers()
        {
            context.Customers.AsNoTracking();
           return context.Customers.Select(x => mapper.ParseCustomer(x)).ToList();
        }
        // private string jsonString;
        // private string filePath = "../StoreDL/Customers.json";
        // public void AddNewCustomer(Customer customer){
        //     List<Customer> customersFromFile = GetCustomers();
        //     customersFromFile.Add(customer);
        //     jsonString = JsonSerializer.Serialize(customersFromFile);
        //     File.WriteAllText(filePath, jsonString);
        // }
        // public List<Customer> GetCustomers(){
        //     try{
        //         jsonString = File.ReadAllText(filePath);
        //     }catch(Exception e){
        //         return new List<Customer>();
        //     }
        //     return JsonSerializer.Deserialize<List<Customer>>(jsonString);
        // }
    }//class
}
