using Model = StoreModels;
using Entity = StoreDL.Entities;
using System.Linq;
using System.Collections.Generic;
namespace StoreDL
{
    public class CustomerMapper
    {
        public Model.Customer ParseCustomer(Entity.Customer customer){
            if(customer.FirstName != null && customer.LastName != null && customer.EmailAddress != null){
                return new Model.Customer{
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    EmailAddress = customer.EmailAddress,
                    Id = customer.Id
                };
            }
            return new Model.Customer();
        }
        public Entity.Customer ParseCustomer(Model.Customer customer){
            if(customer.Id == null){
                return new Entity.Customer{
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    EmailAddress = customer.EmailAddress,
                };
            }
            return new Entity.Customer{
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                EmailAddress = customer.EmailAddress,
                Id = (int)customer.Id
            };
        }
    }
}