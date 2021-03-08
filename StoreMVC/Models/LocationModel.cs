using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StoreMVC.Models
{
    public class LocationModel : PageModel
    {
        [DisplayName("Location Name")]
        public string Name { get; set; }
        [DisplayName("Location Address")]
        public string Address { get; set; }
    }
}
