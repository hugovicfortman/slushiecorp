using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using slushiecorp.Enums;

namespace slushiecorp.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public OrderStates OrderState { get; set; }
        [NotMapped]
        public bool IsNewCustomer
        {
            get
            {
                if(Customer != null)
                {
                    return Customer.CustomerState == CustomerStates.New;
                }
                return false;
            }
        }
        public Customer Customer { get; set; }
    }
}
