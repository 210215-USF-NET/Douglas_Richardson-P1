using Model = StoreModels;
using Entity = StoreDL.Entities;
using System.Collections.Generic;
namespace StoreDL
{
    /// <summary>
    /// The manager interface repo
    /// </summary>
    public interface IManagerRepo
    {
         public void AddNewManager(Model.Manager manager);
         //getCustomers is for putting all the customers into a list and resaving them into the json file
        List<Model.Manager> GetManagers();

        Model.Manager GetManagerByEmail(string email);

    }
}