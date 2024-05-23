using Microsoft.AspNetCore.Mvc;
using Shared.Models.DesafioFullStack;
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
        List<DeviceSelector> selectorDevicesList = new List<DeviceSelector>();
        var client = _httpClientFactory.CreateClient("ApiClient");

        var authToken = HttpContext.Session.GetString("AuthenticationToken");
        if (authToken != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }

        var response = await client.GetAsync(CommonApi.DesafioFSApiUrl+ CommonApi.DesafioFSGetListDevicesRoute);
        
        if (response.IsSuccessStatusCode)
        {
            var devices = await response.Content.ReadFromJsonAsync<IEnumerable<Device>>();
            
            foreach (var device in devices)
            {
                selectorDevicesList.Add(new DeviceSelector()
                {
                    Identifier = device.Identifier,
                    Description = device.Description,
                    Manufacturer = device.Manufacturer,
                    Url = device.Url,
                    IsSelected = false
                });
            }            
            return View(devices);
        }

        return View();
    }
}
