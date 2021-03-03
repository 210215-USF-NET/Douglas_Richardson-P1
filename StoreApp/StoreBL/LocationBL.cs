using StoreDL;
using StoreModels;
using System.Collections.Generic;
namespace StoreBL
{
    /// <summary>
    /// Lets the manager create new locations
    /// </summary>
    public class LocationBL
    {
        LocationRepo locationRepo;
        public LocationBL(LocationRepo newLocationRepo){
            locationRepo = newLocationRepo;
        }
        public void AddNewLocation(Location Location){
            locationRepo.AddNewLocation(Location);
        }
        public List<Location> GetLocations(){
            List<Location> thisLocationList = locationRepo.GetLocations();
            //cannot remove items from a list while iterating using foreach
            if(thisLocationList != null){
                for (int i = thisLocationList.Count - 1; i >= 0; i--)
                {
                    if(thisLocationList[i].LocationName == null){
                        thisLocationList.RemoveAt(i);
                    }        
                }
            }
            
            return thisLocationList;
        }
    }
}