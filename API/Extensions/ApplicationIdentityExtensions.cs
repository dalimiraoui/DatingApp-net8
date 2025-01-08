
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;

// Static class to define extension methods for application identity configuration
public static class ApplicationIdentityExtensions
{
    // Extension method to add identity-related configurations to the IServiceCollection
    public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
    {
        // Add JWT-based authentication to the service collection
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            // Retrieve the TokenKey from the configuration (e.g., appsettings.json)
            // If the key does not exist, throw an exception
            var tokenKey = config["TokenKey"] ?? throw new Exception("TokenKey not exists");

            // Configure the JWT token validation parameters
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // Ensure that the token is signed with a valid security key
                ValidateIssuerSigningKey = true,

                // Specify the signing key used to validate the JWT's signature
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),

                // Disable issuer validation (not validating who issued the token)
                ValidateIssuer = false,

                // Disable audience validation (not validating who the token is intended for)
                ValidateAudience = false,
            };
        });

        // Return the modified IServiceCollection for chaining
        return services;
    }
}

