using System;
using System.Collections.Generic;

#nullable disable

namespace StoreDL.Entities
{
    public partial class Product
    {
        public Product()
        {
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string ProductName { get; set; }
        public double? Price { get; set; }
        public int? Category { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
