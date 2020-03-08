using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinkShorter.Models.Tools;
using Microsoft.EntityFrameworkCore;

namespace LinkShorter.Models.UrlStatistics
{
    public class UrlStatisticsRepository : IUrlStatisticsRepository
    {
        private readonly AppDbContext _appDbContext;


        public UrlStatisticsRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Add(UrlStatistic urlStatistic)
        {
            await _appDbContext.UrlStatistics.AddAsync(urlStatistic);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UrlStatistic>> GetAll()
        {
            return await _appDbContext.UrlStatistics.ToListAsync();
        }

        public Task<UrlStatistic> GetById(long id)
        {
            return _appDbContext.UrlStatistics.FindAsync(id); 
        }

        public async Task<IEnumerable<UrlStatistic>> GetCountFromActualMonthForUserAsync(string userid)
        {

            var results = await _appDbContext.UrlStatistics.Where
                (
                    stat =>
                    (
                        (DateTimeDayOfMonthExtensions.Between(stat.EventDate, DateTimeDayOfMonthExtensions.FirstDayOfMonth(stat.EventDate), DateTimeDayOfMonthExtensions.LastDayOfMonth(stat.EventDate)))
                        &&
                        (stat.Link.AdOwner.Id == userid)
                    )
                ).ToListAsync();
            return results;
            
        }

        public async Task<IEnumerable<UrlStatistic>> GetWhere(Expression<Func<UrlStatistic, bool>> predicate)
        {
            return await _appDbContext.UrlStatistics.Where(predicate).ToListAsync();
        }
    }
}
