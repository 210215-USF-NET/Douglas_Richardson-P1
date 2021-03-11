using StoreModels;
using System.Diagnostics;

namespace StoreMVC.Models.Mappers
{
    public class ItemMapper
    {
        public ManagerItemModel castManagerItemModel(Item item)
        {
            if (item.ItemLocation != null) {
                return new ManagerItemModel
                {
                    Quantity = item.Quantity,
                    ItemId = item.Id,
                    LocationName = item.ItemLocation.LocationName,
                    LocationId = item.ItemLocation.Id,
                    ProductName = item.Product.ProductName,
                    Price = item.Product.Price,
                    Category = item.Product.Category,
                    ProductId = item.Product.Id
                };
            }
            return new ManagerItemModel
            {
                Quantity = item.Quantity,
                ItemId = item.Id,
                ProductName = item.Product.ProductName,
                Price = item.Product.Price,
                Category = item.Product.Category,
                ProductId = item.Product.Id
            };

        }
        public Item castItem(CreateItemModel createItemModel)
        {
            return new Item
            {
                Quantity = createItemModel.Quantity,
                Product = new Product
                {

                },
                ItemLocation = new Location
                {
                    
                }
            };
        }
        public EditItemModel castEditItemModel(Item item)
        {
            if (item.ItemLocation != null)
            {
                return new EditItemModel
                {
                    Quantity = item.Quantity,
                    ItemId = item.Id,
                    LocationName = item.ItemLocation.LocationName,
                    LocationId = item.ItemLocation.Id,
                    ProductName = item.Product.ProductName,
                    Price = item.Product.Price,
                    Category = item.Product.Category,
                    ProductId = item.Product.Id
                };
            }
            return new EditItemModel
            {
                Quantity = item.Quantity,
                ItemId = item.Id,
                ProductName = item.Product.ProductName,
                Price = item.Product.Price,
                Category = item.Product.Category,
                ProductId = item.Product.Id
            };
        }
        public Item castItemFromEditItemModel(EditItemModel editItemModel)
        {
            return new Item {
                Id = editItemModel.ItemId,
                Quantity = editItemModel.Quantity,
                Product = new Product { 
                    ProductName = editItemModel.ProductName,
                    Price = editItemModel.Price,
                    Category = editItemModel.Category
                },
                ItemLocation = new Location
                {
                    Id = editItemModel.LocationId
                }
                
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
