using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DesafioFullStack
{
    public class Device
    {
        public string Identifier { get; set; }

        public string Description { get; set; }

        public string Manufacturer { get; set; }

        public string Url { get; set; }
        public List<CommandDescription> Commands { get; set; }
    }
}
