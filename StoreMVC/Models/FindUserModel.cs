using System;
using StoreModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace StoreMVC.Models
{
    public class FindUserModel
    {
        [Required]
        [EmailAddress]
        [DisplayName("User's Email")]
        public string Email { get; set; }
        
    }
}
