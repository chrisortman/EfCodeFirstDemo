using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;

namespace DemoWebApp.Models
{
    public class DemoSystem
    {
        private ComposablePartCatalog _catalog;
        private CompositionContainer _container;

        public DemoSystem()
        {
            _catalog = new DirectoryCatalog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"bin"));
            _container = new CompositionContainer(_catalog);
        }

        public IEnumerable<string> ListDemos()
        {            
            var exports = _container.GetExports<IDemo, IDemoMetadata>(null);

            foreach(var exp in exports)
            {
                yield return exp.Metadata.DemoName;
            }

        }

        public IEnumerable<string> RunDemo(string demoName)
        {
            var demos = _container.GetExports<IDemo,IDemoMetadata>();
            var demo = demos.First(x => x.Metadata.DemoName == demoName);
            return demo.Value.Run();
        }
    }
}