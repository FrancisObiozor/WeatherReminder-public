using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherReminder.Models.UnconfirmedUsers;

namespace WeatherReminder.Models.DataStorageModel
{
    public class UnconfirmedUserDbContext : DbContext
    {
        public UnconfirmedUserDbContext(DbContextOptions<UnconfirmedUserDbContext> options)
            : base(options)
        {
        }

        public DbSet<UnconfirmedUser> UnconfirmedUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }

    public class UnconfirmedUserDbContextFactory : IDesignTimeDbContextFactory<UnconfirmedUserDbContext>
    {
        //private readonly IKeyVault _keyVault;

        public UnconfirmedUserDbContextFactory()
        {

        }

        //public UnconfirmedUserDbContextFactory(IKeyVault keyVault)
        //{
        //    _keyVault = keyVault;
        //}

        public UnconfirmedUserDbContext CreateDbContext(string[] args)
        {
            //var connectionString = _keyVault.ApiKeys.ConnectionStringsWeatherReminderDbContextConnection;
            var connectionString = "Data Source=MSI\\SQLEXPRESS;Database=WeatherReminder;Trusted_Connection=True;MultipleActiveResultSets=true";

            var optionsBuilder = new DbContextOptionsBuilder<UnconfirmedUserDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new UnconfirmedUserDbContext(optionsBuilder.Options);
        }
    }

}
