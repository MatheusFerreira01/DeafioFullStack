using Microsoft.AspNetCore.Mvc;
using Shared.Models.DesafioFullStack;
using Shared.Models.DesafioFullStack.Authentication;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
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
        
        bool? isAdmin = (bool?)TempData["IsAdmin"];

        if (isAdmin == null)
        {
            return RedirectToAction("Logout","Authentication");
        }

        return View(isAdmin);

    }
}
