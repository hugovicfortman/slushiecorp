using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using slushiecorp.Data;
using slushiecorp.Hubs;
using slushiecorp.Models;

namespace slushiecorp.Services
{
    public class CustomersService
    {
        private readonly slushiecorpContext _context;
        private readonly SlushieHub _hub;

        public CustomersService(slushiecorpContext context, SlushieHub hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task<IEnumerable<Customer>> getCustomers()
        {
            return await _context.Customer.ToListAsync();
        }

        public async Task<Customer> getCustomer(int id)
        {
            return await _context.Customer.FindAsync(id);
        }

        public async Task updateCustomer(Customer customer)
        {

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customer.CustomerID))
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<Customer> addCustomer(Customer customer)
        {
            _context.Customer.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
            // When a new customer is created, this affects the stats
            //_hub.Clients.All.SendAsync("statsupdated", DataManager.GetData());
        }

        public async Task<Customer> deleteCustomer(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            if(customer == null)
            {
                return null;
            }
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.CustomerID == id);
        }
    }
}
