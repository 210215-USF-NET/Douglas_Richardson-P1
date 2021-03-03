using System;
using System.Collections.Generic;

#nullable disable

namespace StoreDL.Entities
{
    public partial class LocationTable
    {
        public LocationTable()
        {
            Carts = new HashSet<Cart>();
            Items = new HashSet<Item>();
            OrderTables = new HashSet<OrderTable>();
        }

        public int Id { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }

        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<OrderTable> OrderTables { get; set; }
    }
}
