using API.DesafioFullStack.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Models.DesafioFullStack;

namespace API.DesafioFullStack.Controllers;

public class UserLoginController : ControllerBase
{

    private readonly IUserConfigInterface _userConfigInterface;

    public UserLoginController(IUserConfigInterface userConfigInterface)
    {
        _userConfigInterface = userConfigInterface;
    }

    [HttpGet("GetList")]
    public IActionResult GetUsers()
    {
        try
        {
            var users = _userConfigInterface.GetList();

            if (users is null)
            {
                return NotFound("Nenhum usuário encontrado.");
            }
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro grave: " + ex.Message);
        }
    }

    [HttpGet("GetByUserName")]
    public IActionResult GetUserById(string UserName)
    {
        try
        {
            var users = _userConfigInterface.GetUserByUserName(UserName);

            if (users is null)
            {
                return NotFound("Nenhum usuário encontrado.");
            }
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro grave: " + ex.Message);
        }
    }
}
