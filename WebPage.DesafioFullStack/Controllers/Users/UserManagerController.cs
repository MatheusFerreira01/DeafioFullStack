using Microsoft.AspNetCore.Mvc;

namespace WebPage.DesafioFullStack.Controllers.Users
{
    public class UserManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Teste()
        {
            return View();
        }
    }
}
