using System;
using System.Threading.Tasks;
using LinkShorter.Models;
using LinkShorter.Models.Tools;
using LinkShorter.Models.UrlStatistics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkShorter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                
            });

            //add http context accessor
            services.AddHttpContextAccessor();

            //add repositories
            services.AddTransient<ILinkRepository, LinkRepository>();
			services.AddTransient<IUrlStatisticsRepository, UrlStatisticsRepository>();
            services.AddTransient<IUrlStatisticsService, UrlStatisticsService>();


            //add email service
            services.AddTransient<IEmailSender, EmailSender>();


            //add email template
            services.AddTransient<EmailTemplate>();


            //add db context
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer( Configuration.GetConnectionString("DefaultConnection") ));



            services.AddDefaultIdentity<IdentityUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {

            var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            using (var context = scope.ServiceProvider.GetService<AppDbContext>())
            {
               context.Database.Migrate();
            }




            app.UseExceptionHandler("/Error");
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            app.UseHttpsRedirection();
            app.UseStatusCodePagesWithRedirects("/Error/{0}");
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            CreateRoles(serviceProvider).GetAwaiter().GetResult();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //adding custom roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roleNames = { "Admin", "PowerUser", "User" };
            
            //creating roles
            foreach(var roleName in roleNames)
            {
                var roleExists = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    IdentityResult roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            string UserEmail = Configuration.GetSection("AdminUser").GetValue<string>("AdminEmail");

            //creating super admin user who could maintain the app
            var mainAdmin = new IdentityUser
            {
                UserName = UserEmail,
                Email = UserEmail
            };

            string UserPassword = Configuration.GetSection("AdminUser").GetValue<string>("AdminPassword");
            var _user = await UserManager.FindByEmailAsync(UserEmail);

            if (_user == null)
            {
                var createMainAdmin = await UserManager.CreateAsync(mainAdmin, UserPassword);
                if ( createMainAdmin.Succeeded )
                {
                    //grant admin role
                    await UserManager.AddToRoleAsync(mainAdmin, "Admin");
                }

            }
            //if user exists
            else
            {
                //make sure to grant proper role
                await UserManager.AddToRoleAsync(_user, "Admin");
            }






        }



    }
}
