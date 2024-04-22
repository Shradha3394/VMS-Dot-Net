using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VendorManagementSystem.Application.Exceptions;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Application.Services;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            /*var valassembly = typeof(DependencyInjectionExtensions).Assembly;
            services.AddValidatorsFromAssembly(valassembly);*/
           
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                };
            });

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IVendorService, VendorService>() ;
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ICategoryService, CategoryService>();
            return services;
        }
    }
}
