using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Reflection;
//
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MediatR;
using Moq;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;

namespace NSG.Integration.Helpers
{
    //
    /// <summary>
    /// Static helper methods
    /// </summary>
    public static class NSG_Helpers
    {
        //
        // Testing constants
        //
        public static string User_Name = "Phil";
        public static string User_Email = @"Phil@any.net";
        public static string Password = @"P@ssword0";
        public static string User_Name2 = "author";
        public static string User_Email2 = @"author@any.net";
        public static string Password2 = @"P@ssword0";
        //
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
        //
        /// <summary>
        /// Sqlite in-memory application db context.
        /// </summary>
        /// <returns>SqliteConnection</returns>
        public static SqliteConnection GetSqliteMemoryConnection( )
        {
            return new SqliteConnection("DataSource=:memory:");
        }
        //
        /// <summary>
        /// Get a relational Sqlite in-memory db instance
        /// </summary>
        /// <param name="sqliteConnection"></param>
        /// <param name="services"></param>
        /// <returns>ApplicationDbContext</returns>
        public static ApplicationDbContext GetSqliteMemoryDbContext(SqliteConnection sqliteConnection, IServiceCollection services)
        {
            // Add ASP.NET Core Identity database in memory.
            sqliteConnection = new SqliteConnection("DataSource=:memory:");
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlite(sqliteConnection)
            );
            // Add Identity using in memory database to create UserManager and RoleManager.
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
                // Object reference not set to an instance of an object. IdentityBuilderUIExtensions.GetApplicationAssembly(IdentityBuilder builder)
                //.AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<UserManager<ApplicationUser>>()
                .AddRoleManager<RoleManager<ApplicationRole>>()
                .AddDefaultTokenProviders();
            //
            ApplicationDbContext db_context = services.BuildServiceProvider()
                .GetService<ApplicationDbContext>();
            db_context.Database.OpenConnection();
            db_context.Database.EnsureCreated();
            //
            return db_context;
        }
        //
        /// <summary>
        /// Get UserManager from the ServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static UserManager<ApplicationUser> GetUserManager(ServiceCollection services)
        {
            if( services == null )
            {
                throw new ArgumentNullException(nameof(services));
            }
            return services.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
        }
        //
        /// <summary>
        /// Get RoleManager from the ServiceCollection
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        static public void AddLoggingService(this IServiceCollection services)
        {
            services.AddLogging(builder => builder
                //.AddConfiguration(Configuration.GetSection("Logging"))
                .AddConsole()
                .AddDebug()
            );
        }
        //
        public static ClaimsPrincipal TestPrincipal(string userName, string role)
        {
            ClaimsPrincipal _user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, $"{userName}@any.net"),
                new Claim(ClaimTypes.Role, role)
            }, "basic"));
            //
            return _user;
        }
        //
    }
}
