using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinkShorter.Models.UrlStatistics
{
    public interface IUrlStatisticsRepository
    {
        Task<IEnumerable<UrlStatistic>> GetAll();

        Task Add(UrlStatistic urlStatistic);

        Task<UrlStatistic> GetById(long id);


    }
}
