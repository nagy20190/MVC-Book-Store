using Microsoft.AspNetCore.Mvc;

namespace BKStore_MVC.Controllers
{
    public class ReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
