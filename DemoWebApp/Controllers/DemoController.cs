using System.ComponentModel.Composition;
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
            var demoService = new DemoSystem();
            demoService.RunDemo(demoName);

            return RedirectToRoute("DumpUrl");
        }

    }
}
