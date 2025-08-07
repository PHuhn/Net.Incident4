// ===========================================================================
using Microsoft.Extensions.Options;
//
using NSG.NetIncident4.Core;
using NSG.NetIncident4.Core.Domain.Entities;
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
startup.ConfigureSwaggerServices(builder.Services);
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
public partial class Program { }
