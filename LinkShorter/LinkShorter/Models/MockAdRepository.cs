using System;
using System.Collections.Generic;
using System.Linq;

namespace LinkShorter.Models
{
    public class MockAdRepository : IAdRepository
    {
        private List<Ad> _ads;


        public MockAdRepository()
        {
            if ( _ads == null )
            {
                InitializeAds();
            }
        }

        private void InitializeAds()
        {
            _ads = new List<Ad>
            {
                new Ad{ Id = 1,
                        ImageAltText = "shoes 1",
                        ImageUrl = "https://obuwiee.pl/10149-large_default/buty-nike-air-force-one-low-af1-green.jpg",
                    RedirectUrl = "https://obuwiee.pl/air-force/1647-buty-nike-air-force-one-low-af1-green.html", ShortUrl = "1"
                },
                new Ad{ Id = 2,
                        ImageAltText = "shoes 2",
                        ImageUrl = "https://obuwiee.pl/6737-large_default/buty-nike-air-force-flyknit-one-low-af1.jpg",
                    RedirectUrl = "https://obuwiee.pl/air-force/1217-buty-nike-air-force-flyknit-one-low-af1.html", ShortUrl = "2"
                },
                new Ad{ Id = 3,
                        ImageAltText = "shoes 3",
                        ImageUrl = "https://obuwiee.pl/6743-large_default/buty-nike-air-force-one-flyknit-low-af1.jpg",
                    RedirectUrl = "https://obuwiee.pl/air-force/1218-buty-nike-air-force-one-flyknit-low-af1.html", ShortUrl = "3"
                },
                new Ad{ Id = 4,
                        ImageAltText = "shoes 4",
                        ImageUrl = "https://obuwiee.pl/6749-large_default/buty-nike-air-force-one-flyknit-low-af1.jpg",
                    RedirectUrl = "https://obuwiee.pl/air-force/1219-buty-nike-air-force-one-flyknit-low-af1.html", ShortUrl = "4"
                },
            };
        }

        public Ad GetAdByShortUrl(string shortUrl)
        {
            return _ads.FirstOrDefault(p => p.ShortUrl.Equals(shortUrl));
        }

        public IEnumerable<Ad> GetAds()
        {
            return _ads;
        }
    }
}
