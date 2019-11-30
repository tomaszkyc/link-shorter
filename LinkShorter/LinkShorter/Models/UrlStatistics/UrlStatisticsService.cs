using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UAParser;



namespace LinkShorter.Models.UrlStatistics
{
    public class UrlStatisticsService : IUrlStatisticsService
    {
        private readonly IAdRepository _adRepository;

        private readonly IUrlStatisticsRepository _urlStatisticsRepository;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly ILogger _logger;

        private Parser _uaParser;



        public UrlStatisticsService(IAdRepository adRepository,
                                    ILogger<UrlStatisticsService> logger,
                                    IHttpContextAccessor httpContextAccessor,
                                    IUrlStatisticsRepository urlStatisticsRepository)
        {
            _adRepository = adRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _uaParser = Parser.GetDefault();
            _urlStatisticsRepository = urlStatisticsRepository;
        }

        public async Task HandleRequest(Ad _ad)
        {
            _logger.LogDebug("Rozpoczynam logowanie zapytania");
            _logger.LogDebug("Url linku: {0}", _ad.ShortUrl);

            if ( _ad == null )
            {
                throw new Exception("Given ad is null");
            }

            var context = _httpContextAccessor.HttpContext;
            string uaString = context.Request.Headers["User-Agent"].ToString();
            ClientInfo clientInfo = _uaParser.Parse(uaString);

            //build information about statistic
            UrlStatistic NewStatistic = UrlStatisticsBuilder.build(clientInfo, context);
            NewStatistic.Ad = _ad;

            //check if has some statistics to this Ad
            if ( _ad.UrlStatistics == null )
            {
                _ad.UrlStatistics = new List<UrlStatistic>();
            }

            _ad.UrlStatistics.Add(NewStatistic);
            await _urlStatisticsRepository.Add(NewStatistic);




            _logger.LogDebug("Zakończyłem logowanie zapytania");
        }
    }
}
