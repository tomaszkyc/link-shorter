using System;
using LinkShorter.Models.UrlStatistics;
using Microsoft.AspNetCore.Http;
using UAParser;

namespace LinkShorter.Models
{
    public class UrlStatisticsBuilder
    {

        
        public static UrlStatistic build(ClientInfo clientInfo , HttpContext context)
        {
            UrlStatistic urlStatistic = new UrlStatistic();

            //add basic info about user browser,device and OS
            urlStatistic.BrowserFamily = clientInfo.UserAgent.Family;
            urlStatistic.BrowserMajorVersion = clientInfo.UserAgent.Major;
            urlStatistic.OSFamily = clientInfo.OS.Family;
            urlStatistic.OSMajorVersion = clientInfo.OS.Major;
            urlStatistic.OSMinorVersion = clientInfo.OS.Minor;
            urlStatistic.DeviceBrand = clientInfo.Device.Brand;
            urlStatistic.DeviceModel = clientInfo.Device.Model;

            //add info about bot services
            urlStatistic.BotService = clientInfo.Device.IsSpider;

            //ad some network info
            urlStatistic.IPAddress = context.Connection.RemoteIpAddress.ToString();

            //add info about date and time of event
            urlStatistic.EventDate = DateTime.UtcNow.ToLocalTime();

            return urlStatistic;
        }
    }
}
