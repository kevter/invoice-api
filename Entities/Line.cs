using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.Entities
{
    public class Line
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Tax_rate { get; set; }
        public decimal Discount_rate { get; set; }
        public string Currency { get; set; }
    }
}
