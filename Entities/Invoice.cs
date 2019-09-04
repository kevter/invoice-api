using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PruebaTecnica.Entities
{
    public class Invoice
    {
        public int id { get; set; }
        public Client Client { get; set; }
        public List<Line> Lines { get; set; }
        public decimal Tax_total { get; set; }
        public decimal Discount_total { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public List<Payment> Payments { get; set; }
        public decimal Balance { get; set; }
    }
}
