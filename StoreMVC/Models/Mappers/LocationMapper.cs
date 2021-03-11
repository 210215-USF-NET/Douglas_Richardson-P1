using StoreModels;
namespace StoreMVC.Models.Mappers
{
    public class LocationMapper
    {
        public ManagerLocationModel castManagerLocationModel(Location location)
        {
            return new ManagerLocationModel
            {
                Address = location.Address,
                Id = location.Id,
                Name = location.LocationName
            };
        }
        public Location castLocation(CreateLocationModel managerLocationModel)
        {
            return new Location
            {
                LocationName = managerLocationModel.Name,
                Address = managerLocationModel.Address
            };
        }
        public EditLocationModel castEditLocationModel(Location location)
        {
            return new EditLocationModel
            {
                Id = location.Id,
                Name = location.LocationName,
                Address = location.Address
            };
        }
        public Location castLocationFromEditLocationModel(EditLocationModel editLocationModel)
        {
            return new Location { 
                Id = editLocationModel.Id,
                LocationName = editLocationModel.Name,
                Address = editLocationModel.Address
            };
        }

        public LocationModel castLocationModel(Location location)
        {
            return new LocationModel
            {
                Address = location.Address,
                Id = location.Id,
                Name = location.LocationName
            };
        }
    }
}
