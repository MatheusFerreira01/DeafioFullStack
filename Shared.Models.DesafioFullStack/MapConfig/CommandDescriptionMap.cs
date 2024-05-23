using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.DesafioFullStack
{
    public class CommandDescriptionMap : ClassMap<CommandDescription>
    {
        public CommandDescriptionMap()
        {
            Map(m => m.Operation).Name("Operation");
            Map(m => m.Description).Name("Description");
            Map(m => m.Result).Name("Result");
            Map(m => m.Format).Name("Format");
        }
    }
}
