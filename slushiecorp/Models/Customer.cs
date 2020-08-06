using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using slushiecorp.Enums;

namespace slushiecorp.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int Satisfaction { get; set; }
        public int SlushieLevel { get; set; }
        public CustomerStates CustomerState { get; set; }
        public int ConsumptionRate { get; set; }
        public List<Order> Orders { get; set; }
    }
}
