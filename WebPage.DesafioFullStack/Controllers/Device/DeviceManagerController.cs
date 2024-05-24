using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Shared.DataBase.DesafioFullStack;
using Shared.Models.DesafioFullStack;
using Shared.Models.DesafioFullStack.Authentication;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using WebPage.DesafioFullStack.Integration;
using WebPage.DesafioFullStack.Models;

namespace WebPage.DesafioFullStack.Controllers
{
    public class DeviceManagerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DeviceManagerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

        }

        public IActionResult DeviceManagement()
        {
            DeviceManagementPageModel pageModel = new DeviceManagementPageModel();
            var client = _httpClientFactory.CreateClient("ApiClient");

            var authToken = HttpContext.Session.GetString("AuthenticationToken");

            if (authToken != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
            }

            var response = client.GetAsync(CommonApi.DFSApiUrl + CommonApi.DFSGetListDevicesRoute).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;
                var devices = JsonConvert.DeserializeObject<List<Device>>(jsonString);
                pageModel.ListSelectDevices = devices;
            }
            return View(pageModel);
        }

        [HttpPost]
        public IActionResult Add([FromBody] DeviceManagementPageModel pageModel)
        {
            try
            {

                var isNewDevice = ValidateExistence(pageModel);

                if (isNewDevice)
                {
                    var response = SendAsync(CommonApi.DFSApiUrl, CommonApi.DFSPostDeviceRoute, null, pageModel, "POST").Result;

                    return Json(new { success = true, message = "O dispositivo foi cadastrado com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Esse dispositivo já existe" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro: " + ex.Message });
            }

        }
        [HttpPost]
        public IActionResult Edit([FromBody] DeviceManagementPageModel pageModel)
        {
            try
            {
                pageModel.Identifier = pageModel.SelectedDevice;

                var isNewDevice = ValidateExistence(pageModel);

                if (!isNewDevice)
                {
                    var response = SendAsync(CommonApi.DFSApiUrl, CommonApi.DFSPutDeviceRoute, pageModel.Identifier, pageModel, "PUT").Result;

                    return Json(new { success = true, message = "O dispositivo foi alterado com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Dispositivo não encontrado" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro: " + ex.Message });
            }
        }
        [HttpPost]
        public IActionResult Remove([FromBody] DeviceManagementPageModel pageModel)
        {
            try
            {
                pageModel.Identifier = pageModel.SelectedDevice;

                var isNewDevice = ValidateExistence(pageModel);

                if (!isNewDevice)
                {
                    var response = SendAsync(CommonApi.DFSApiUrl, CommonApi.DFSDeleteDeviceRoute, pageModel.Identifier, null, "DELETE").Result;

                    return Json(new { success = true, message = "O dispositivo foi removido com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Dispositivo não encontrado" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Erro: " + ex.Message });
            }
        }

        private bool ValidateExistence(DeviceManagementPageModel pageModel)
        {
            bool isConfirmed = false;

            var client = _httpClientFactory.CreateClient("ApiClient");

            var authToken = HttpContext.Session.GetString("AuthenticationToken");

            if (authToken != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
            }

            var response = client.GetAsync(CommonApi.DFSApiUrl + CommonApi.DFSGetListDevicesRoute).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync().Result;

                var devices = JsonConvert.DeserializeObject<List<Device>>(jsonString);

                if (!devices.Where(x => x.Identifier == pageModel.Identifier).Any())
                {
                    isConfirmed = true;
                }
            }
            return isConfirmed;
        }
        public async Task<string> SendAsync(string baseUrl, string route, string? identifier, DeviceManagementPageModel valuesToJson, string method)
        {
            var client = _httpClientFactory.CreateClient("ApiClient");

            var authToken = HttpContext.Session.GetString("AuthenticationToken");

            if (authToken != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
            }

            string url = baseUrl + route;

            if (!string.IsNullOrEmpty(identifier))
            {
                url += $"?Identifier={identifier}";
            }

            HttpResponseMessage response;
            Device objectJsonStruct = new Device();
            if (!method.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
            {
                objectJsonStruct = new Device()
                {
                    Identifier = valuesToJson.Identifier,
                    Manufacturer = valuesToJson.Manufacturer,
                    Description = valuesToJson.Description,
                    Url = string.Empty,
                    Commands = new List<CommandDescription>
                {
                    new CommandDescription
                    {
                        Operation = string.Empty,
                        Description = string.Empty,
                        Command = new Command
                        {
                            CommandText = string.Empty,
                            Parameters = new List<Parameter>
                            {
                                new Parameter { Name = string.Empty, Description = string.Empty }
                            }
                        },
                        Result = string.Empty,
                        Format = string.Empty
                    }
                }
                };
            }
            string serializedJson = JsonConvert.SerializeObject(objectJsonStruct);

            var content = new StringContent(serializedJson, Encoding.UTF8, "application/json");

            if (method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                response = await client.PostAsync(url, content);
            }
            else if (method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                response = await client.PutAsync(url, content);
            }
            else if (method.Equals("DELETE", StringComparison.OrdinalIgnoreCase))
            {
                response = await client.DeleteAsync(url);
            }
            else
            {
                throw new ArgumentException("Método HTTP inválido.", nameof(method));
            }

            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();

            return result;
        }
    }
}
