using StoreModels;
using System.Collections.Generic;
namespace StoreBL
{
    public interface ICustomerBL
    {
        void AddNewCustomer(Customer customer);
        List<Customer> GetCustomers();
        Customer FindCustomerOnEmail(string emailAddress);
        Customer FindCustomerOnLastName(string lastName);

        void CheckIfEmailExists(string emailAddress);
    }
}