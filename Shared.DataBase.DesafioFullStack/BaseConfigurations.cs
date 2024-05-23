using CsvHelper;
using CsvHelper.Configuration;
using Shared.Models.DesafioFullStack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shared.DataBase.DesafioFullStack
{
    public class BaseConfigurations
    {

        private static string baseFilesDevicesPath = GetFilesLocation("Devices.csv");
        public static string BaseFilesDevicesPath
        {
            get => baseFilesDevicesPath;
        }
        private static string baseFilesCommandDescPath = GetFilesLocation("CommandDescriptions.csv");
        public static string BaseFilesCommandDescPath
        {
            get => baseFilesCommandDescPath;
        }
        private static string baseFilesUsersPath = GetFilesLocation("Users.csv");
        public static string BaseFilesUsersPath
        {
            get => baseFilesUsersPath;
        }

        public static string GetFilesLocation(string fileName)
        {
            string originalPath = Directory.GetCurrentDirectory();

            string partToRemove = @"\DeafioFullStack";

            string baseDirectory = originalPath.Substring(0, originalPath.IndexOf(partToRemove) + partToRemove.Length);

            string fullPath = Path.Combine(baseDirectory, "Shared.DataBase.DesafioFullStack\\BaseFiles");

            if (!File.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);

                if (fileName == "Devices.csv") 
                {
                    using (StreamWriter writer = File.CreateText(Path.Combine(fullPath, fileName)))
                    {
                        writer.WriteLine("Identifier;Description;Manufacturer;Url");
                        writer.WriteLine("Dispositivo01;Dispositivo Virtual 01;TestsLTDA;localhost:8001");
                    }
                }
                else if (fileName == "Users.csv")
                {
                    using (StreamWriter writer = File.CreateText(Path.Combine(fullPath, fileName)))
                    {
                        writer.WriteLine("FullName;Username;Password;Profile");
                        writer.WriteLine("Perfil Standard;admin;admin;ADMINISTRADOR");
                    }
                }
                else if(fileName == "CommandDescriptions.csv")
                {
                    using (StreamWriter writer = File.CreateText(Path.Combine(fullPath, fileName)))
                    {
                        writer.WriteLine("Operation;Description;Result;Format");
                        writer.WriteLine("Ler Valor; Realiza o valor de leitura do dispositivo;double; application / json");
                        writer.WriteLine("Estado; Realiza a consulta do estado do dispositivo;boolean; application / json");
                        writer.WriteLine("Reiniciar; Reinicia o dispositivo; string; application / json");
                        writer.WriteLine("Comandos; Retorna a lista de comandos do dispositivo;string; application / json");
                        writer.WriteLine("Descrição; Retorna a descrição do dispositivo;string; application / json");
                    }
                }
            }

            fullPath = Path.Combine(baseDirectory, "Shared.DataBase.DesafioFullStack\\BaseFiles",fileName);

            return fullPath;
        }

        public static List<User> LoadContentUsers()
        {            
            List<User> listUsers;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using (var reader = new StreamReader(BaseFilesUsersPath))

            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<DeviceMap>();
                listUsers = new List<User>(csv.GetRecords<User>());
            }

            return listUsers;

        }
        public static List<Device> LoadDeviceConfigs()
        {
            List<Device> listDevices;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using (var reader = new StreamReader(BaseFilesDevicesPath))

            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<DeviceMap>();
                listDevices = new List<Device>(csv.GetRecords<Device>());
            }

            List<CommandDescription> commandList = LoadCommandDescriptionConfigs();

            listDevices.ForEach(x =>
            {
                x.Commands = commandList;
            });

            return listDevices;
        }

        public static List<CommandDescription> LoadCommandDescriptionConfigs()
        {           

            List<CommandDescription> listCommandDesc;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HeaderValidated = null,
                MissingFieldFound = null
            };
            using (var reader = new StreamReader(BaseFilesCommandDescPath))

            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<CommandDescriptionMap>();
                listCommandDesc = new List<CommandDescription>(csv.GetRecords<CommandDescription>());
            }

            listCommandDesc.ForEach(x =>
            {
                if (x.Operation == "Ler Valor")
                    x.Command.CommandText = "READ";
                else if (x.Operation == "Comandos")
                    x.Command.CommandText = "HELP";
                else if (x.Operation == "Reiniciar")
                    x.Command.CommandText = "RESET";
                else if (x.Operation == "Estado")
                    x.Command.CommandText = "STATUS";
                else
                    x.Command.CommandText = "Desconhecido";
            });

            return listCommandDesc;
        }

    }
}
