using System;
using StoreModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace StoreModels
{
    public class OrderHistoryModel
    {
        [DisplayName("Location")]
        public string LocationName { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Order Total")]
        public double Total { get; set; }
        public int Id { get; set; }
        public int LocationId { get; set; }
        public string Email { get; set; }
    }
}
