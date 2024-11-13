using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
