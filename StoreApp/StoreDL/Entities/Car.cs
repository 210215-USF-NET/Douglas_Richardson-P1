using System;
using System.Collections.Generic;

#nullable disable

namespace StoreDL.Entities
{
    public partial class Car
    {
        public int Id { get; set; }
        public string CarModel { get; set; }
        public string CarMake { get; set; }
        public int? CarYear { get; set; }
        public double Price { get; set; }
    }
}
