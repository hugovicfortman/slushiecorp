using Microsoft.AspNetCore.Mvc;

namespace slushiecorp.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        public HomeController() { }

        [HttpGet]
        public ActionResult Get()
        {
            return File("~/index.html", "text/html");
        }
    }
}
