using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Persistence;
using CleanArchitecture.Infrastructure.SignalR.Hubs;
using Microsoft.Extensions.DependencyInjection.Areas.Moderator.ActionFilters;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebUIServices();
builder.Services.AddSignalR(o =>
{
    o.EnableDetailedErrors = true;
});
builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
{
    ProgressBar = false,
    PositionClass = ToastPositions.BottomRight
});
builder.Services.AddMiniProfiler().AddEntityFramework();
builder.Services.AddScoped<CheckModeratorActionFilter>();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
    await initialiser.InitialiseAsync();
    await initialiser.SeedAsync();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();

    // Initialise and seed database
   
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseMiniProfiler();

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();



app.UseRouting();
app.UseNToastNotify();

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<GameHub>("/gameHub");
    endpoints.MapControllers();
    endpoints.MapRazorPages();

});

app.MapFallbackToFile("index.html");

app.Run();