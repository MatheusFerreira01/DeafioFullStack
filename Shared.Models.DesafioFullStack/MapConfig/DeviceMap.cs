using CsvHelper;
using CsvHelper.Configuration;

namespace Shared.Models.DesafioFullStack
{
    public  class DeviceMap : ClassMap<Device>
    {
        public DeviceMap()
        {
            Map(m => m.Identifier).Name("Identifier");
            Map(m => m.Description).Name("Description");
            Map(m => m.Manufacturer).Name("Manufacturer");
            Map(m => m.Url).Name("Url");
        }
    }
}
