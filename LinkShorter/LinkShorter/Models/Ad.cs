using System;
namespace LinkShorter.Models
{
    public class Ad
    {
        public long Id { get; set; }
        public string ShortUrl { get; set; }
        public string RedirectUrl { get; set; }
        public string ImageUrl { get; set; }
        public string ImageAltText { get; set; }

        public Ad()
        {
        }
    }
}
