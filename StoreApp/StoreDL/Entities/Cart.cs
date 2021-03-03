using System;
using System.Collections.Generic;

#nullable disable

namespace StoreDL.Entities
{
    public partial class Cart
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? Quantity { get; set; }
        public int? LocationId { get; set; }
        public double? Total { get; set; }
        public int? ItemId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Item Item { get; set; }
        public virtual LocationTable Location { get; set; }
    }
}
