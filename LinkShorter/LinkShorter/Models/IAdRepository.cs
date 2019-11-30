using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LinkShorter.Models
{
    public interface IAdRepository
    {
        IEnumerable<Ad> GetAds();

        Ad GetAdByShortUrl(string shortUrl);

        Ad Add(Ad _newAd);

        Task<Ad> GetAdByShortUrlAsync(Expression<Func<Ad, bool>> predicate);
    }
}
