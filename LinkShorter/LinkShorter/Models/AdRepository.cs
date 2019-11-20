using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkShorter.Models
{
    public class AdRepository : IAdRepository
    {
        private readonly AppDbContext _appDbContext;

        public AdRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Ad Add(Ad _newAd)
        {
            //insert entity to db to get unique Id
            if (_appDbContext.Ads.Add(_newAd) == null)
            {
                throw new Exception("There was an error during creating short url. Please try again later.");
            }
            _appDbContext.SaveChanges();
            //generate short url for Id
            ShortUrlGenerator shortUrlGenerator = new ShortUrlGenerator(_newAd.Id);
            _newAd.ShortUrl = shortUrlGenerator.GeneratedShortUrl;

            //update entty in db
            _appDbContext.Ads.Update(_newAd);
            _appDbContext.SaveChanges();
            return _newAd;
        }

        public Ad GetAdByShortUrl(string shortUrl)
        {
            return _appDbContext.Ads.FirstOrDefault(p => p.ShortUrl.Equals(shortUrl));
        }

        public IEnumerable<Ad> GetAds()
        {
            return _appDbContext.Ads;
        }
    }
}
