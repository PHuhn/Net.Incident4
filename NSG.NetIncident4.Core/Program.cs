// ===========================================================================
// Code: Program.cs
using Microsoft.OpenApi;
using NSG.NetIncident4.Core;
//
string _swaggerVersion = "v1";
string _swaggerNameVersion = "v1";
string _swaggerNameTitle = "NSG Net-Incident4.Core";
string _swaggerName = $"{_swaggerNameTitle} {_swaggerNameVersion}";
//
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var startup = new Startup(configuration);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
startup.ConfigureLoggingServices(builder.Services);
//
startup.AddAndConfigureControllers(builder.Services);
//
startup.ConfigureViewLocation(builder.Services);
/*
** Read values from the 'appsettings.json'
** * Add and configure email/notification services
** * Services like ping/whois
** * Applications information line name and phone #
** * Various authorization information
*/
startup.ConfigureNotificationServices(builder.Services);
// Add session for state and temp data provider
startup.ConfigureSessionServices(builder.Services);
// CORS
startup.ConfigureCorsServices(builder.Services);
// Cookie and JWT (swagger) authentication
startup.ConfigureAuthenticationServices(builder.Services);
// AdminRole/CompanyAdminRole/AnyUserRole
startup.ConfigureAuthorizationPolicyServices(builder.Services);
//
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
// startup.ConfigureSwaggerServices(builder.Services);
builder.Services.AddSwaggerGen(swagger =>
{
    // Generate the Default UI of Swagger Documentation
    swagger.SwaggerDoc(_swaggerVersion, new OpenApiInfo
    {
        Version = _swaggerNameVersion,
        Title = _swaggerNameTitle,
        Description = "Authentication and Authorization in ASP.NET 10 with JWT and Swagger"
    });
    // To Enable authorization using Swagger (JWT)
    // 1) Need to define a bearer scheme with a name (Bearer),
    // 2) Add requirements that specify that scheme via Id (Bearer)
    // had to downgrade Microsoft.OpenApi to 2.3.12 to not have compile err
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Enter 'Bearer' [space] and then your valid token in the text input below. Example: \"Authorization: Bearer {token}\"",
    });
    swagger.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});


// Build the configured application
WebApplication app = builder.Build();
//
if (app.Environment.IsDevelopment())
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
app.MapStaticAssets();
app.UseCookiePolicy();
app.UseRouting();
app.UseCors(startup.corsNamedOriginPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseSwagger();
//app.UseSwaggerUI(options =>
//{
//	options.SwaggerEndpoint("/swagger/v1/swagger.json", startup.swaggerName);
//	options.RoutePrefix = string.Empty;
//});
app.UseSwaggerUI(c =>
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NSG Net-Incident4.Core v1"));
//
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}").WithStaticAssets();
    _ = endpoints.MapRazorPages().WithStaticAssets();
});
//
app.Run();
//
//NSG.NetIncident4.Core.Domain.Entities.SeedData.Initialize(
//	context, roleManager, false).Wait();
//NSG.NetIncident4.Core.Domain.Entities.SeedData.SeedFakeIncidents(context, 1).Wait();
//
public partial class Program
{
}
