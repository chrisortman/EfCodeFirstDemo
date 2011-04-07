using System.Collections.Generic;
using DemoWebApp.Controllers;

namespace DemoWebApp.Models
{
    public class TableModel
    {
        public string Name { get; set; }
        public ColumnModel[] Columns { get; set; }
        public List<object[]> Data { get; set; }

        public TableModel()
        {
            Data = new List<object[]>();
        }
    }
}