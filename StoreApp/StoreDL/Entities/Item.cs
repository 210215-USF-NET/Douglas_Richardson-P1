using System;
using System.Collections.Generic;

#nullable disable

namespace StoreDL.Entities
{
    public partial class Item
    {
        public Item()
        {
            Carts = new HashSet<Cart>();
            OrderTables = new HashSet<OrderTable>();
        }

        public int Id { get; set; }
        public int? Quantity { get; set; }
        public int? LocationId { get; set; }
        public int? ProductId { get; set; }

        public virtual LocationTable Location { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderTable> OrderTables { get; set; }
    }
}
