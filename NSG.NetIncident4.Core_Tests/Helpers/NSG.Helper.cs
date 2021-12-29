using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;

namespace NSG.Integration.Helpers
{
    public static class NSG_Helpers
    {
        public static ApplicationDbContext GetMemoryDbContext(ServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            // string _name = "NetIncident4_" + Guid.NewGuid().ToString();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "NetIncident4")
            );
            ApplicationDbContext db_context = services.BuildServiceProvider().GetService<ApplicationDbContext>();
            db_context.Database.EnsureCreated();
            // db_context.Database.OpenConnection();
            return db_context;
        }
        /*
        ** Sqlite in-memory application db context.
        */
        public static SqliteConnection GetSqliteMemoryConnection( )
        {
            return new SqliteConnection("DataSource=:memory:");
        }
        //
        public static ApplicationDbContext GetSqliteMemoryDbContext(SqliteConnection sqliteConnection, IServiceCollection services)
        {
            // Add ASP.NET Core Identity database in memory.
            sqliteConnection = new SqliteConnection("DataSource=:memory:");
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlite(sqliteConnection)
            );
            //services.AddDefaultIdentity<ApplicationUser>(options =>
            //{
            //    options.SignIn.RequireConfirmedAccount = true;
            //    options.Password.RequireDigit = true;
            //    options.Password.RequiredLength = 8;
            //    options.Password.RequireLowercase = true;
            //    options.Password.RequireUppercase = true;
            //    options.Password.RequireNonAlphanumeric = true;
            //})
            //    .AddRoles<ApplicationRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();
            //
            ApplicationDbContext db_context = services.BuildServiceProvider()
                .GetService<ApplicationDbContext>();
            db_context.Database.OpenConnection();
            db_context.Database.EnsureCreated();
            // Add Identity using in memory database to create UserManager and RoleManager.
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            return db_context;
        }
        //
        public static UserManager<ApplicationUser> GetUserManager(ServiceCollection services)
        {
            if( services == null )
            {
                throw new ArgumentNullException(nameof(services));
            }
            return services.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
        }
        //
        public static RoleManager<ApplicationRole> GetRoleManager(ServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            return services.BuildServiceProvider().GetService<RoleManager<ApplicationRole>>();
        }
        //
        static public async Task<bool> SeedMemoryDbContext(ApplicationDbContext db_context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            Console.WriteLine("Database defined ...");
            if (db_context == null)
            {
                throw new ArgumentNullException(nameof(db_context));
            }
            DatabaseSeeder _seeder = new DatabaseSeeder(db_context, userManager, roleManager);
            bool ret = await _seeder.Seed();
            Console.WriteLine("Database seeded ...");
            return ret;
        }
        //
        static public void AddLoggingService(IServiceCollection services)
        {
            services.AddLogging(builder => builder
                //.AddConfiguration(Configuration.GetSection("Logging"))
                .AddConsole()
                .AddDebug()
            );
        }
        static public void AddLoggingMoq( IServiceCollection services)
        {
            //ILoggerFactory
            //// var mockLog = new Mock<ILog<MyClass>>();
            //var mockLogFactory = new Mock<ILogFactory>();
            //mockLogFactory.Setup(f => f.CreateLog<MyClass>()).Returns(mockLog.Object);
            //services.AddSingleton<ILogFactory>(mockLogFactory.Object);
        }
        //
    }
}
