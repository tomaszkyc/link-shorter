using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LinkShorter.Models.UrlStatistics
{
    public interface IUrlStatisticsService
    {
        Task HandleRequest(Link _ad);
    }
}
