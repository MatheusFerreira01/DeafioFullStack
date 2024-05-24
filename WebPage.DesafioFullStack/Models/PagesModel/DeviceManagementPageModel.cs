using Shared.Models.DesafioFullStack;

namespace WebPage.DesafioFullStack.Models
{
    public class DeviceManagementPageModel
    {
        public string Identifier { get; set; }

        public string Description { get; set; }

        public string Manufacturer { get; set; }

        public bool IsCreated { get; set; } 

        public List<Device> ListSelectDevices { get; set; } 

        public string SelectedDevice { get; set; }
    }
}
