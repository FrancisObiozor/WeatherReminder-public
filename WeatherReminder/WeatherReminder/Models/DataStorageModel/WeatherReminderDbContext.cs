using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using WeatherReminder.Models.WeatherReminderModel;

namespace WeatherReminder.Models.DataStorageModel
{
    public class WeatherReminderDbContext : IdentityDbContext<WeatherReminderUser>
    {

        public WeatherReminderDbContext(DbContextOptions<WeatherReminderDbContext> options)
            : base(options)
        {
        }

        public DbSet<WeatherSettings> WeatherSettings { get; set; }
        public DbSet<UserLocation> UserLocation { get; set; }
        public DbSet<Temperature> Temperature { get; set; }
        public DbSet<Umbrella> Umbrella { get; set; }
        public DbSet<Snow> Snow { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }

    //Bring in when deploying project
    public class WeatherReminderDbContextFactory : IDesignTimeDbContextFactory<WeatherReminderDbContext>
    {
        //private readonly IKeyVault _keyVault;

        //public WeatherReminderDbContextFactory()
        //{

        //}

        //public WeatherReminderDbContextFactory(IKeyVault keyVault)
        //{
        //    _keyVault = keyVault;
        //}

        public WeatherReminderDbContext CreateDbContext(string[] args)
        {
            //var connectionString = _keyVault.ApiKeys.ConnectionStringsWeatherReminderDbContextConnection;
            var connectionString = "Data Source=MSI\\SQLEXPRESS;Database=WeatherReminder;Trusted_Connection=True;MultipleActiveResultSets=true";
            var optionsBuilder = new DbContextOptionsBuilder<WeatherReminderDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new WeatherReminderDbContext(optionsBuilder.Options);
        }
    }


}
