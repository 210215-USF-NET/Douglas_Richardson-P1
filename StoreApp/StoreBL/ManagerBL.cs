using StoreModels;
using StoreDL;
using System.Collections.Generic;
namespace StoreBL
{
    /// <summary>
    /// Lets the user add new managers and login
    /// </summary>
    public class ManagerBL : IManagerBL
    {

        private IManagerRepo iManagerRepo;
        public ManagerBL(IManagerRepo newIManagerRepo){
            iManagerRepo = newIManagerRepo;
        }
        public void AddNewManager(Manager Manager){
            iManagerRepo.AddNewManager(Manager);
        }
        public List<Manager> GetManagers(){
            return iManagerRepo.GetManagers();
        }

        public Manager FindManagerOnEmail(string emailAddress){
            return iManagerRepo.GetManagerByEmail(emailAddress);
        }
    }
}