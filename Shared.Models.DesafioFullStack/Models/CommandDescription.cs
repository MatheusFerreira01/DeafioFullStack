using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DesafioFullStack
{
    public class CommandDescription
    {
        public string Operation { get; set; }
        public string Description { get; set; }
        public Command Command { get; set; } = new Command();
        public string Result { get; set; }
        public string Format { get; set; }
    }
}
