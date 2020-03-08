using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LinkShorter.Models
{
    public class LinkRepository : ILinkRepository
    {
        private readonly AppDbContext _appDbContext;

        public LinkRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Link Add(Link _newAd)
        {
            //insert entity to db to get unique Id
            if (_appDbContext.Links.Add(_newAd) == null)
            {
                throw new Exception("There was an error during creating short url. Please try again later.");
            }
            _appDbContext.SaveChanges();

            //if on form user doesn't created his own url
            if ( string.IsNullOrEmpty( _newAd.ShortUrl ) )
            {
                ShortUrlGenerator shortUrlGenerator = new ShortUrlGenerator(_newAd.Id);
                _newAd.ShortUrl = shortUrlGenerator.GeneratedShortUrl;
                _appDbContext.Links.Update(_newAd);
            }


            

            //update entty in db
            _appDbContext.SaveChanges();
            return _newAd;
        }

        public Link GetLinkByShortUrl(string shortUrl)
        {
            return _appDbContext.Links.FirstOrDefault(p => p.ShortUrl.Equals(shortUrl));
        }

        public Task<Link> GetLinkByShortUrlAsync(Expression<Func<Link, bool>> predicate)
            => _appDbContext.Set<Link>().FirstOrDefaultAsync(predicate);

        public IEnumerable<Link> GetLinks()
        {
            return _appDbContext.Links;
        }

        public async Task<IEnumerable<Link>> GetAll()
        {
            return await _appDbContext.Links.ToListAsync();
        }

        public async Task<IEnumerable<Link>> GetWhere(Expression<Func<Link, bool>> predicate)
        {
            return await _appDbContext.Links.Where(predicate).ToListAsync();
        }

        public async Task<Link> FirstOrDefailtAsync(Expression<Func<Link, bool>> predicate)
        {
            return await _appDbContext.Set<Link>().FirstOrDefaultAsync(predicate);
        }

        public void Remove(Link link)
        {
            var statistics = _appDbContext.UrlStatistics.Where(stat => stat.Link == link);

            _appDbContext.UrlStatistics.RemoveRange(statistics);
            _appDbContext.SaveChanges();
            _appDbContext.Links.Remove(link);
            _appDbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Update(Link link)
        {
            var existing = await _appDbContext.Links.FindAsync(link.Id);

            if ( existing != null )
            {
                //change only redirect link
                existing.RedirectUrl = link.RedirectUrl;
            }
            await _appDbContext.SaveChangesAsync();

        }

        public async Task<bool> ExistsById(long id)
        {
            return await _appDbContext.Links.FirstOrDefaultAsync(x => x.Id == id) != null;
        }

        public async Task<bool> UserHasAccessToLink(IdentityUser user, long linkId)
        {
            return await _appDbContext.Links.FirstOrDefaultAsync(link => link.AdOwner == user && link.Id == linkId) != null;
        }

        public IQueryable<Link> GetAllUserLinks(IdentityUser user)
        {
            var links = _appDbContext.Links
                            .Where(link => link.AdOwner == user)
                            .AsQueryable();
            return links;
        }
    }
}
