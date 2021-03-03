using StoreModels;
namespace StoreBL
{
    public interface IUserBL
    {
        bool LogUserIn { get; set; }
        bool IsUserManager{ get; set;}
        Location UserLocation{get;set;}
        int? CartID{get;set;}
        //void LogUserIn();
    }
}