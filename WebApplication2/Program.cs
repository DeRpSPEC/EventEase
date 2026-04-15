using Microsoft.EntityFrameworkCore;
using EventEase.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add Razor Pages support (we have a Pages/Index.cshtml in the project)
builder.Services.AddRazorPages();

// Register EF Core DbContext using SQL Server (DefaultConnection must be in appsettings.json)
builder.Services.AddDbContext<EventEaseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sql => sql.EnableRetryOnFailure())
);

var app = builder.Build();

// Apply pending EF Core migrations at startup. This ensures the database exists and schema is up to date during development.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<EventEaseDbContext>();
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Map Razor Pages
app.MapRazorPages();

app.Run();
