using System.ComponentModel;
namespace StoreMVC.Models
{
    public class ManagerLocationModel
    {
        [DisplayName("Location Id")]
        public int Id { get; set; }
        [DisplayName("Location Name")]
        public string Name { get; set; }
        [DisplayName("Location Address")]
        public string Address { get; set; }
    }
}
