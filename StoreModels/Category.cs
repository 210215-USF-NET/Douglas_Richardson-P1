using System.ComponentModel;

namespace StoreModels
{
    /// <summary>
    /// The product category
    /// </summary>
    [DefaultValue(Category.Nothing)]
    public enum Category
    {
        Nothing = 1, Food = 2, Collars = 3, Leashes = 4, Clothes = 5, Beds = 6, Accessories = 7

    }
}