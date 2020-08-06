using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using slushiecorp.Data;
using slushiecorp.Hubs;
using slushiecorp.Models;

namespace slushiecorp.Services
{
    public class OrdersService
    {
        private readonly slushiecorpContext _context;
        private readonly SlushieHub _hub;

        public OrdersService(slushiecorpContext context, SlushieHub hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task<IEnumerable<Order>> getOpenOrders()
        {
            return await _context.Order.Include(c => c.Customer)
                .Where(c => c.OrderState == Enums.OrderStates.New)
                .ToListAsync();
        }

        public async Task<Order> getOrder(int id)
        {
            return await _context.Order.Include(c => c.Customer)
                .FirstOrDefaultAsync(o => o.OrderID == id);
        }

        public async Task updateOrder(Order order)
        {

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(order.OrderID))
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<Order> addOrder(Order order)
        {
            _context.Order.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> deleteOrder(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return null;
            }
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<Order> findOpenOrderByCustomer(int customerID)
        {
            return await _context.Order
                .Where(o => o.CustomerID == customerID && o.OrderState == Enums.OrderStates.New)
                .FirstOrDefaultAsync();
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }
    }
}

