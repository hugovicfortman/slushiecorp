using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace slushiecorp.Models
{
    // This model does not require a data migration as it is generated on the fly.
    [NotMapped]
    public class Stats
    {
        public int OpenOrders { get; set; }
        public int TotalOrdersMade { get; set; }
        public int TotalSlushiesMade { get; set; }
        public int Customers { get; set; }
        public double CustomerSatisfaction { get; set; }
    }
}
