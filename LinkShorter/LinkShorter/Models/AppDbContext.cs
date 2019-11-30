using System;
using LinkShorter.Models.UrlStatistics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkShorter.Models
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        public DbSet<Ad> Ads { get; set; }
        public DbSet<UrlStatistic> UrlStatistics { get; set; }



    }
}
