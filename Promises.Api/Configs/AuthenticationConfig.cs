using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Promises.Domain.Identity;
using Promises.Persistence;

namespace Promises.Api.Configs;

public static class AuthenticationConfig
{
    public static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
    {
       services.AddScoped<RoleManager<Role>>();
       services.AddScoped<UserManager<User>>();
       
       services.AddIdentity<User, Role>()
           .AddUserManager<UserManager<User>>()
           .AddRoles<Role>()
           .AddRoleManager<RoleManager<Role>>()
           .AddEntityFrameworkStores<ApplicationContext>()
           .AddDefaultTokenProviders();
       
       services.Configure<IdentityOptions>(options =>
       {
           options.Password.RequireDigit = false;
           options.Password.RequireLowercase = false;
           options.Password.RequireNonAlphanumeric = false;
           options.Password.RequireUppercase = false;
           options.Password.RequiredLength = 6;
           options.Password.RequiredUniqueChars = 1;
       
           options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
           options.Lockout.MaxFailedAccessAttempts = 5;
           options.Lockout.AllowedForNewUsers = true;
       
           options.User.AllowedUserNameCharacters =
               "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
           options.User.RequireUniqueEmail = true;
       
       });
       services.AddAuthentication(options =>
           {
               options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           })
           .AddJwtBearer(options =>
           {
               options.SaveToken = true;
               options.IncludeErrorDetails = true;
       
               options.TokenValidationParameters = new TokenValidationParameters()
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = configuration["TokenSetting:Issuer"],
                   ValidAudience = configuration["TokenSetting:Audience"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenSetting:Key"])),
               };
           });
        return services;
    }
}


