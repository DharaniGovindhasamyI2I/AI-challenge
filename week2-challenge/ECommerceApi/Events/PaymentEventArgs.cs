using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApi.Events
{
    public class PaymentEventArgs : EventArgs
    {
        public string Email { get; set; }
        public string Product { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } // "success" or "failure"
    }
}
