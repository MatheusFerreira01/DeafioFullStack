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
        /// <response code="500">Erro interno no servidor</response>
        [Authorize]
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
        /// <summary>
        /// Cadastra um novo dispositivo na plataforma.
        /// </summary>
        /// <param name="NewDevice">Detalhes do dispositivo sendo cadastrados</param>
        /// <returns>Detalhes do dispositivo sendo cadastrados.</returns>
        /// <response code="200">Requisição executada com sucesso</response>
        /// <response code="401">As credenciais fornecidas pelo usuário são inexistentes ou inválidas</response>
        /// <response code="500">Erro interno no servidor</response>
        
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Device>), 200)]
        public IActionResult AddNewDevice([FromBody] Device NewDevice)
        {
            try
            {
                var device = _deviceInterface.AddNewDevice(NewDevice);

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

        /// <summary>
        /// Retorna os detalhes de um dispositivo.
        /// </summary>
        /// <param name="Identifier">Identificador do dispositivo para o qual os detalhes devem ser retornados</param>
        /// <returns>Uma lista de dispositivos.</returns>
        /// <response code="200">Requisição executada com sucesso</response>
        /// <response code="401">As credenciais fornecidas pelo usuário são inexistentes ou inválidas</response>
        /// <response code="500">Erro interno no servidor</response>
        
        [Authorize]
        [HttpGet("{Id}")]
        [ProducesResponseType(typeof(IEnumerable<Device>), 200)]
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

        /// <summary>
        /// Atualiza os dados de um dispositivo.
        /// </summary>
        /// <returns>Uma lista de dispositivos.</returns>
        /// <param name="Identifier">Identificador do dispositivo para o qual os detalhes devem ser atualizados</param>
        /// <param name="UpdateValues">Detalhes atualizados do dispositivo</param>
        /// <response code="200">Requisição executada com sucesso</response>
        /// <response code="401">As credenciais fornecidas pelo usuário são inexistentes ou inválidas</response>
        /// <response code="500">Erro interno no servidor</response>
        
        [Authorize]
        [HttpPut("{Id}")]
        [ProducesResponseType(typeof(IEnumerable<Device>), 200)]
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
        /// <summary>
        /// Remove os dados de um dispositivo.
        /// </summary>
        /// <param name="Identifier">Identificador do dispositivo para o qual os detalhes devem ser removidos</param>
        /// <returns>Uma lista de dispositivos.</returns>
        /// <response code="200">Requisição executada com sucesso</response>
        /// <response code="401">As credenciais fornecidas pelo usuário são inexistentes ou inválidas</response>
        /// <response code="500">Erro interno no servidor</response>
        
        [Authorize]
        [HttpDelete("{Id}")]
        [ProducesResponseType(typeof(IEnumerable<Device>), 200)]
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
    }
}
