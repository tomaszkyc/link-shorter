using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LinkShorter.Models
{
    public interface ILinkRepository
    {
        IEnumerable<Link> GetLinks();

        Link GetLinkByShortUrl(string shortUrl);

        Link Add(Link _newLink);

        Task<Link> GetLinkByShortUrlAsync(Expression<Func<Link, bool>> predicate);

        Task<IEnumerable<Link>> GetAll();

        Task<IEnumerable<Link>> GetWhere(Expression<Func<Link, bool>> predicate);

        IQueryable<Link> GetAllUserLinks(IdentityUser user);

        Task<Link> FirstOrDefailtAsync(Expression<Func<Link, bool>> predicate);

        void Remove(Link link);

        Task SaveChangesAsync();

        Task Update(Link link);

        Task<bool> ExistsById(long id);

        Task<bool> UserHasAccessToLink(IdentityUser user, long linkId);

    }
}
