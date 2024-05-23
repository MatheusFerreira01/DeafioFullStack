using API.DesafioFullStack.Interface;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.DataBase.DesafioFullStack;
using Shared.Models.DesafioFullStack;
using System.Globalization;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace API.DesafioFullStack.Services
{
    public class DeviceRespository : IDeviceInterface
    {
        public List<Device>? GetListDevices()
        {
            List<Device> getDevices = BaseConfigurations.LoadDeviceConfigs();

            if (getDevices is not null && getDevices.Count > 0)
            {
                return getDevices;
            }

            return null;
        }

        public Device? AddNewDevice(Device NewDevice)
        {
            List<Device> devicesExistents = BaseConfigurations.LoadDeviceConfigs();

            if (NewDevice is not null)
            {
                var lastDevice = devicesExistents.Last();
                NewDevice.Url = lastDevice.Url.Split(':')[0] + ":" + (Convert.ToInt32(lastDevice.Url.Split(':')[1]) + 1).ToString();

                devicesExistents.Add(NewDevice);

                WriteDevicesToCsv(devicesExistents, BaseConfigurations.BaseFilesDevicesPath);

                devicesExistents = BaseConfigurations.LoadDeviceConfigs();

                Device deviceRegister = devicesExistents.FirstOrDefault(x => x.Identifier == NewDevice.Identifier);

                return deviceRegister;
            }

            return null;

        }

        public Device? GetDeviceById(string Identifier)
        {
            List<Device> getDevices = BaseConfigurations.LoadDeviceConfigs();

            Device getDeviceByIdentifier = getDevices.FirstOrDefault(x => x.Identifier == Identifier);

            if (getDeviceByIdentifier != null)
                return getDeviceByIdentifier;

            return null;
        }

        public Device? UpdateDevice(string Identifier, Device UpdateValues)
        {
            List<Device> getDevices = BaseConfigurations.LoadDeviceConfigs();

            Device getDeviceUpdate = getDevices.FirstOrDefault(x => x.Identifier == Identifier);

            if (getDeviceUpdate == null)
            {
                return null;
            }

            getDeviceUpdate.Identifier = Identifier;
            getDeviceUpdate.Manufacturer = UpdateValues.Manufacturer;
            getDeviceUpdate.Description = UpdateValues.Description;
            getDeviceUpdate.Url = UpdateValues.Url;

            WriteDevicesToCsv(getDevices, BaseConfigurations.BaseFilesDevicesPath);

            getDevices = BaseConfigurations.LoadDeviceConfigs();

            Device deviceUpdated = getDevices.FirstOrDefault(x => x.Identifier == Identifier);

            return deviceUpdated;
        }

        public Device? DeleteDevice(string Identifier)
        {
            List<Device> getDevices = BaseConfigurations.LoadDeviceConfigs();

            Device getDeviceToDelete = getDevices.FirstOrDefault(x => x.Identifier == Identifier);

            if (getDeviceToDelete == null)
            {
                return null;
            }

            getDevices.Remove(getDeviceToDelete);

            WriteDevicesToCsv(getDevices, BaseConfigurations.BaseFilesDevicesPath);

            return getDeviceToDelete;
        }

        private static void WriteDevicesToCsv(List<Device> devices, string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                Encoding = System.Text.Encoding.UTF8,
                NewLine = Environment.NewLine
            };

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, config))
            {
                csv.WriteRecords(devices);
                csv.NextRecord();
            }
        }
    }
}
