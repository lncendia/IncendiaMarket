using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApplication.Data;
using WebApplication.Data.Interfaces;
using WebApplication.Data.Repository;

namespace WebApplication
{
    public class Startup
    {
        private static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        private readonly IConfigurationRoot _configuration;

        public Startup(IWebHostEnvironment environment)
        {
            _configuration = new ConfigurationBuilder().SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json").Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<Db>(options =>
            {
                options.UseSqlite(_configuration.GetConnectionString("MyConnection"));
                options.UseLoggerFactory(MyLoggerFactory);
            });
            services.AddTransient<IAllAdvertisement, AdvertisementRepository>();
            services.AddTransient<IAllGroups, GroupRepository>();
            services.AddTransient<IMessage, MessageRepository>();
            services.AddTransient<EmailService>();
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddMemoryCache();
            services.AddIdentity<IdentityUser, IdentityRole>(opt =>
                {
                    opt.User.RequireUniqueEmail = true;
                    opt.Password.RequireUppercase = false;
                    opt.Password.RequireDigit = false;
                    opt.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<Db>()
                .AddDefaultTokenProviders();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.UseStatusCodePages();
            app.UseDeveloperExceptionPage();
            //app.UseSession();
            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(name: "categoryFilter", template: "Cars/{action}/{category}",
                    new {Controller = "Cars", Action = "List"});
            });
        }
    }
}