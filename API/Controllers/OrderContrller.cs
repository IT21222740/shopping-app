using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class OrderContrller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
