using Cars.ApplicationServices.Services;
using Cars.Data;
using Cars.Core.ServiceInterface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CarsContext>(options =>
{
    // Set up the connection to the SQL Server and specify the migration assembly.
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Cars")); // M‰‰rab migratsioonide kogumi
});

// Register the application services (add your custom services here)
builder.Services.AddScoped<ICarsServices, CarsServices>(); // Auto teenuse lisamine

// Add MVC support (for controllers and views)
builder.Services.AddControllersWithViews(); // For MVC architecture
builder.Services.AddEndpointsApiExplorer(); // For API endpoints if needed

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Developer exception page for debugging during development
    app.UseDeveloperExceptionPage();
}
else
{
    // Use a generic error page in production and enforce HSTS
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection(); // Enforce HTTPS
app.UseStaticFiles(); // Serve static files (e.g., CSS, JavaScript)

app.UseRouting(); // Enable routing

// Add authorization middleware
app.UseAuthorization();

// Set up the default route for controllers and actions
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cars}/{action=Index}/{id?}"); // Redirect to CarsController by default

// Run the application
app.Run();
