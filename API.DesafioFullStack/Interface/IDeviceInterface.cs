using Microsoft.AspNetCore.Mvc;
using Shared.Models.DesafioFullStack;

namespace API.DesafioFullStack.Interface;

public interface IDeviceInterface
{
    List<Device>? GetListDevices();
    Device? AddNewDevice(Device NewDevice);
    Device? GetDeviceById(string Identifier);
    Device? UpdateDevice(string Identifier, Device UpdateValues);
    Device? DeleteDevice(string Identifier);
}
