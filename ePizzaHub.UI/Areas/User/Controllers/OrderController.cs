using Microsoft.AspNetCore.Mvc;

namespace ePizzaHub.UI.Areas.User.Controllers
{
    public class OrderController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
