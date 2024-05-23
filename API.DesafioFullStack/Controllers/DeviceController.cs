using System.Net.Sockets;
using System.Text;
using API.DesafioFullStack.Interface;
using API.DesafioFullStack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DataBase.DesafioFullStack;
using Shared.Models.DesafioFullStack;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class DeviceController : ControllerBase
    {

        private readonly IDeviceInterface _deviceInterface;

        public DeviceController(IDeviceInterface deviceInterface)
        {
            _deviceInterface = deviceInterface;
        }

        /// <summary>
        /// Obtém a lista de dispositivos.
        /// </summary>
        /// <returns>Uma lista de dispositivos.</returns>
        /// <response code="200">Requisição executada com sucesso</response>
        /// <response code="401">As credenciais fornecidas pelo usuário são inexistentes ou inválidas</response>

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Device>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetListDevices()
        {
            try
            {
                var devices = _deviceInterface.GetListDevices();

                if (devices is null)
                {
                    return NotFound("Nenhum dispositivo encontrado.");
                }
                return Ok(devices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro grave: " + ex.Message);
            }
        }

        [Authorize]
        [HttpPost("{Id}")]
        [ProducesResponseType(typeof(IEnumerable<Device>), 200)]
        [ProducesResponseType(500)]
        public IActionResult AddNewDevice([FromBody] Device newDevice)
        {
            try
            {
                var device = _deviceInterface.AddNewDevice(newDevice);

                if (device is null)
                {
                    return NotFound("Não foi possivel cadastrar um novo dispositivo");
                }
                return Ok(device);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro grave: " + ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(IEnumerable<Device>), 200)]
        [ProducesResponseType(500)]
        public IActionResult GetDevice(string Identifier)
        {
            try
            {
                var device = _deviceInterface.GetDeviceById(Identifier);

                if (device is null)
                {
                    return NotFound("Dispositivo não foi encontrado");
                }
                return Ok(device);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro grave: " + ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{Id}")]
        [ProducesResponseType(typeof(IEnumerable<Device>), 200)]
        [ProducesResponseType(500)]
        public IActionResult UpdateDevice(string Identifier, [FromBody] Device UpdateValues)
        {
            try
            {
                var device = _deviceInterface.UpdateDevice(Identifier, UpdateValues);

                if (device is null)
                {
                    return NotFound("Dispositivo não foi encontrado");
                }

                return Ok(device);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro grave: " + ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{Id}")]
        [ProducesResponseType(typeof(IEnumerable<Device>), 200)]
        [ProducesResponseType(500)]
        public IActionResult DeleteDeviceDetais(string Identifier)
        {
            try
            {
                var device = _deviceInterface.DeleteDevice(Identifier);

                if (device is null)
                {
                    return NotFound("Dispositivo não foi encontrado");
                }

                return Ok(device);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpGet("SendCommand")]
        [ProducesResponseType(typeof(IEnumerable<Device>), 200)]
        [ProducesResponseType(500)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> SendCommand(string Id, int Port, string Command)
        {
            try
            {
                using (TcpClient client = new TcpClient(Id, Port))
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = Encoding.ASCII.GetBytes(Command + "\n");
                    await stream.WriteAsync(data, 0, data.Length);

                    byte[] responseData = new byte[1024];
                    int bytes = await stream.ReadAsync(responseData, 0, responseData.Length);
                    string response = Encoding.ASCII.GetString(responseData, 0, bytes);

                    return Ok(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
