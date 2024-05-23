using Microsoft.AspNetCore.Mvc;

namespace WebPage.DesafioFullStack.Controllers.Users
{
    public class UserManagerController : Controller
    {
        public IActionResult UserManagement()
        {
            return View();
        }

        public IActionResult AddNew()    
        {

            return View();
        }
        public IActionResult Update()
        {

            return View();
        }

        public IActionResult Delete()
        {

            return View();
        }
    }
}
