using CsvHelper;
using CsvHelper.Configuration;
using Shared.DataBase.DesafioFullStack;
using Shared.Models.DesafioFullStack;
using Shared.Models.DesafioFullStack.Authentication;
using System.Collections.Generic;
using System.Globalization;

namespace WebPage.DesafioFullStack.Integration
{
    public class UserManagerIntegration
    {
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

        public static bool Add(User newUser)
        {
            try
            {
                List<User> users = GetUsers();
                users.Add(newUser);
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
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        public static bool Update(User updateUser)
        {
            try
            {
                List<User> users = GetUsers();
                users.Add(updateUser);

                User? getUserUpdate = users.FirstOrDefault(x => x.Username == updateUser.Username);

                if (getUserUpdate == null)
                {
                    return false;
                }

                getUserUpdate.FullName = updateUser.FullName;
                getUserUpdate.Username = updateUser.Username;
                getUserUpdate.Password = updateUser.Password;
                getUserUpdate.Profile = updateUser.Profile;

                WriteToCsvUsers(users);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Delete(User newUser)
        {
            try
            {
                List<User> users = GetUsers();

                User? getUserToDelete = users.FirstOrDefault(x => x.Username == newUser.Username);

                if (getUserToDelete == null)
                {
                    return false;
                }

                users.Remove(getUserToDelete);
                WriteToCsvUsers(users);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
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
