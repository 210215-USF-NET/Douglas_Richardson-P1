using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StoreMVC.Models
{
    public class LocationModel : PageModel
    {
        public int Id { get; set; }
        [DisplayName("Location Name")]
        public string Name { get; set; }
        [DisplayName("Location Address")]
        public string Address { get; set; }

        public const string SessionKeyLocation = "_LocationID";
        public string Session_LocationID { get; private set; }

        public void OnGet()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyLocation)))
            {
                //HttpContext.Session.SetInt32(SessionKeyLocation, Id);
            }
            //Id = (int) HttpContext.Session.GetInt32(SessionKeyLocation);
        }
        
    }
}
