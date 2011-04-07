using System.Collections.Generic;
using DemoWebApp.Controllers;

namespace DemoWebApp.Models
{
    public class DumpModel
    {
        public IEnumerable<string> Demos { get; set; }
        public IEnumerable<TableModel> Tables { get; set; }
    }
}