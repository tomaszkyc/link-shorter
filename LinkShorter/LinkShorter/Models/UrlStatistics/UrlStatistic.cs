using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkShorter.Models.UrlStatistics
{
    public class UrlStatistic
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string BrowserFamily { get; set; }
        public string BrowserMajorVersion { get; set; }

        public string OSFamily { get; set; }
        public string OSMajorVersion { get; set; }
        public string OSMinorVersion { get; set; }

        public string DeviceBrand { get; set; }
        public string DeviceModel { get; set; }
        public bool BotService { get; set; }

        public string IPAddress { get; set; }

        public DateTime EventDate { get; set; }

        public Ad Ad { get; set; }

        public UrlStatistic()
        {
            
        }
    }
}
