using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Northwind.Mvc.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLogging(opt =>
{
    opt.AddSimpleConsole(options => options.TimestampFormat = "[HH:mm:ss.fffff] ");
});
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddNorthwindContext();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddOutputCache(options =>
{
    options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(20);
    options.AddPolicy("views", p => p.SetVaryByQuery("alertstyle"));
});

builder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // enable role management
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient("Northwind.WebApi", options => {
    options.BaseAddress = new Uri("https://localhost:7001");
    options.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue(mediaType: "application/json", quality: 1.0));

    options.DefaultRequestVersion = HttpVersion.Version30;
    options.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseOutputCache();
app
    .MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}")
    .CacheOutput("views");
app.MapRazorPages();
app.MapGet("/notcached", () => DateTime.Now.ToString());
app.MapGet("/cached", () => DateTime.Now.ToString()).CacheOutput();

app.Run();
