using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VendorManagementSystem.Application.IRepository;
using VendorManagementSystem.Infrastructure.Repository;
using VendorManagementSystem.Infrastructure.Data;
using VendorManagementSystem.Application.IServices;
using VendorManagementSystem.Infrastructure.Services;

namespace VendorManagementSystem.Infrastructure
{
    public static class InfrastructerDependencyInjection
    {
        public static IServiceCollection AddInfrastructer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("DbConnection")));

            services.AddScoped<IUserRepository, UserReopsitory>();
            services.AddScoped<IVendorRepository, VendorRepository>();
            services.AddScoped<ICategoryRepository, CategoryRespository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped<IVendorCategoryRepository, VendorCategoryRepository>();

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IErrorLoggingService, ErrorLoggingService>();
            return services;
        }
    }
}
