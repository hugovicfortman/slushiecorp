
using Microsoft.AspNetCore.Mvc;
using slushiecorp.Models;
using slushiecorp.Services;

namespace slushiecorp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly StatsService statsService;

        public StatsController(StatsService statsService)
        {
            this.statsService = statsService;
        }

        [HttpGet]
        public ActionResult<Stats> GetStatistics()
        {
            return statsService.getStatistics();
        }
    }
}