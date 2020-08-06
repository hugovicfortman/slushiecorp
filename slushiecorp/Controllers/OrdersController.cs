using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using slushiecorp.Models;
using slushiecorp.Services;
using slushiecorp.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace slushiecorp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersService ordersService;
        private readonly CustomersService customersService;
        private readonly StatsService statsService;
        private readonly SlushieHub slushieHub;

        public OrdersController(OrdersService ordersService, 
            StatsService statsService, 
            CustomersService customersService,
            SlushieHub slushieHub)
        {
            this.ordersService = ordersService;
            this.statsService = statsService;
            this.customersService = customersService;
            this.slushieHub = slushieHub;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await ordersService.getOpenOrders();
            return Ok(orders);
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await ordersService.getOrder(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderID)
            {
                return BadRequest();
            }
            
            try
            {
                await ordersService.updateOrder(order);
                var stats = statsService.getStatistics();
                var _order = ordersService.getOrder(order.OrderID);
                await slushieHub.Clients.All.SendAsync("ordersupdated", _order);
                await slushieHub.Clients.All.SendAsync("statsupdated", stats);

                // Update the customer who created the order
                var customer = await customersService.getCustomer(order.CustomerID);
                switch(order.OrderState)
                {
                    case Enums.OrderStates.Accepted:
                    {
                        customer.CustomerState = Enums.CustomerStates.Consuming;
                        customer.SlushieLevel = 100;
                        await customersService.updateCustomer(customer);
                        break;
                    }
                    case Enums.OrderStates.Rejected:
                    {
                        customer.CustomerState = Enums.CustomerStates.Rejected;
                        customer.Satisfaction = 0;
                        await customersService.updateCustomer(customer);
                        break;
                    }
                    default:
                        // Do nothing...
                        break;

                }
                await slushieHub.Clients.All.SendAsync("customersupdated", customer);
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
            catch(Exception)
            {
                throw;
            }

            return NoContent();
        }

        // POST: api/Orders
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            // If customer already has an open order, don't create another
            var existingOrder = await ordersService.findOpenOrderByCustomer(order.CustomerID);
            if(existingOrder != null)
            {
                return StatusCode((int) HttpStatusCode.Conflict, existingOrder);
            }

            // Update customer state to ordering
            await customersService.updateCustomer(order.Customer);

            // Otherwise continue to create order
            var _order = await ordersService.addOrder(new Order (){ 
                OrderState = Enums.OrderStates.New,
                CustomerID = order.CustomerID
            });


            // We need to get the (full) order back from context with the customer information.
            _order = await ordersService.getOrder(_order.OrderID);
            var stats = statsService.getStatistics();

            await slushieHub.Clients.All.SendAsync("ordersupdated", _order);
            await slushieHub.Clients.All.SendAsync("customersupdated", _order.Customer);
            await slushieHub.Clients.All.SendAsync("statsupdated", stats);
            return CreatedAtAction("GetOrder", new { id = order.OrderID }, _order);
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            var order = await ordersService.deleteOrder(id);
            if (order == null)
            {
                return NotFound();
            }
            return order;
        }

    }
}
