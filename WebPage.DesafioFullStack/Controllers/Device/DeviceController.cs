using Microsoft.AspNetCore.Mvc;
using Shared.Models.DesafioFullStack;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using WebPage.DesafioFullStack.Integration;
using WebPage.DesafioFullStack.Models;
using System.Text.Json;
using Newtonsoft.Json;
using Shared.DataBase.DesafioFullStack;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Models.DesafioFullStack.Models;

public class DeviceController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DeviceController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    #region DeviceListSelector page
    public ActionResult DeviceListSelector()
    {
        var items = GetItems();
        return View(items);
    }

    private List<DeviceSelector> GetItems()
    {
        var client = _httpClientFactory.CreateClient("ApiClient");

        var authToken = HttpContext.Session.GetString("AuthenticationToken");
        if (authToken != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);
        }

        var response = client.GetAsync(CommonApi.DesafioFSApiUrl + CommonApi.DesafioFSGetListDevicesRoute).Result;

        if (response.IsSuccessStatusCode)
        {
            var jsonString = response.Content.ReadAsStringAsync().Result;
            var devices = JsonConvert.DeserializeObject<List<Device>>(jsonString);

            var listDevicesSelectes = devices.Select(device => new DeviceSelector
            {
                Identifier = device.Identifier,
                Description = device.Description,
                Manufacturer = device.Manufacturer,
                Url = device.Url,
                IsSelected = false
            }).ToList();

            return listDevicesSelectes;
        }

        return new List<DeviceSelector>();
    }
    #endregion

    #region DeviceComunication page    
    [HttpPost]
    public ActionResult ShowDeviceComunication(List<DeviceSelector> devices)
    {
        DeviceCommunicationPageModel pageModel = new DeviceCommunicationPageModel();
        pageModel.RefererLastPage = Request.Headers["Referer"].ToString();
        pageModel.SelectedDevices = devices.Where(x => x.IsSelected).ToList();

        if (string.IsNullOrEmpty(pageModel.ChartLabelsSerialized))
        {
            string[] initialize = [];
            pageModel.ChartLabelsSerialized = JsonConvert.SerializeObject(initialize);
        } 
        SetCommandList(pageModel);

        return View("DeviceComunication", pageModel);
    }

    [HttpPost]
    public ActionResult RealTimeData([FromBody] string pageModel)
    {
        DeviceCommunicationPageModel loadPageModel = JsonConvert.DeserializeObject<DeviceCommunicationPageModel>(pageModel);

        loadPageModel.SelectedCommand = "READ";

        var response = CommunicateDevice(loadPageModel);

        loadPageModel.ResultCommandSender = response.ToString();

        var dataChartSetList = DashBoardLoad(loadPageModel);

        loadPageModel.ChartLabelsSerialized = JsonConvert.SerializeObject(dataChartSetList);

        return Json(new { success = true, loadPageModel.ChartLabelsSerialized });
    }

    [HttpPost]
    public ActionResult SubmitCommand(DeviceCommunicationPageModel pageModel)
    {
        string commandToSend = GetTelnetCommand(pageModel.SelectedCommand);

        pageModel.SelectedCommand = commandToSend;

        var response = CommunicateDevice(pageModel);

        pageModel.ResultCommandSender = response.ToString();

        var dataChartSetList = DashBoardLoad(pageModel);
        
        pageModel.ChartLabelsSerialized = JsonConvert.SerializeObject(dataChartSetList);

        return View("DeviceComunication", pageModel);
    }

    private void SetCommandList(DeviceCommunicationPageModel pageModel)
    {
        List<CommandSelector> commandsSelect = new List<CommandSelector>();
        var commands = BaseConfigurations.LoadCommandDescriptionConfigs();
        foreach (var item in commands)
        {
            commandsSelect.Add(new CommandSelector()
            {
                Operation = item.Operation,
                Description = item.Description,
                Format = item.Format,
                Result = item.Result,
                Tittle = $"{item.Operation} : {item.Description}"
            });
        }
        pageModel.CommandsList = commandsSelect;
    }  

    public IActionResult BackScreen(DeviceCommunicationPageModel pageModel)
    {
        return Redirect(pageModel.RefererLastPage);
    }

    private List<DeviceData> DashBoardLoad(DeviceCommunicationPageModel pageModel)
    {
        List<DeviceData> dataChartSetList = new List<DeviceData>();

        foreach (var device in pageModel.SelectedDevices)
        {
            var dataChartSet = new DeviceData()
            {
                DeviceName = device.Identifier,
                Data = device.RainVolume, 
                Color = "rgba(0, 0, 255)"
            };
            dataChartSetList.Add(dataChartSet);
        }
        return dataChartSetList;
    }
   
    private string  CommunicateDevice(DeviceCommunicationPageModel pageModel)
    {
        string response = "Valores e dispositivos encontrados: \n";

        try
        {
            foreach (var device in pageModel.SelectedDevices)
            {
                var stringWrap = device.Url.Split(':');
                string ip = stringWrap[0];

                if (ip == "localhost")
                    ip = "127.0.0.1";

                int port = Convert.ToInt32(stringWrap[1]);

                using (TcpClient client = new TcpClient(ip, port))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.ASCII.GetBytes(pageModel.SelectedCommand + "\n");
                    stream.WriteAsync(data, 0, data.Length);

                    byte[] responseData = new byte[1024];
                    int bytes = stream.Read(responseData, 0, responseData.Length);
                    string onlyValue = Encoding.ASCII.GetString(responseData, 0, bytes) + "\n";

                    response += onlyValue;

                    if (pageModel.SelectedCommand == "READ")
                    {
                        var splitRainVolume = onlyValue.Split(':')[1].Replace("mm", "").Replace("\n", "");
                        double getRainVolume = Convert.ToDouble(splitRainVolume);
                        device.RainVolume = getRainVolume;
                    }
                    Thread.Sleep(500);
                }
            }
            return response;
        }
        catch (Exception ex)
        {
            return "Erro : "+ex.Message;
        }

    }

    private string GetTelnetCommand(string selectedCommand)
    {
        string commandTelnet;
        if (selectedCommand == "Ler Valor")
            commandTelnet = "READ";
        else if (selectedCommand == "Comandos")
            commandTelnet = "HELP";
        else if (selectedCommand == "Reiniciar")
            commandTelnet = "RESET";
        else if (selectedCommand == "Estado")
            commandTelnet = "STATUS";
        else if (selectedCommand == "Descrição")
            commandTelnet = "CONFIG";
        else
            commandTelnet = "Nada";

        return commandTelnet;
    }
    #endregion
}
