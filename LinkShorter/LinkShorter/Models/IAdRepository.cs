using System;
using System.Collections.Generic;

namespace LinkShorter.Models
{
    public interface IAdRepository
    {
        IEnumerable<Ad> GetAds();

        Ad GetAdByShortUrl(string shortUrl);

        Ad Add(Ad _newAd);
    }
}
