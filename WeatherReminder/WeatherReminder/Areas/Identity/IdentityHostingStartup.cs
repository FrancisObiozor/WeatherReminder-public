using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WeatherReminder.Models.DataStorageModel;
using WeatherReminder.Models.WeatherReminderModel;

[assembly: HostingStartup(typeof(WeatherReminder.Areas.Identity.IdentityHostingStartup))]
namespace WeatherReminder.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {

        public void Configure(IWebHostBuilder builder)
        {


            builder.ConfigureServices((context, services) => {
                services.AddDbContext<WeatherReminderDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("WeatherReminderDbContextConnection")));

                services.AddDefaultIdentity<WeatherReminderUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<WeatherReminderDbContext>();

                services.Configure<IdentityOptions>(options =>
                {
                    // Default Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 8;
                    options.Password.RequiredUniqueChars = 1;
                });
            });
        }
    }
}