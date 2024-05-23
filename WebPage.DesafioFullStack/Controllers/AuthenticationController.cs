using Microsoft.AspNetCore.Mvc;
using Shared.Models.DesafioFullStack.Authentication;
using System.Net.Http.Headers;
using System.Text;
using WebPage.DesafioFullStack.Integration;

namespace WebPage.DesafioFullStack.Controllers;

[Route("[controller]")]
public class AuthenticationController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AuthenticationController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;

    }

    [HttpGet("Login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("SendLogin")]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        //TODO TIRAR DEPOIS
        loginModel.Username = "matheus";
        loginModel.Password = "password";
        if (string.IsNullOrEmpty(loginModel.Username) || string.IsNullOrEmpty(loginModel.Password))
        {
            ViewBag.Error = "Username and password are required.";
            return View();
        }

        var handler = HttpClientHandlerFactory.GetHandler();
        var client = new HttpClient(handler);

        var authToken = Encoding.UTF8.GetBytes($"{loginModel.Username}:{loginModel.Password}");
        var authHeaderValue = Convert.ToBase64String(authToken);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeaderValue);

        var response = await client.GetAsync("https://localhost:7078/Device");
        response.EnsureSuccessStatusCode();

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Invalid credentials.";
            return View();
        }

        HttpContext.Session.SetString("AuthenticationToken", authHeaderValue);
        return RedirectToAction("Index", "Home");
    }   

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("User");
        return RedirectToAction("Login");
    }
}
