using StoreModels;
using StoreDL;
using System.Collections.Generic;
namespace StoreBL
{
    /// <summary>
    /// Allows the user to register as a customer
    /// </summary>
    public class CustomerBL : ICustomerBL
    {

        private ICustomerRepo iCustomerRepo;
        public CustomerBL(ICustomerRepo newICustomerRepo){
            iCustomerRepo = newICustomerRepo;
        }

        public void AddNewCustomer(Customer customer){
            iCustomerRepo.AddNewCustomer(customer);
        }
        public List<Customer> GetCustomers(){
            return iCustomerRepo.GetCustomers();
        }
        public Customer FindCustomerOnEmail(string emailAddress){
            return iCustomerRepo.GetCustomerByEmail(emailAddress);
        }
        public Customer FindCustomerOnLastName(string lastName){
            return iCustomerRepo.GetCustomerByLastName(lastName);
        }
        public void CheckIfEmailExists(string emailAddress){
            if(FindCustomerOnEmail(emailAddress) != null){
                throw new EmailExistsException();
            }
        }
    }
}