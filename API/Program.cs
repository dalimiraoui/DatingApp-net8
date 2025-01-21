using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using API.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);

// Adds custom identity configuration to the service collection.
// The "AddApplicationIdentity" method is an extension method for IServiceCollection.
// We pass only one explicit argument (builder.Configuration) because of how extension methods work in C#.
// 
// 1. `builder.Services`: This is an instance of IServiceCollection, which holds all registered services for dependency injection.
//    - It is passed **implicitly** as the first argument to the extension method because the method is defined with "this IServiceCollection services".
//    - In this case, `builder.Services` is automatically passed to the method as the first parameter by the C# compiler.
//
// 2. `builder.Configuration`: This is the explicit second parameter we pass.
//    - It is the IConfiguration instance that provides access to application configuration settings (e.g., appsettings.json).
//    - This is used in the extension method to retrieve the "TokenKey" for configuring authentication.
//
// The result is a clean and intuitive way to extend IServiceCollection without explicitly passing the `builder.Services` object.
builder.Services.AddApplicationIdentity(builder.Configuration);


builder.Services.AddCors();

var app = builder.Build();

// Custom middleware to handle exceptions globally.
// It catches unhandled exceptions, logs them, and returns a structured error response in JSON format.
app.UseMiddleware<ExceptionMiddleware>();

// Built-in middleware used in development mode to provide detailed error pages for unhandled exceptions.
// This should not be used in production environments for security reasons.
//app.UseDeveloperExceptionPage();

// configure http request pipilines
app.UseCors(
    x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200", "https://localhost:4200")
);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<PresenceHub>("hubs/message");

// Create a new scope for resolving services with a controlled lifetime.
// This ensures that any scoped or transient services resolved in this block are properly disposed of when the scope ends.
using var scope = app.Services.CreateScope();

// Retrieve the scoped service provider to resolve dependencies within the created scope.
var services = scope.ServiceProvider;

try
{
    // Resolve the application's database context (DataContext) from the scoped service provider.
    // Throws an exception if the service cannot be resolved, ensuring the required service is available.
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    
    // Apply any pending EF Core migrations to the database.
    // This ensures the database schema matches the application's current data model.
    // Runs asynchronously to avoid blocking the main thread.
    await context.Database.MigrateAsync();
    
    // Seed the database with initial data (e.g., users, roles, or default values).
    // This is typically used to ensure the application starts with essential data.
    await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
    // Resolve the logger service for the Program class to log errors.
    var logger = services.GetRequiredService<ILogger<Program>>();
    
    // Log the exception that occurred during database migration or seeding.
    // Provides helpful information for debugging startup issues.
    logger.LogError(ex, "An error occurred during migration");
}

// Start the web application and begin listening for incoming HTTP requests.
app.Run();

