using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    }
}
