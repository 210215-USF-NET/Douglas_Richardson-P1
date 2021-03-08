using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreDL
{
    public class ManagerStringRepo
    {
        CustomerDBContext context;
        public ManagerStringRepo(CustomerDBContext context)
        {
            this.context = context;
        }
        public string GetManagerString(string name)
        {
            var result = from s in context.ManagerStrings where s.ManagerPhrase.Equals(name) select s;
            return result.FirstOrDefault().ManagerPhrase;
        }
    }
}
