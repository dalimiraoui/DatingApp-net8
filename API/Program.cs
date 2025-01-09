using API.Extensions;
using API.Middleware;

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
    x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200", "https://localhost:4200")
);

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
