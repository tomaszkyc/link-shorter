using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LinkShorter.Models;
using Microsoft.Extensions.Logging;
using LinkShorter.Models.UrlStatistics;
using Microsoft.AspNetCore.Identity;

namespace LinkShorter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILinkRepository _adRepository;

        private readonly ILogger _logger;

        private readonly IUrlStatisticsService _urlStatisticsService;

        private readonly UserManager<IdentityUser> _userManager;


        public HomeController(ILinkRepository adRepository,
            ILogger<HomeController> logger,
            IUrlStatisticsService urlStatisticsService,
            UserManager<IdentityUser> userManager)
        {

            _adRepository = adRepository;
            _logger = logger;
            _urlStatisticsService = urlStatisticsService;
            _userManager = userManager;

        }


        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult GenerateShortUrl(Link newAd)
        {

            
            if ( ModelState.IsValid )
            {


                //dodanie użytkownika, który tworzy link (null w przypadku braku)
                var user = _userManager.GetUserAsync(User);
                newAd.AdOwner = user.Result;
                try
                {
                    _adRepository.Add(newAd);
                }
                catch(Exception exception)
                {
                    return RedirectToAction("GenerationShortUrlFailure");
                }
                SerializingTools.Put<Link>(TempData, "newAd", newAd);
                return RedirectToAction("GenerationShortUrlSuccess");
            }



            return View(newAd);

        }

        public IActionResult GenerationShortUrlFailure()
        {
            return View();
        }

        public IActionResult GenerationShortUrlSuccess()
        {
            Link newAd = SerializingTools.Get<Link>(TempData, "newAd");

            if ( newAd != null )
            {
                return View(newAd);
            }

            return RedirectToAction("Index");
        }




        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> IndexAsync(string shortUrl)
        {

            if ( !String.IsNullOrEmpty( shortUrl ) )
            {
                



                var ad = _adRepository.GetLinkByShortUrl(shortUrl);
                if ( ad != null )
                {
                    await _urlStatisticsService.HandleRequest(ad);


                    return Redirect( ad.RedirectUrl );
                }
                else
                {
                    return NotFound();
                }

            }



            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
