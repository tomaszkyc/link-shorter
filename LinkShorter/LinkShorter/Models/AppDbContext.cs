using System;
using LinkShorter.Models.UrlStatistics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LinkShorter.Models
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        public DbSet<Link> Links { get; set; }
        public DbSet<UrlStatistic> UrlStatistics { get; set; }


    }
}
