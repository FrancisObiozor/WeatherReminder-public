using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using WeatherReminder.Controllers;
using WeatherReminder.Models.ApiModel.ApiKeys;
using WeatherReminder.Models.ApiModel.AutoCompleteApi;
using WeatherReminder.Models.ApiModel.EmailApi;
using WeatherReminder.Models.ApiModel.GeocodeApi;
using WeatherReminder.Models.ApiModel.IpAddressGeocodeApi;
using WeatherReminder.Models.ApiModel.LocationFromBrowserApi;
using WeatherReminder.Models.ApiModel.TextMessageApi;
using WeatherReminder.Models.ApiModel.TextMessageApi.VerifyCellApi;
using WeatherReminder.Models.ApiModel.WeatherForecastApi.ForecastApi;
using WeatherReminder.Models.ApiModel.WeatherForecastApi.LocationApi;
using WeatherReminder.Models.BackgroundModel;
using WeatherReminder.Models.DataStorageModel;
using WeatherReminder.Models.HelpFunctionModel;
using WeatherReminder.Models.HomepageModel;
using WeatherReminder.Models.UnconfirmedUsers;

namespace WeatherReminder
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

            services.Configure<IdentityOptions>(options =>
            {
                // Default Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddDbContext<WeatherReminderDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("WeatherReminderDbContextConnection")));
            services.AddDbContext<UnconfirmedUserDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("WeatherReminderDbContextConnection")));


            services.AddHangfire(config => config.UseSqlServerStorage(Configuration.GetConnectionString("WeatherReminderDbContextConnection")));
            services.AddHangfireServer();

            //services.AddSingleton<ITemperatureReminder, TemperatureReminder>();
            services.AddSingleton<IHomepageStats, HomepageStats>();
            //services.AddSingleton<ICellVerificationStatus, CellVerificationStatus>();
            services.AddSingleton<IHelperFunctions, HelperFunctions>();
            services.AddSingleton<IKeyVault, KeyVault>();
            services.AddSingleton<ILocationFromBrowser, LocationFromBrowser>();

            services.AddScoped<IWeatherReminderUserRepository, WeatherReminderUserRepository>();
            services.AddScoped<IAutoComplete, AutoComplete>();
            services.AddScoped<IGeocodeLocation, GeocodeLocation>();
            services.AddScoped<IIpAddressGeocode, IpAddressGeocode>();
            services.AddScoped<ILocationKey, LocationKey>();
            services.AddScoped<IWeatherForecast, WeatherForecast>();
            services.AddScoped<ITextMessage, TextMessage>();
            services.AddScoped<IVerifyCell, VerifyCell>();
            services.AddScoped<IUnconfirmedUserRepository, UnconfirmedUserRepository>();
            services.AddScoped<IBackgroundTasks, BackgroundTasks>();

            services.AddTransient<IEmail, Email>();
            services.AddTransient<ICustomEmail, CustomEmail>();


            services.AddHostedService<BackgroundTaskHostedService>();

            services.AddLogging(logging =>
            {
                logging.AddConsole();
            });

            services.Configure<AuthMessageSenderOptions>(Configuration);

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddHttpClient();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute
                (
                    name: "default",
                    pattern: "{controller=Home}/{action=Welcome}/{id?}"
                    );
                endpoints.MapRazorPages();
            });

        }


    }
}
