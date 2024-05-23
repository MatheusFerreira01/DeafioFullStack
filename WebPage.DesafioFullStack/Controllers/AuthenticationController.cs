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

        var response = await client.GetAsync(CommonApi.DFSApiUrl+ CommonApi.DFSGetListDevicesRoute);       

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Usuario e senha invalidos";
            return View();
        }

        HttpContext.Session.SetString("AuthenticationToken", authHeaderValue);

        var userProfile = UserManagerIntegration.GetUsers().Where(x=> x.Username == loginModel.Username).FirstOrDefault();

        bool isAdmin = false;

        if (userProfile.Profile == "ADMINISTRADOR")
        {
            isAdmin = true;
        }
        TempData["IsAdmin"] = isAdmin;
        TempData["FullName"] = userProfile.FullName ;
        return RedirectToAction("Index","Home");
    }   

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("AuthenticationToken");
        return RedirectToAction("Login");
    }
}
