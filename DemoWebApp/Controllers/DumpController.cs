using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SqlMapper;

namespace DemoWebApp.Controllers
{
    public class DumpModel
    {
        public IEnumerable<Table> Tables { get; set; }
    }

    public class DumpController : Controller
    {
        //
        // GET: /Dump/

        public ActionResult Index()
        {
            List<Table> tables = new List<Table>();

            var connectionString = ConfigurationManager.ConnectionStrings["FamilyMembers"].ConnectionString;
            using(var connection = new SqlCeConnection(connectionString))
            {
                connection.Open();

                var schemaTables = connection.ExecuteMapperQuery("select TABLE_NAME from information_schema.tables");
                
                foreach(var schemaTable in schemaTables)
                {
                    if(schemaTable.TABLE_NAME == "EdmMetadata")
                    {
                        continue;                        
                    }
                    var table = new Table()
                    {
                        Name = schemaTable.TABLE_NAME
                    };

                    var columns =
                        connection.ExecuteMapperQuery(
                            "select * from information_schema.columns where TABLE_NAME = @TableName",
                            new {TableName = table.Name});

                    table.Columns = columns.Select(x => new Column()
                    {
                        Name = x.COLUMN_NAME,
                        Type = x.DATA_TYPE + "(" + x.CHARACTER_MAXIMUM_LENGTH + ")",

                    }).ToArray();

                    
                    var dataSql = "select * from " + table.Name;
                    var command = connection.CreateCommand();
                    command.CommandText = dataSql;
                    using(var reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            var data = new object[reader.FieldCount];
                            reader.GetValues(data);
                            table.Data.Add(data);
                        }
                    }

                    tables.Add(table);
                }
                
            }

            
            return View(new DumpModel() { Tables = tables});
        }

    }

    public class Table
    {
        public string Name { get; set; }
        public Column[] Columns { get; set; }
        public List<object[]> Data { get; set; }

        public Table()
        {
            Data = new List<object[]>();
        }
    }

    public class Column
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
