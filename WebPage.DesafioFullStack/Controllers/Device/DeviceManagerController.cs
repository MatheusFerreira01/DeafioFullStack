using Microsoft.AspNetCore.Mvc;

namespace WebPage.DesafioFullStack.Controllers.Device
{
    public class DeviceManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
