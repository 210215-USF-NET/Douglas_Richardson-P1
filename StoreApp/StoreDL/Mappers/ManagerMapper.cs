using Model = StoreModels;
using Entity = StoreDL.Entities;
using System.Linq;
using System.Collections.Generic;
namespace StoreDL.Mappers
{
    public class ManagerMapper
    {
        public Model.Manager ParseManager(Entity.Manager manager){
            if(manager.FirstName != null && manager.LastName != null && manager.EmailAddress != null){
                return new Model.Manager{
                    FirstName = manager.FirstName,
                    LastName = manager.LastName,
                    EmailAddress = manager.EmailAddress
                };
            }
            return new Model.Manager();
        }
        public Entity.Manager ParseManager(Model.Manager manager){
            return new Entity.Manager{
                FirstName = manager.FirstName,
                LastName = manager.LastName,
                EmailAddress = manager.EmailAddress
            };
        }
    }
}