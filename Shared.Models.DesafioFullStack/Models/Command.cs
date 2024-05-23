using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DesafioFullStack
{
    public class Command
    {
        public string CommandText { get; set; }
        public List<Parameter>? Parameters { get; set; } = new List<Parameter>();
    }
}
