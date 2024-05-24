using Microsoft.AspNetCore.Mvc;
using WebPage.DesafioFullStack.Integration;
using WebPage.DesafioFullStack.Models;
public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        string urlAnterior = Request.Headers["Referer"].ToString();

        var pageModel = new HomePageModel()
        {
            FullName = UserManagerIntegration.FullName,
            IsAdmin = UserManagerIntegration.IsAdmin
        };

        if (urlAnterior.Contains("Login") && UserManagerIntegration.Initialized != null)
        {
            return RedirectToAction("Logout", "Authentication");
        }

        UserManagerIntegration.Initialized = true;

        return View(pageModel);
    }
}
