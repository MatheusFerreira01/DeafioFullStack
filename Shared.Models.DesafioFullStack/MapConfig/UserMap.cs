using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DesafioFullStack;

public class UserMap : ClassMap<User>
{
    public UserMap()
    {
        Map(m => m.FullName).Name("FullName");
        Map(m => m.Username).Name("Username");
        Map(m => m.Password).Name("Password");
        Map(m => m.Profile).Name("Profile");
    }
}
