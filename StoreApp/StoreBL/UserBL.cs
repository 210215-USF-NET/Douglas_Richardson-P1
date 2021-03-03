using StoreModels;
namespace StoreBL
{
    public class UserBL : IUserBL
    {
        public bool LogUserIn { get; set; }
        public bool IsUserManager { get; set; }
        public int? CartID{get;set;}
        public Location UserLocation { get; set; }
    }
}