using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LinkShorter.Models.UrlStatistics
{
    public interface IUrlStatisticsRepository
    {
        Task<IEnumerable<UrlStatistic>> GetAll();

        Task Add(UrlStatistic urlStatistic);

        Task<UrlStatistic> GetById(long id);

        Task<IEnumerable<UrlStatistic>> GetWhere(Expression<Func<UrlStatistic, bool>> predicate);

        Task<IEnumerable<UrlStatistic>> GetCountFromActualMonthForUserAsync(string userid);
    }
}
