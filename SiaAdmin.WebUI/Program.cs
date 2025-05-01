using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication.Cookies;
using SiaAdmin.Application;
using SiaAdmin.Application.Interfaces.NotificationProcessor;
using SiaAdmin.Application.Validators.Survey;
using SiaAdmin.Infrastructure;
using SiaAdmin.Infrastructure.Filters;
using SiaAdmin.Persistence;
using SiaAdmin.WebUI.Extensions; 
using SiaAdmin.WebUI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options => { options.Filters.Add<ValidationFilter>();
    })
    .AddFluentValidation(configuration =>
        configuration.RegisterValidatorsFromAssemblyContaining<CreateSurveyValidator>())
    .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true).AddRazorRuntimeCompilation();
builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddControllers();
builder.Services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()));



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("RoleType", "Admin"));
    options.AddPolicy("User", policy => policy.RequireClaim("RoleType", "User"));
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    options.SlidingExpiration = true;
    options.LoginPath = "/User/Login";
    options.AccessDeniedPath = "/yetkisiz-sayfa";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
 // Her dakika kontrol et

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.ConfigureExceptionHandler<Program>(app.Services.GetRequiredService<ILogger<Program>>());
app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] {new LocalRequestsOnlyAuthorizationFilter() },
    DashboardTitle = "Reminder System Hangfire Dashboard"
});
RecurringJob.AddOrUpdate<INotificationProcessor>(
    "check-pending-notifications",
    processor => processor.ProcessPendingNotifications(),
    Cron.Minutely());

 
app.Run();
