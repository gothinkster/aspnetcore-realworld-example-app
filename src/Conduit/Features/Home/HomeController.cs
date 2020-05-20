using Microsoft.AspNetCore.Mvc;

namespace Conduit.Features.Home
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}