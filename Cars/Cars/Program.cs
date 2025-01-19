using Cars.ApplicationServices.Services;
using Cars.Data;
using Cars.Core.ServiceInterface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDbContext<CarsContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Cars")); // M‰‰rab migratsioonide kogumi
});

// Register application services
builder.Services.AddScoped<ICarsServices, CarsServices>(); // Add your service here

// Add MVC support
builder.Services.AddControllersWithViews(); // For MVC architecture
builder.Services.AddEndpointsApiExplorer(); // For API endpoints if needed

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add authorization middleware
app.UseAuthorization();

// Set up default route for controllers
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the application
app.Run();
