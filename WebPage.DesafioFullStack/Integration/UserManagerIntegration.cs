using CsvHelper;
using CsvHelper.Configuration;
using Shared.DataBase.DesafioFullStack;
using Shared.Models.DesafioFullStack;
using Shared.Models.DesafioFullStack.Authentication;
using System.Collections.Generic;
using System.Globalization;
using WebPage.DesafioFullStack.Models;

namespace WebPage.DesafioFullStack.Integration
{
    public static class UserManagerIntegration
    {

        public static bool? IsAdmin { get; set; }
        public static string? FullName { get; set; }
        public static bool? Initialized { get; set; }


        public static List<User> GetUsers()
        {
            try
            {
                return BaseConfigurations.LoadContentUsers();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Add(UserManagementPageModel pageModel)
        {
            List<User> users = GetUsers();

            var removeActual = users.Where(x => x.Username == pageModel.Username).FirstOrDefault();

            users.Remove(removeActual);

            var newUser = new User()
            {
                Username = pageModel.Username,
                FullName = pageModel.FullName,
                Password = pageModel.Password,
                Profile = pageModel.Profile
            };

            users.Add(newUser);

            WriteToCsvUsers(users);
        }

        public static void Update(UserManagementPageModel pageModel)
        {
            List<User> users = GetUsers();

            var updateUser = new User()
            {
                Username = pageModel.Username,
                FullName = pageModel.FullName,
                Password = pageModel.Password,
                Profile = pageModel.Profile
            };

            users.Add(updateUser);

            WriteToCsvUsers(users);

        }

        public static void Remove(UserManagementPageModel pageModel)
        {
            List<User> users = GetUsers();
            var userToRemove =  users.Where(x => x.Username == pageModel.Username).FirstOrDefault();

            var removeUser = new User()
            {
                Username = pageModel.Username,
                FullName = pageModel.FullName,
                Password = pageModel.Password,
                Profile = pageModel.Profile
            };

            users.Remove(userToRemove);
            WriteToCsvUsers(users);

        }

        private static void WriteToCsvUsers(List<User> users)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                Encoding = System.Text.Encoding.UTF8,
                NewLine = Environment.NewLine
            };

            using (var writer = new StreamWriter(BaseConfigurations.BaseFilesUsersPath))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(users);
                csv.NextRecord();
            }

        }
    }
}
