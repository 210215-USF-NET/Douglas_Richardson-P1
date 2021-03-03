using Model = StoreModels;
using Entity = StoreDL.Entities;
using System.Collections.Generic;
namespace StoreDL
{
    /// <summary>
    /// The interface for the customer repo
    /// </summary>
    public interface ICustomerRepo
    {
         public void AddNewCustomer(Model.Customer customer);
         //getCustomers is for putting all the customers into a list and resaving them into the json file
        List<Model.Customer> GetCustomers();

        Model.Customer GetCustomerByLastName(string lastname);
        Model.Customer GetCustomerByEmail(string email);

    }
}