using API.DesafioFullStack.Interface;
using Shared.DataBase.DesafioFullStack;
using Shared.Models.DesafioFullStack;

namespace API.DesafioFullStack.Repository;

public class UserConfigRepository : IUserConfigInterface
{
    public List<User>? GetList()
    {
        List<User> getUsers = BaseConfigurations.LoadContentUsers();

        if (getUsers is not null && getUsers.Count > 0)
        {
            return getUsers;
        }

        return null;
    }

    public User? GetUserByUserName(string UserName)
    {
        List<User> getUsers = BaseConfigurations.LoadContentUsers();

        User getUserUserNameId = getUsers.FirstOrDefault(x => x.Username == UserName);

        if (getUserUserNameId != null)
            return getUserUserNameId;

        return null;

    }
}
