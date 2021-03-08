using System;
using StoreModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace StoreMVC.Models
{
    public class CreateLocationModel
    {
        [DisplayName("Location Name")]
        [Required]
        public string Name { get; set; }
        [DisplayName("Location Address")]
        [Required]
        public string Address { get; set; }
    }
}
