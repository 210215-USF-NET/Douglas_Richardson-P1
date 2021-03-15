using System;
using StoreModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace StoreModels
{
    public class OrderHistoryModel
    {
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Order Total")]
        public double Total { get; set; }
    }
}
