using Shared.Models.DesafioFullStack;

namespace WebPage.DesafioFullStack.Models
{
    public class DeviceSelector
    {
        public string Identifier { get; set; }

        public string Description { get; set; }

        public string Manufacturer { get; set; }

        public string Url { get; set; }

        public double RainVolume { get; set; }

        public bool IsSelected { get; set; }    
    }
}
