using StoreModels;
using System.Collections.Generic;
namespace StoreBL
{
    public interface IManagerBL
    {
         public void AddNewManager(Manager manager);
         public List<Manager> GetManagers();
        Manager FindManagerOnEmail(string emailAddress);
    }
}