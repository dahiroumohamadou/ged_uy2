using Microsoft.AspNetCore.Mvc;

namespace GED_APP.Controllers
{
    public class DocumentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
