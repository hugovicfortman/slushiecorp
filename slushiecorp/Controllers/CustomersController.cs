using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using slushiecorp.Models;
using slushiecorp.Services;
using slushiecorp.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace slushiecorp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomersService customersService;
        private readonly StatsService statsService;
        private readonly SlushieHub slushieHub;

        public CustomersController(CustomersService customersService, StatsService statsService, SlushieHub slushieHub)
        {
            this.customersService = customersService;
            this.statsService = statsService;
            this.slushieHub = slushieHub;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return Ok(await customersService.getCustomers());
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await customersService.getCustomer(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {

            if (id != customer.CustomerID)
            {
                return BadRequest();
            }

            try
            {
                await customersService.updateCustomer(customer);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }catch(Exception)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Customers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            Customer _customer = await customersService.addCustomer(customer);
            var stats = statsService.getStatistics();
            await slushieHub.Clients.All.SendAsync("customersupdated", _customer);
            await slushieHub.Clients.All.SendAsync("statsupdated", stats);
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerID }, _customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await customersService.deleteCustomer(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }

    }
}
