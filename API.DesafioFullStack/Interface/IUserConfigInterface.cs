using Shared.Models.DesafioFullStack;

namespace API.DesafioFullStack.Interface;

public interface IUserConfigInterface
{
    List<User>? GetList();
    User? GetUserByUserName(string UserName);
}
