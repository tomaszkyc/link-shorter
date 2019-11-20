using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LinkShorter.Models
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {

            if ( !context.Ads.Any() )
            {
                context.AddRange
                (

                new Ad
                {
                Id = 1,
                RedirectUrl = "https://obuwiee.pl/air-force/1647-buty-nike-air-force-one-low-af1-green.html",
                ShortUrl = "1"
                },
                new Ad
                {
                    Id = 2,
                    RedirectUrl = "https://obuwiee.pl/air-force/1217-buty-nike-air-force-flyknit-one-low-af1.html",
                    ShortUrl = "2"
                },
                new Ad
                {
                    Id = 3,
                    RedirectUrl = "https://obuwiee.pl/air-force/1218-buty-nike-air-force-one-flyknit-low-af1.html",
                    ShortUrl = "3"
                },
                new Ad
                {
                    Id = 4,
                    RedirectUrl = "https://obuwiee.pl/air-force/1219-buty-nike-air-force-one-flyknit-low-af1.html",
                    ShortUrl = "4"
                }

                );
                context.Database.OpenConnection();
                try
                {
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Ads ON");
                    context.SaveChanges();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Ads OFF");
                }
                finally
                {
                    context.Database.CloseConnection();
                }

            }

        }
    }
}
