﻿using System;
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
        private readonly IAdRepository _adRepository;

        private readonly ILogger _logger;

        private readonly IUrlStatisticsService _urlStatisticsService;

        private readonly UserManager<IdentityUser> _userManager;


        public HomeController(IAdRepository adRepository,
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
        public IActionResult GenerateShortUrl(Ad newAd)
        {
            _logger.LogDebug("Rozpoczynam tworzenie url dla: {url}", newAd.RedirectUrl);

            
            if ( ModelState.IsValid )
            {
                //TODO: Dodać sprawdzanie, czy taki link już istnieje.
                //TODO: Dodać sprawdzanie, czy to nie jest nasz skrócony link


                //dodanie użytkownika, który tworzy link (null w przypadku braku)
                var user = _userManager.GetUserAsync(User);
                newAd.AdOwner = user.Result;
                try
                {
                    _adRepository.Add(newAd);
                }
                catch(Exception exception)
                {
                    _logger.LogError("Problem podczas dodawania linku", exception);
                    return RedirectToAction("GenerationShortUrlFailure");
                }
                SerializingTools.Put<Ad>(TempData, "newAd", newAd);
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
            Ad newAd = SerializingTools.Get<Ad>(TempData, "newAd");

            if ( newAd != null )
            {
                return View(newAd);
            }

            return RedirectToAction("Index");
        }




        [HttpGet("{shortUrl}")]
        public async Task<IActionResult> IndexAsync(string shortUrl)
        {
            _logger.LogDebug("Input shorturl is: {shortUrl}", shortUrl);


            if ( !String.IsNullOrEmpty( shortUrl ) )
            {
                



                var ad = _adRepository.GetAdByShortUrl(shortUrl);
                if ( ad != null )
                {
                    await _urlStatisticsService.HandleRequest(ad);


                    return Redirect( ad.RedirectUrl );
                }
                else
                {
                    return RedirectToAction("GenerationShortUrlFailure");
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
