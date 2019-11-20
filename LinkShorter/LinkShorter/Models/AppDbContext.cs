using System;
using Microsoft.EntityFrameworkCore;

namespace LinkShorter.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        public DbSet<Ad> Ads { get; set; }


    }
}
