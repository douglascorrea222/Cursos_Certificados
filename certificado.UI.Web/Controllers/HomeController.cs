using Microsoft.AspNetCore.Mvc;
using Certificado.UI.Web.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace Certificado.UI.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            var errorModel = new ErrorModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(errorModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

    }
}
