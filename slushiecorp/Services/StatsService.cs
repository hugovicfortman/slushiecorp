using slushiecorp.Data;
using slushiecorp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace slushiecorp.Services
{
    public class StatsService
    {
        private readonly slushiecorpContext _context;
        public StatsService(slushiecorpContext context)
        {
            _context = context;
        }

        public Stats getStatistics()
        {
            var customerCount = _context.Customer.Count();
            var customerSatisfaction = _context.Customer.Sum(s => s.Satisfaction)/customerCount;
            var openOrders = _context.Order.Where(o => o.OrderState == Enums.OrderStates.New).Count();
            var totalSlushiesMade = _context.Order.Where(o => o.OrderState == Enums.OrderStates.Accepted).Count();
            var totalOrdersMade = _context.Order.Count();
            return new Stats()
            {
                Customers = customerCount,
                CustomerSatisfaction = customerSatisfaction,
                OpenOrders = openOrders,
                TotalSlushiesMade = totalSlushiesMade,
                TotalOrdersMade = totalOrdersMade
            };
        }
    }
}
