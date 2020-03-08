using System.Linq;
using System.Threading.Tasks;
using LinkShorter.Models;
using LinkShorter.Models.UrlStatistics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LinkShorter.Models.Dashboard.Statistics;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using LinkShorter.Models.Pagination;


namespace LinkShorter.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class DashboardController : Controller
    {
        private readonly ILinkRepository _linkRepository;

        private readonly ILogger _logger;

        private readonly IUrlStatisticsRepository _urlStatisticsRepository;

        private readonly UserManager<IdentityUser> _userManager;


        public DashboardController(ILinkRepository adRepository,
            ILogger<HomeController> logger,
            IUrlStatisticsRepository urlStatisticsRepository,
            UserManager<IdentityUser> userManager)
        {
            _linkRepository = adRepository;
            _logger = logger;
            _urlStatisticsRepository = urlStatisticsRepository;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return RedirectToAction("Statistics");
        }

        [HttpGet]
        [Route("api/v1/links/check-unique")]
        public async Task<IActionResult> CheckIfShortUrlIsUnique([FromQuery] string shortUrl)
        {
            if ( string.IsNullOrEmpty( shortUrl ) )
            {
                return Ok("Given short url is null or empty.");
            }


            _logger.LogDebug("Checking short url: {0}", shortUrl);
            var linkWithSameShortUrl = await _linkRepository.GetWhere(link => link.ShortUrl == shortUrl);


            if (linkWithSameShortUrl.Any())
            {
                return Ok(false);
            }
            else
            {
                return Ok(true);
            }


        }

        [HttpGet]
        [Route("api/v1/links/rights/can-create-own-short-links")]
        public IActionResult CanUserCreateOwnShortLinks()
        {
            bool hasAccessToCreateUniqueLinks = User.IsInRole("Admin") || User.IsInRole("PowerUser");
            return Ok(hasAccessToCreateUniqueLinks);
        }

        [HttpGet]
        [Route("api/v1/statistics/by-platform")]
        public async Task<IActionResult> GetStatisticsByPlatform()
        {
            var user = await _userManager.GetUserAsync(User);

            var statistics = await _urlStatisticsRepository.GetWhere(res => res.Link.AdOwner != null && res.Link.AdOwner.Id == user.Id);

            var groupedResults = statistics.GroupBy(res => res.OSFamily)
                                           .Select(res => new
                                           {
                                               Label = res.Key,
                                               Count = res.Count()
                                           }).ToList();




            if (groupedResults != null)
            {
                return Ok(groupedResults);
            }
            else
            {
                return View();
            }
        }


        [HttpGet]
        [Route("api/v1/statistics/by-browser")]
        public async Task<IActionResult> GetStatisticsByBrowserCount()
        {
            var user = await _userManager.GetUserAsync(User);

            var statistics = await _urlStatisticsRepository.GetWhere(res => res.Link.AdOwner != null && res.Link.AdOwner.Id == user.Id);

            var groupedResults = statistics.GroupBy(res => res.BrowserFamily)
                                           .Select(res => new
                                           {
                                               Label = res.Key,
                                               Count = res.Count()
                                           }).ToList();



            if (groupedResults != null)
            {
                return Ok(groupedResults);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [Route("api/v1/statistics/actual-month")]
        public async Task<IActionResult> GetStatisticsByActualMonth()
        {
            ViewData["ActivePage"] = "Statistics";

            var user = await _userManager.GetUserAsync(User);

            var statistics = await _urlStatisticsRepository.GetCountFromActualMonthForUserAsync(user.Id);




            var groupedResults = statistics.GroupBy(s => s.EventDate.ToString("yyyy-MM-dd"))
                                            .Select(s => new DataPoint(s.Count(), s.Key)).OrderBy(x => x._category).ToList();

            if (groupedResults != null)
            {
                return Ok(groupedResults);
            }
            else
            {
                return View();
            }
        }

        [Route("Statistics")]
        public ViewResult Statistics()
        {
            ViewData["ActivePage"] = "Statistics";

            return View();
        }

        [Route("Users")]
        public RedirectToActionResult Users()
        {
            ViewData["ActivePage"] = "Users";

            return RedirectToAction("Index", "User");
        }


        //links section start

        [Route("Links")]
        [Route("Links/{pageId}")]
        public async Task<IActionResult> Links(int? pageId, int? pageSize)
        {
            ViewData["ActivePage"] = "Links";
            if (pageId == null)
            {
                pageId = 0;
            }

            if (pageSize == null)
            {
                pageSize = 10;
            }

            //get user id and check if isn't null
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return null;
            }

            //get link list
            var links = _linkRepository.GetAllUserLinks(user);



            //create pagination
            var paginatedLinks = new PaginatedList<Link>(links, (int)pageId, (int)pageSize);

            return View(paginatedLinks);
        }


        [Route("Links/Details/{id}")]
        public async Task<IActionResult> LinkDetails(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //check if user has access start
            var user = await _userManager.GetUserAsync(User);
            var hasAccess = await _linkRepository.UserHasAccessToLink(user, (long)id);
            if ( !hasAccess )
            {
                return Forbid();
            }
            //check if user has access stop

            var link = await _linkRepository.FirstOrDefailtAsync(m => m.Id == id);
            if (link == null)
            {
                return NotFound();
            }

            return View("~/Views/Dashboard/Links/Details.cshtml", link);
        }

        // POST: Link/Delete/5
        [Route("Links/Delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            //check if user has access start
            var user = await _userManager.GetUserAsync(User);
            var hasAccess = await _linkRepository.UserHasAccessToLink(user, (long)id);
            if (!hasAccess)
            {
                return Forbid();
            }
            //check if user has access stop


            var link = await _linkRepository.FirstOrDefailtAsync(m => m.Id == id);
            
            _linkRepository.Remove(link);
            await _linkRepository.SaveChangesAsync();
            return RedirectToAction(nameof(Links));
        }

        // GET: Link/Delete/5
        [Route("Links/Delete/{id}")]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //check if user has access start
            var user = await _userManager.GetUserAsync(User);
            var hasAccess = await _linkRepository.UserHasAccessToLink(user, (long)id);
            if (!hasAccess)
            {
                return Forbid();
            }
            //check if user has access stop

            var link = await _linkRepository.FirstOrDefailtAsync(l => l.Id == id);
            if (link == null)
            {
                return NotFound();
            }

            return View("~/Views/Dashboard/Links/Delete.cshtml", link);
        }


        // GET: Link/Edit/5
        [Route("Links/Edit/{id}")]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //check if user has access start
            var user = await _userManager.GetUserAsync(User);
            var hasAccess = await _linkRepository.UserHasAccessToLink(user, (long)id);
            if (!hasAccess)
            {
                return Forbid();
            }
            //check if user has access stop

            var link = await _linkRepository.FirstOrDefailtAsync(x => x.Id == id);
            if (link == null)
            {
                return NotFound();
            }
            return View("~/Views/Dashboard/Links/Edit.cshtml", link);
        }

        // POST: Link/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Links/Edit/{id}")]
        public async Task<IActionResult> Edit(long id, [Bind("Id,ShortUrl,RedirectUrl")] Link link)
        {
            if (id != link.Id)
            {
                return NotFound();
            }

            //check if user has access start
            var user = await _userManager.GetUserAsync(User);
            var hasAccess = await _linkRepository.UserHasAccessToLink(user, (long)id);
            if (!hasAccess)
            {
                return Forbid();
            }
            //check if user has access stop
            if (ModelState.IsValid)
            {
                try
                {
                    await _linkRepository.Update(link);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _linkRepository.ExistsById(link.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Links));
            }
            return View(link);
        }


        // GET: Link/Create
        [Route("Links/Create")]
        public IActionResult Create()
        {
            return View("~/Views/Dashboard/Links/Create.cshtml");
        }

        // POST: Link/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Links/Create")]
        public async Task<IActionResult> Create([Bind("Id,ShortUrl,RedirectUrl")] Link link)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                link.AdOwner = user;

                //if user has basic role - clear short url and generate random one
                if (User.IsInRole("PowerUser") || User.IsInRole("Admin") )
                {
                    
                }
                else
                {
                    link.ShortUrl = null;
                }

                _linkRepository.Add(link);
                await _linkRepository.SaveChangesAsync();
                return RedirectToAction(nameof(Links));
            }
            return View(link);
        }


        //links section end


    }
}
