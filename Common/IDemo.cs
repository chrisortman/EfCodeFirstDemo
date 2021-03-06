using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace DemoWebApp.Models
{   
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class,AllowMultiple = false)]
    public class ExportDemoAttribute : ExportAttribute
    {
        public ExportDemoAttribute(string demoName) : base(typeof(IDemo))
        {
            DemoName = demoName;
        }

        public string DemoName { get; set; }
    }

    public interface IDemo
    {
        IEnumerable<string> Run();
    }

    public interface IDemoMetadata
    {
        string DemoName { get; }
    }
}