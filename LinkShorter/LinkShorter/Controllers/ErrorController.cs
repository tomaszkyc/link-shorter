using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LinkShorter.Controllers
{
    [AllowAnonymous]
    [Route("Error")]
    public class ErrorController : Controller
    {
        private readonly ILogger _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }


        [HttpGet("{statusCode}")]
        public IActionResult Index(int? statusCode)
        {
            if ( statusCode == null)
            {
                return View();
            }

            switch(statusCode)
            {
                case 404:
                    return RedirectToAction("PageNotFound");
                default:
                    return View();
            }
        }

        public IActionResult PageNotFound()
        {
            return View();
        }


    }
}
