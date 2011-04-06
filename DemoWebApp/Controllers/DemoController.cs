using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DemoWebApp.Models;
using SqlMapper;

namespace DemoWebApp.Controllers
{
    public class DemoController : Controller
    {
        //
        // GET: /Demo/

        public ActionResult Index(string demoName)
        {
            var demoService = new DemoService();
            demoService.RunDemo(demoName);

            return RedirectToRoute("DumpUrl");
        }

    }
    
    public class DemoService
    {
        private AssemblyCatalog _catalog;
        private CompositionContainer _container;

        public DemoService()
        {  
            _catalog = new AssemblyCatalog(typeof(IDemo).Assembly);
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

        public void RunDemo(string demoName)
        {
            var demos = _container.GetExports<IDemo,IDemoMetadata>();
            var demo = demos.First(x => x.Metadata.DemoName == demoName);
            demo.Value.Run();
        }
    }
}
