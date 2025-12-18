// ===========================================================================
//
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
// using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
// using Microsoft.OpenApi.Models;
// using Swashbuckle.AspNetCore;
//
using NSG.NetIncident4.Core.Domain.Entities;
using NSG.NetIncident4.Core.Infrastructure.Authentication;
using NSG.NetIncident4.Core.Infrastructure.Common;
using NSG.NetIncident4.Core.Infrastructure.Notification;
using NSG.NetIncident4.Core.Infrastructure.Services;
using NSG.NetIncident4.Core.Persistence;
using System.Reflection;
using System.Text;
//
namespace NSG.NetIncident4.Core
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			authSettings = new AuthSettings();
        }

        public IConfiguration Configuration { get; }
		public AuthSettings? authSettings;
		private string _corsName = "CorsOrigins";
		public string corsNamedOriginPolicy
		{
			private set { }
			get { return this._corsName; }
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public virtual void ConfigureServices(IServiceCollection services)
		{
			//
			ConfigureLoggingServices(services);
			//
			AddAndConfigureControllers(services);
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
			// Add session for state and temp data provider
			ConfigureSessionServices(services);
			// CORS
			ConfigureCorsServices(services);
			// Cookie and JWT (swagger) authentication
			ConfigureAuthenticationServices(services);
			// AdminRole/CompanyAdminRole/AnyUserRole
			ConfigureAuthorizationPolicyServices(services);
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
		/// Add and configure logging
		/// </summary>
		/// <param name="services">The current collection of services, 
		/// add logging services.
		/// </param>
		public virtual void AddAndConfigureControllers(IServiceCollection services)
		{
			services.AddControllersWithViews();
			services.AddControllers()
				.AddJsonOptions(options =>
					options.JsonSerializerOptions.PropertyNamingPolicy = null);
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
            // System.Diagnostics.Debug.WriteLine("ConfigureCorsServices ...");
            // System.Diagnostics.Debug.WriteLine(authSettings.CorsAllowOrigins);
            var _corsOrigins = authSettings.CorsAllowOrigins.Split(";");
			if ( _corsOrigins.Count() == 0 )
			{
				throw (new ApplicationException("No CORS origins found (required for API)."));
			}
			services.AddCors(options => {
				options.AddPolicy(name: corsNamedOriginPolicy, builder => {
					builder
						.WithOrigins(_corsOrigins)
						.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowCredentials();
				});
			});
		}
		//
		/// <summary>
		/// add authentication schemes
		/// </summary>
		/// <param name="services">The current collection of services, 
		/// add authentication schemes to the container.
		public virtual void ConfigureAuthenticationServices(IServiceCollection services)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = "BearerOrCookie";
				options.DefaultChallengeScheme = "BearerOrCookie";
				options.DefaultScheme = "BearerOrCookie";
			})
				/*
				** Add Jwt Bearer
				*/
				.AddJwtBearer(options =>
				{
					options.SaveToken = true;
					// Require should be true in production appsetting.json
					options.RequireHttpsMetadata = authSettings.JwtRequireHttps;
					options.TokenValidationParameters = new TokenValidationParameters()
					{
						// needs to match AuthenticateController.Login JwtSecurityToken
						ClockSkew = TimeSpan.Zero,
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidAudience = authSettings.JwtAudience,
						ValidIssuer = authSettings.JwtIssuer,
						IssuerSigningKey = new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes(authSettings.JwtSecret)),
					};
				})
				/*
				** Add cookie authentication scheme
				*/
				.AddCookie(options => {
					options.Cookie.Name = SharedConstants.IdentityApplicationScheme;
					options.LoginPath = new PathString("/Account/Login");
					options.LogoutPath = new PathString("/Account/Logout");
					options.AccessDeniedPath = new PathString("/Account/AccessDenied");
					options.SlidingExpiration = authSettings.CookieSlidingExpiration;
					options.ExpireTimeSpan = TimeSpan.FromHours(authSettings.CookieExpirationHours);
				})
				/*
				** Conditionally add either JWT bearer of cookie authentication scheme
				*/
				.AddPolicyScheme("BearerOrCookie", "Custom JWT bearer or cookie", options =>
				{
					options.ForwardDefaultSelector = context =>
					{
						// since all my api will be starting with /api, modify this condition as per your need.
						if (context.Request.Path.StartsWithSegments("/api", StringComparison.InvariantCulture))
							return JwtBearerDefaults.AuthenticationScheme;
						else
							return IdentityConstants.ApplicationScheme;
					};
				});
			// Conditionally add Google external authentication.
			IConfigurationSection googleAuthSection = Configuration.GetSection("Authentication:Google");
			if (googleAuthSection != null)
			{
				if (!string.IsNullOrEmpty(googleAuthSection["ClientId"]))
				{
					services.AddAuthentication()
						.AddGoogle(options =>
						{
							string? _cId = googleAuthSection["ClientId"];
                            string? _cSecret = googleAuthSection["ClientSecret"];
                            if (_cId != null && _cSecret != null)
							{
                                options.ClientId = _cId;
                                options.ClientSecret = _cSecret;
                            }
                        });
				}
			};
			// Conditionally add Microsoft Account external authentication.
			IConfigurationSection microsoftAuthSection = Configuration.GetSection("Authentication:Microsoft");
			if (microsoftAuthSection != null)
			{
				if (!string.IsNullOrEmpty(microsoftAuthSection["ClientId"]))
				{
					services.AddAuthentication()
						.AddMicrosoftAccount(microsoftOptions =>
						{
                            string? _cId = microsoftAuthSection["ClientId"];
                            string? _cSecret = microsoftAuthSection["ClientSecret"];
                            if (_cId != null && _cSecret != null)
                            {
                                microsoftOptions.ClientId = _cId;
                                microsoftOptions.ClientSecret = _cSecret;
                            }
						});
				}
			};
		}
		//
		/// <summary>
		/// Configure cookie redirects or return status code
		/// </summary>
		/// <param name="services">The current collection of services, 
		/// configure cookie options.
		/// </param>
		public virtual void ConfigureCookies(IServiceCollection services)
		{
			services.ConfigureApplicationCookie(options =>
			{
				options.Events = new CookieAuthenticationEvents()
				{
					OnRedirectToLogin = (context) =>
					{
						if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == 200)
						{
							context.Response.StatusCode = 401;
						}
						return Task.CompletedTask;
					},
					OnRedirectToAccessDenied = (context) =>
					{
						if (context.Request.Path.StartsWithSegments("/api") && context.Response.StatusCode == 200)
						{
							context.Response.StatusCode = 403;
						}
						return Task.CompletedTask;
					}
				};
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
		    //services.AddSwaggerGen(swagger =>
			//{
			//	// Generate the Default UI of Swagger Documentation
			//	swagger.SwaggerDoc(_swaggerVersion, new OpenApiInfo
			//	{
			//		Version = _swaggerNameVersion,
			//		Title = _swaggerNameTitle,
			//		Description = "Authentication and Authorization in ASP.NET 10 with JWT and Swagger"
			//	});
		    // // To Enable authorization using Swagger (JWT)
		    // // 1) Need to define a bearer scheme with a name (Bearer),
		    // // 2) Add requirements that specify that scheme via Id (Bearer)
		    // // had to downgrade Microsoft.OpenApi to 2.3.12 to not have compile err
		   // swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
		   // {
		   //                 Type = SecuritySchemeType.Http,
		   //                 Scheme = "bearer",
		   //                 BearerFormat = "JWT",
		   //                 Description = "Enter 'Bearer' [space] and then your valid token in the text input below. Example: \"Authorization: Bearer {token}\"",
		   // });
		   // swagger.AddSecurityRequirement(document => new OpenApiSecurityRequirement
		   // {
		   //	[new OpenApiSecuritySchemeReference("Bearer", document)] = []
		   // });
		   // );
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
            services.Configure<Dictionary<string, MimeKit.NSG.EmailSettings>>(Configuration.GetSection("EmailSettings"));
			services.Configure<ServicesSettings>(Configuration.GetSection("ServicesSettings"));
			services.Configure<ApplicationSettings>(Configuration.GetSection("ApplicationSettings"));
			authSettings = Options.Create<AuthSettings>(
				Configuration.GetSection("AuthSettings").Get<AuthSettings>()).Value;
            if ( authSettings == null )
            {
                throw (new ApplicationException("No auth setting found"));
            }
            services.Configure<AuthSettings>(Configuration.GetSection("AuthSettings"));
			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
			/*
			** Add email/application services
			*/
			services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IEmailSender, NotificationService>();
            services.AddTransient<IApplication, ApplicationImplementation>();
			/*
			** Add MediatR from MediatR.Extensions.Microsoft.DependencyInjection package
			*/
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
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
			//
			services.AddMvc()
				.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNameCaseInsensitive = false);
			//
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
		/// <summary>
		/// This method gets called by the runtime.
		/// <summary>
		/// Configure database
		/// * AddDbContext with SqlServer
		/// * AddDefaultIdentity roles and store
		/// </summary>
		/// <param name="services">The current collection of services, 
		/// used to change view location for clean architecture.
		/// </param>
		public virtual void ConfigureSqlContextIdentity(IServiceCollection services)
		{
			//
			string? _connetionString = Configuration.GetConnectionString("NetIncident4");
			IdentitySettings? identitySettings = Configuration.GetSection(
				"IdentitySettings").Get<IdentitySettings>();
			if (string.IsNullOrEmpty(_connetionString) || identitySettings == null)
			{
				throw (new ApplicationException("No connection or identity setting string found"));
			}
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(_connetionString));
			// services.AddDatabaseDeveloperPageExceptionFilter(); <- for development
			services.AddDefaultIdentity<ApplicationUser>(options => {
				options.Password.RequiredLength = identitySettings.PasswordMinLength;
				options.Password.RequireDigit = identitySettings.PasswordRequireDigit;
				options.Password.RequireLowercase = identitySettings.PasswordRequireLowercase;
				options.Password.RequireUppercase = identitySettings.PasswordRequireUppercase;
				options.Password.RequireNonAlphanumeric = identitySettings.PasswordRequireSpecialCharacter;
				options.User.RequireUniqueEmail = true; // identitySettings.UserRequireUniqueEmail;
				options.SignIn.RequireConfirmedAccount = identitySettings.SignInRequireConfirmedAccount;
				options.SignIn.RequireConfirmedEmail = identitySettings.SignInRequireConfirmedEmail;
				options.Lockout.AllowedForNewUsers = identitySettings.LockoutAllowed;
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(identitySettings.LockoutTimeSpanMinutes);
				options.Lockout.MaxFailedAccessAttempts = identitySettings.LockoutFailedAttempts;
			})
				.AddRoles<ApplicationRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>();
			//
		}
	}
}
// ===========================================================================
