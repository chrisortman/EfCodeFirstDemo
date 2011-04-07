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
            var errorMessages = demoService.RunDemo(demoName);
            TempData["errorMessages"] = errorMessages;

            return RedirectToRoute("DumpUrl");
        }

    }
}
