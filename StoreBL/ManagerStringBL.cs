using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreDL;
using Serilog;
namespace StoreBL
{
    public class ManagerStringBL
    {
        private ManagerStringRepo managerStringRepo;
        public ManagerStringBL(ManagerStringRepo managerStringRepo)
        {
            this.managerStringRepo = managerStringRepo;
        }
        public string GetManagerString(string name) {
            return managerStringRepo.GetManagerString(name);
        }

        public void UserWasMadeManager(string email)
        {
            Log.Information("The user at "+email+" was made a manager.");
        }
    }
}
