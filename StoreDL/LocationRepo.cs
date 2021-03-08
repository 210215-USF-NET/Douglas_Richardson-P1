
using System;
using Model = StoreModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Serilog;
namespace StoreDL
{
    public class LocationRepo
    {
        private CustomerDBContext context;
        public LocationRepo(CustomerDBContext context){
            this.context = context;
        }
        public void AddNewLocation(Model.Location location)
        {
            context.Locations.Add(location);
            context.SaveChanges();
            context.Entry(location).State = EntityState.Detached;
            Log.Information("New location was created. "+location.LocationName);
        }

        public List<Model.Location> GetLocations()
        {
            if(context.Locations.Any(x => x != null)){
                var result = from location in context.Locations select location;
                return result.ToList();
            }else{
                return null;
            }
        }

        public Model.Location GetLocationFromId(int Id)
        {
            var result = from location in context.Locations where location.Id == Id select location;
            return result.FirstOrDefault();
        }

        public void UpdateLocation(Model.Location location)
        {
            Model.Location oldLocation = GetLocationFromId(location.Id);
            context.Entry(oldLocation).CurrentValues.SetValues(location);
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }//class
}
