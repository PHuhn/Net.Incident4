// ===========================================================================
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
//
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
//
using NSG.NetIncident4.Core.Domain.Entities.Authentication;
using NSG.NetIncident4.Core.Infrastructure.Authentication;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.Infrastructure.Services;
//
namespace NSG.NetIncident4.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public virtual void ConfigureServices(IServiceCollection services)
        {
            //
            ConfigureLoggingServices(services);
            //
            services.AddControllersWithViews();
            //
            ConfigureViewLocation(services);
            /*
            ** Read values from the 'appsettings.json'
            ** * Add and configure email/notification services
            ** * Services like ping/whois
            ** * Applications information line name and phone #
            ** * Various authorization information
            */
            ConfigureNotificationServices(services);
            // AdminRole/CompanyAdminRole/AnyUserRole
            ConfigureAuthorizationPolicyServices(services);
            // Add session for state and temp data provider
            ConfigureSessionServices(services);
            // CORS
            ConfigureCorsServices(services);
            //
            ConfigureSwaggerServices(services);
            //
        }
        //
        /// <summary>
        /// Add and configure logging
        /// </summary>
        /// <param name="services">The current collection of services, 
        /// add logging services.
        /// </param>
        public virtual void ConfigureLoggingServices(IServiceCollection services)
        {
            services.AddLogging(builder => builder
                .AddConfiguration(Configuration.GetSection("Logging"))
                .AddConsole()
                .AddDebug()
            );
        }
        //
        /// <summary>
        /// Add session services for state and temp data provider
        /// </summary>
        /// <param name="services">The current collection of services, 
        /// add session services and options.
        /// </param>
        public virtual void ConfigureSessionServices(IServiceCollection services)
        {
            services.AddSession(options =>
                options.Cookie.IsEssential = true
            );
        }
        //
        /// <summary>
        /// Add CORS services
        /// </summary>
        /// <param name="services">The current collection of services, 
        /// add CORS services and options.
        /// </param>
        public virtual void ConfigureCorsServices(IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy("CorsAnyOrigin", builder => {
                    builder
                        .WithOrigins("http://localhost:4200,http://localhost:10111")
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
        //
        /// <summary>
        /// Add Swagger services
        /// </summary>
        /// <param name="services">The current collection of services, 
        /// add swagger services and options.
        /// </param>
        public virtual void ConfigureSwaggerServices(IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                // Generate the Default UI of Swagger Documentation
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "NSG Net-Incident4.Core",
                    Description = "Authentication and Authorization in ASP.NET 5 with JWT and Swagger"
                });
                // To Enable authorization using Swagger (JWT)
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }
        //
        /// <summary>
        /// Read values from the 'appsettings.json'
        /// * Add and configure email/notification services
        /// * Services like ping/whois
        /// * Applications information line name and phone #
        /// </summary>
        /// <param name="services">The current collection of services, 
        /// add notification services.
        /// </param>
        public virtual void ConfigureNotificationServices(IServiceCollection services)
        {
            /*
            ** Read values from the 'appsettings.json'
            ** * Add and configure email/notification services
            ** * Services like ping/whois
            ** * Applications information line name and phone #
            */
            services.Configure<MimeKit.NSG.EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.Configure<ServicesSettings>(Configuration.GetSection("ServicesSettings"));
            services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
            AuthSettings _authSettings = Options.Create<AuthSettings>(
                Configuration.GetSection("AuthSettings").Get<AuthSettings>()).Value;
            services.Configure<AuthSettings>(Configuration.GetSection("AuthSettings"));
            /*
            ** Add email/notification services
            */
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IEmailSender, NotificationService>();
            services.AddTransient<IApplication, ApplicationImplementation>();
            /*
            ** Add MediatR from MediatR.Extensions.Microsoft.DependencyInjection package
            */
            // services.AddMediatR(typeof(Startup));
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
        //
        /// <summary>
        /// Add authorization services
        /// </summary>
        /// <param name="services">The current collection of services, 
        /// add authorization services policies.
        /// </param>
        public virtual void ConfigureAuthorizationPolicyServices(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("CompanyAdminRole", policy => policy.RequireRole("Admin", "CompanyAdmin"));
                options.AddPolicy("AnyUserRole", policy => policy.RequireRole("User", "Admin", "CompanyAdmin"));
            });
        }
        //
        /// <summary>
        /// Configure locations of views
        /// </summary>
        /// <param name="services">The current collection of services, 
        /// used to change view location for clean architecture.
        /// </param>
        public virtual void ConfigureViewLocation(IServiceCollection services)
        {
            services.Configure<RazorPagesOptions>(options =>
            {
                options.RootDirectory = "/UI/Identity";
            });
            services.Configure<RazorViewEngineOptions>(options =>
            {
                // http://www.binaryintellect.net/articles/c50d3f14-7048-4b4f-84f4-1b28cb0f9d96.aspx
                // {2} is area, {1} is controller,{0} is the action
                options.ViewLocationFormats.Clear();
                options.ViewLocationFormats.Add("/UI/Views/{1}/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Add("/UI/Views/Admin/{1}/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Add("/UI/Views/CompanyAdmin/{1}/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Add("/UI/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                options.ViewLocationFormats.Add("/UI/Identity/{0}" + RazorViewEngine.ViewExtension);
                // now razor pages
                options.PageViewLocationFormats.Clear();
                options.PageViewLocationFormats.Add("{0}" + RazorViewEngine.ViewExtension);
                options.PageViewLocationFormats.Add("/Account/{0}" + RazorViewEngine.ViewExtension);
                options.PageViewLocationFormats.Add("/Account/Manage/{0}" + RazorViewEngine.ViewExtension);
                options.PageViewLocationFormats.Add("{1}/{0}" + RazorViewEngine.ViewExtension);
                options.PageViewLocationFormats.Add("/Admin/{1}/{0}" + RazorViewEngine.ViewExtension);
                options.PageViewLocationFormats.Add("/CompanyAdmin/{1}/{0}" + RazorViewEngine.ViewExtension);
                options.PageViewLocationFormats.Add("/UI/Views/Shared/{0}" + RazorViewEngine.ViewExtension);
                options.PageViewLocationFormats.Add("/UI/Identity/Account/Manage/{0}" + RazorViewEngine.ViewExtension);
                //
            });
        }
        //
        /// <summary>
        /// This method gets called by the runtime.
        /// Use this method to configure the HTTP request pipeline.
        /// </summary>
        public virtual void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            // routing/CORS/endpoint
            app.UseCors("CorsAnyOrigin");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NSG Net-Incident4.Core v1"));
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            //
            //NSG.NetIncident4.Core.Domain.Entities.SeedData.Initialize(
            //    context, roleManager, false).Wait();
            //
        }
    }
}
// ===========================================================================